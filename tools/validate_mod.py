#!/usr/bin/env python3
"""Validates IsoRim mod structure and content.

Checks:
- Well-formed XML in About.xml, LoadFolders.xml, Models.xml, KeyBindings.xml
- Every model path named in Defs/Models.xml (and 1.7/Defs/Models.xml when present) exists on disk
- Every .obj string in Source/*.cs and Source17/*.cs exists under Models/
- Every material referenced by usemtl in Models/*.obj exists in Models/isorim.mtl
- Every IsoRim/ texture referenced in Source/*.cs and Source17/*.cs exists under Textures/IsoRim/
- Every LUT filename referenced in Source/Looks.cs and Source17/Looks.cs exists under LUTs/
- When --check-assemblies is passed, 1.5/Assemblies/IsoRim.dll, 1.6/Assemblies/IsoRim.dll, and
  1.7/Assemblies/IsoRim.dll exist and are non-empty
- Prepared 1.7 model/gear references, adjacent material libraries, face indices and finite normals
- --mod-dir selects a staged mod; absent authored normals remain valid geometric fallback
"""

import argparse
from decimal import Decimal, InvalidOperation
import glob
import json
import math
import os
from pathlib import Path
import re
import struct
import sys
import xml.etree.ElementTree as ET


def validate_xml_files(root: Path) -> list[str]:
    errors = []
    xml_paths = [
        root / "About" / "About.xml",
        root / "About" / "LoadFolders.xml",
        root / "Defs" / "Models.xml",
        root / "Defs" / "KeyBindings.xml",
    ]
    for p in xml_paths:
        if not p.exists():
            errors.append(f"Missing XML file: {p}")
            continue
        try:
            ET.parse(p)
        except Exception as e:
            errors.append(f"Malformed XML in {p}: {e}")
    for models_17 in (root / "1.7" / "Defs").rglob("*.xml"):
        try:
            ET.parse(models_17)
        except Exception as e:
            errors.append(f"Malformed XML in {models_17}: {e}")
    return errors


def _models_xml_paths(root: Path) -> list[Path]:
    paths = [root / "Defs" / "Models.xml"]
    if (root / "Defs/WornGear.xml").exists():
        paths.append(root / "Defs/WornGear.xml")
    paths.extend(sorted((root / "1.7/Defs").rglob("*.xml")))
    return paths


def validate_model_defs(root: Path) -> list[str]:
    errors = []
    for models_xml in _models_xml_paths(root):
        if not models_xml.exists():
            errors.append(f"Missing {models_xml}")
            continue
        try:
            tree = ET.parse(models_xml)
        except Exception as e:
            errors.append(f"Malformed XML in {models_xml}: {e}")
            continue

        for elem in tree.iter():
            if elem.text and ".obj" in elem.text.lower():
                rel_path = elem.text.strip().replace("\\", "/")
                target = root / rel_path
                if not target.resolve().is_relative_to(root.resolve()):
                    errors.append(f"Model path in {models_xml.relative_to(root)} escapes its defining mod: {rel_path}")
                elif not target.is_file():
                    errors.append(f"Model path in {models_xml.relative_to(root)} not found on disk: {rel_path}")
    return errors


def _source_dirs(root: Path) -> list[Path]:
    dirs = []
    for name in ("Source", "Source17"):
        d = root / name
        if d.is_dir():
            dirs.append(d)
    return dirs


def validate_source_obj_strings(root: Path) -> list[str]:
    errors = []
    obj_pattern = re.compile(r'"([^"]+\.obj)"')
    for source_dir in _source_dirs(root):
        for cs_file in source_dir.glob("*.cs"):
            if cs_file.name.startswith("ProbeNormals"):
                continue  # These guarded test cases generate their own inputs outside production assets.
            try:
                content = cs_file.read_text(encoding="utf-8")
            except Exception as e:
                errors.append(f"Could not read {cs_file}: {e}")
                continue
            for match in obj_pattern.finditer(content):
                val = match.group(1).replace("\\", "/")
                if ((root / val).exists() or (root / "Models" / Path(val).name).exists()
                        or (source_dir.name == "Source17" and (root / "1.7/Models" / Path(val).name).exists())):
                    continue
                errors.append(f"{cs_file.relative_to(root)} references missing .obj: {val}")
    return errors


def _float32(value: float) -> float:
    return struct.unpack("f", struct.pack("f", value))[0]


SCALAR_TOKEN = re.compile(r"[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)(?:[eE][+-]?[0-9]+)?", re.ASCII)
INDEX_TOKEN = re.compile(r"[+-]?[0-9]+", re.ASCII)


def prepared_single(token: str) -> float:
    """Round the exact decimal ratio directly to binary32, nearest/even."""
    if not SCALAR_TOKEN.fullmatch(token):
        raise ValueError("invalid prepared scalar")
    sign = 0x80000000 if token.startswith("-") else 0
    mantissa, _, exponent_text = token.lstrip("+-").lower().partition("e")
    digits = mantissa.replace(".", "").lstrip("0")
    offset = len(digits) - len(mantissa.partition(".")[2]) - 1
    exponent_digits = exponent_text.lstrip("+-").lstrip("0") or "0"
    # Saturate only when the exponent provably dominates the entire mantissa and
    # both binary32 decimal bounds. No significant-digit or exponent-text cap.
    bound = str(abs(offset) + 47)
    dominates = len(exponent_digits) > len(bound) or (len(exponent_digits) == len(bound) and exponent_digits > bound)
    exponent = (abs(offset) + 47) if dominates else int(exponent_digits)
    if exponent_text.startswith("-"):
        exponent = -exponent
    adjusted = offset + exponent
    if not digits or adjusted < -46:
        bits = 0
    elif adjusted > 38:
        bits = 0x7f800000
    else:
        value = Decimal(token)
        numerator, denominator = value.copy_abs().as_integer_ratio()
        exponent = numerator.bit_length() - denominator.bit_length()
        below = numerator < denominator << exponent if exponent >= 0 else numerator << -exponent < denominator
        if below:
            exponent -= 1
        shift = 149 if exponent < -126 else 23 - exponent
        scaled, divisor = (numerator << shift, denominator) if shift >= 0 else (numerator, denominator << -shift)
        rounded, remainder = divmod(scaled, divisor)
        if remainder * 2 > divisor or (remainder * 2 == divisor and rounded & 1):
            rounded += 1
        if exponent < -126:
            bits = rounded  # Includes rounding up to the smallest normal.
        else:
            if rounded == 1 << 24:
                exponent += 1
                rounded >>= 1
            bits = 0x7f800000 if exponent > 127 else ((exponent + 127) << 23) | (rounded - (1 << 23))
    return struct.unpack("f", struct.pack("I", sign | bits))[0]


def _prepared_index(token: str, count: int) -> int:
    if not INDEX_TOKEN.fullmatch(token):
        raise ValueError("invalid signed Int32 index")
    digits = token.lstrip("+-").lstrip("0") or "0"
    if len(digits) > 10:
        raise ValueError("index outside signed Int32")
    value = int(("-" if token.startswith("-") else "") + digits)
    if not -2147483648 <= value <= 2147483647:
        raise ValueError("index outside signed Int32")
    resolved = count + value if value < 0 else value - 1
    if value == 0 or not 0 <= resolved < count:
        raise ValueError("unresolved index")
    return resolved


def _prepared_records(path: Path):
    # File.ReadAllLines has physical CR/LF/CRLF boundaries; only SP/HT split tokens.
    text = path.read_bytes().decode("utf-8-sig")
    for number, raw in enumerate(re.split(r"\r\n|\r|\n", text), 1):
        line = raw.strip(" \t")
        if not line or line.startswith("#"):
            continue
        if any((ord(char) < 32 and char != "\t") or (char.isspace() and char not in " \t") for char in line):
            yield number, line, [], "unsupported record separator/control"
            continue
        parts = re.split(r"[ \t]+", line)
        consumed = parts[0] in {"v", "vt", "vn", "f", "mtllib", "usemtl", "Kd", "newmtl"}
        yield number, line, parts, "inline comment on consumed record" if consumed and "#" in line else None


def _prepared_triangle(a, b, c):
    try:
        u = [_float32(b[i] - a[i]) for i in range(3)]
        v = [_float32(c[i] - a[i]) for i in range(3)]
        n = [_float32(_float32(u[i] * v[j]) - _float32(u[j] * v[i])) for i, j in ((1, 2), (2, 0), (0, 1))]
        square = [_float32(value * value) for value in n]
        length = _float32(_float32(square[0] + square[1]) + square[2])
        return not length < _float32(1e-12)
    except OverflowError:
        # The production geometric branch only skips a squared magnitude below its cutoff.
        return True


def validate_prepared_objs(root: Path, report_scalars: bool = True) -> tuple[list[str], list[str]]:
    errors = []
    checked = []
    scalar_bits = []
    for obj in sorted((root / "1.7/Models").rglob("*.obj", case_sensitive=False)):
        relative = obj.relative_to(root).as_posix()
        checked.append(relative)
        positions, textures, normals = [], [], []
        triangles = 0
        defined, used = set(), set()
        for number, line, parts, record_error in _prepared_records(obj):
            label = f"{relative}:{number}"
            if record_error:
                errors.append(f"{label}: {record_error}")
                continue
            kind = parts[0]
            if kind in ("v", "vt", "vn"):
                streams = {"v": positions, "vt": textures, "vn": normals}
                streams[kind].append(None)  # Preserve declaration ordinals, including invalid vn.
                count = 2 if kind == "vt" else 3
                allowed = {"v": (4, 7), "vt": (3, 4), "vn": (4,)}
                try:
                    if len(parts) not in allowed[kind]:
                        raise ValueError("unsupported numeric arity")
                    values = [prepared_single(token) for token in parts[1:]]
                    if report_scalars:
                        scalar_bits.extend({"path": relative, "line": number, "kind": kind, "component": i,
                                            "token": token, "bits": f"{struct.unpack('I', struct.pack('f', value))[0]:08X}"}
                                           for i, (token, value) in enumerate(zip(parts[1:], values)))
                    if not all(math.isfinite(value) for value in values):
                        raise ValueError("nonfinite scalar")
                    if kind == "vn":
                        square = [_float32(value * value) for value in values]
                        length_squared = _float32(_float32(square[0] + square[1]) + square[2])
                        if not math.isfinite(length_squared) or length_squared < _float32(1e-12):
                            raise ValueError("unusable float32 magnitude")
                        if _float32(math.sqrt(length_squared)) <= _float32(1e-5):
                            raise ValueError("unusable float32 normalization")
                    streams[kind][-1] = values[:count]
                except (ValueError, OverflowError, InvalidOperation) as error:
                    role = "finite nonzero normal values with usable float32 magnitude" if kind == "vn" else "finite coordinates/colour"
                    errors.append(f"{label}: {kind} requires {role}; scalar/arity: {error}")
            elif kind == "mtllib":
                library = obj.parent / line[6:].strip(" \t")
                if len(parts) < 2 or not library.resolve().is_relative_to(root.resolve()) or not library.is_file():
                    errors.append(f"{label}: adjacent material library missing or escaping: {library}")
                    continue
                for line_number, material_line, material, material_error in _prepared_records(library):
                    mlabel = f"{library.relative_to(root)}:{line_number}"
                    if material_error:
                        errors.append(f"{mlabel}: {material_error}")
                    elif material[0] == "newmtl":
                        if len(material) != 2:
                            errors.append(f"{mlabel}: unsupported material name payload")
                        else:
                            defined.add(material[1])
                    elif material[0] == "Kd":
                        try:
                            if len(material) != 4:
                                raise ValueError("unsupported colour arity")
                            colour = [prepared_single(token) for token in material[1:]]
                            if report_scalars:
                                scalar_bits.extend({"path": library.relative_to(root).as_posix(), "line": line_number, "kind": "Kd", "component": i,
                                                    "token": token, "bits": f"{struct.unpack('I', struct.pack('f', value))[0]:08X}"}
                                                   for i, (token, value) in enumerate(zip(material[1:], colour)))
                            if not all(math.isfinite(value) for value in colour):
                                raise ValueError("nonfinite colour")
                        except (ValueError, OverflowError, InvalidOperation) as error:
                            errors.append(f"{mlabel}: invalid material colour scalar: {error}")
            elif kind == "usemtl":
                if len(parts) != 2:
                    errors.append(f"{label}: unsupported material payload")
                else:
                    used.add(parts[1])
            elif kind == "f":
                face = []
                for corner in parts[1:]:
                    indices = corner.split("/")
                    if len(indices) > 3:
                        errors.append(f"{label}: unsupported slash payload")
                    for index, (name, stream) in enumerate((("position", positions), ("texture", textures), ("normal", normals))):
                        token = indices[index] if index < len(indices) else ""
                        if not token and index > 0:
                            continue
                        try:
                            resolved = _prepared_index(token, len(stream))
                            if stream[resolved] is None:
                                raise ValueError("invalid declaration ordinal")
                            if index == 0:
                                face.append(resolved)
                        except ValueError as error:
                            errors.append(f"{label}: invalid {name} index '{token}': {error}")
                if len(parts) < 4:
                    errors.append(f"{label}: face needs three corners")
                if len(face) == len(parts) - 1:
                    triangles += sum(_prepared_triangle(positions[face[0]], positions[face[i]], positions[face[i + 1]])
                                     for i in range(1, len(face) - 1))
        if triangles == 0:
            errors.append(f"{relative}: no face emits a triangle")
        for material in sorted(used - defined):
            errors.append(f"{relative}: undefined adjacent material '{material}'")
    # Full CLI clients can compare independently specified Single bits even when normalizing hides them.
    if report_scalars:
        print("Prepared scalar bits: " + json.dumps(scalar_bits))
    return errors, checked


def validate_materials(root: Path) -> list[str]:
    errors = []
    models_dir = root / "Models"
    mtl_path = models_dir / "isorim.mtl"
    if not mtl_path.exists():
        return [f"Missing {mtl_path}"]

    defined_mtls = set()
    for mtl_file in models_dir.glob("*.mtl"):
        try:
            mtl_text = mtl_file.read_text(encoding="utf-8")
        except Exception as e:
            errors.append(f"Could not read {mtl_file}: {e}")
            continue
        for line in mtl_text.splitlines():
            line = line.strip()
            if line.startswith("newmtl "):
                defined_mtls.add(line[7:].strip())
    if not defined_mtls and mtl_path.exists():
        try:
            mtl_text = mtl_path.read_text(encoding="utf-8")
        except Exception as e:
            return [f"Could not read {mtl_path}: {e}"]
        for line in mtl_text.splitlines():
            line = line.strip()
            if line.startswith("newmtl "):
                defined_mtls.add(line[7:].strip())

    models_dir = root / "Models"
    for obj_file in models_dir.glob("*.obj"):
        try:
            obj_text = obj_file.read_text(encoding="utf-8")
        except Exception as e:
            errors.append(f"Could not read {obj_file}: {e}")
            continue
        for line in obj_text.splitlines():
            line = line.strip()
            if line.startswith("usemtl "):
                mat = line[7:].strip()
                if mat not in defined_mtls:
                    errors.append(f"Model {obj_file.name} uses undefined material '{mat}' (not in isorim.mtl)")
    return errors


def validate_source_textures(root: Path) -> list[str]:
    errors = []
    tex_pattern = re.compile(r'"(IsoRim/[^"]+)"')
    textures_dir = root / "Textures"

    for source_dir in _source_dirs(root):
        for cs_file in source_dir.glob("*.cs"):
            try:
                content = cs_file.read_text(encoding="utf-8")
            except Exception as e:
                errors.append(f"Could not read {cs_file}: {e}")
                continue
            for match in tex_pattern.finditer(content):
                val = match.group(1).replace("\\", "/")
                if val == "IsoRim/LUT":
                    continue
                if val.endswith("/"):
                    target_dir = textures_dir / val
                    if not target_dir.is_dir():
                        errors.append(f"{cs_file.relative_to(root)} references missing texture folder: {val}")
                else:
                    target_png = textures_dir / (val + ".png")
                    target_raw = textures_dir / val
                    if not (target_png.exists() or target_raw.exists()):
                        errors.append(f"{cs_file.relative_to(root)} references missing texture: {val}")
    return errors


def validate_luts(root: Path) -> list[str]:
    errors = []
    luts_dir = root / "LUTs"
    for source_dir in _source_dirs(root):
        looks_cs = source_dir / "Looks.cs"
        if not looks_cs.exists():
            continue
        try:
            content = looks_cs.read_text(encoding="utf-8")
        except Exception as e:
            errors.append(f"Could not read {looks_cs}: {e}")
            continue

        names = re.findall(r'Add\("([^"]+)"', content)
        for name in names:
            if name == "Custom":
                continue
            target = luts_dir / (name + ".png")
            if not target.exists():
                errors.append(f"{looks_cs.relative_to(root)} references missing LUT file: {target.name}")
    return errors


def validate_assemblies(root: Path) -> list[str]:
    errors = []
    load = root / "About" / "About.xml"
    if not load.exists():
        return [f"Missing {load}"]
    try:
        tree = ET.parse(load)
    except Exception as e:
        return [f"Malformed XML in {load}: {e}"]
    versions = []
    for el in tree.findall(".//supportedVersions/li"):
        if el.text:
            versions.append(el.text.strip())
    if not versions:
        errors.append(f"No game versions named in {load}")
    for version in versions:
        asm = root / version / "Assemblies" / "IsoRim.dll"
        if not asm.exists():
            errors.append(f"Assembly missing: {asm}")
        elif asm.stat().st_size == 0:
            errors.append(f"Assembly is empty: {asm}")
    return errors


def main() -> int:
    parser = argparse.ArgumentParser(description="Validate IsoRim mod structure and assets")
    parser.add_argument("--check-assemblies", action="store_true", help="Check that 1.5, 1.6, and 1.7 assemblies exist and are non-empty")
    parser.add_argument("--mod-dir", type=Path, help="Validate a staged mod instead of this repository")
    parser.add_argument("--summary", action="store_true", help="Report production asset counts without per-scalar diagnostics")
    args = parser.parse_args()

    root = (args.mod_dir or Path(__file__).resolve().parent.parent).resolve()

    all_errors = []
    all_errors.extend(validate_xml_files(root))
    all_errors.extend(validate_model_defs(root))
    all_errors.extend(validate_source_obj_strings(root))
    all_errors.extend(validate_materials(root))
    all_errors.extend(validate_source_textures(root))
    all_errors.extend(validate_luts(root))
    prepared_errors, prepared_paths = validate_prepared_objs(root, report_scalars=not args.summary)
    all_errors.extend(prepared_errors)

    if args.check_assemblies:
        all_errors.extend(validate_assemblies(root))

    if all_errors:
        for err in all_errors:
            print(f"Validation error: {err}", file=sys.stderr)
        return 1

    prepared_paths.extend(path.relative_to(root).as_posix() for path in _models_xml_paths(root) if path.is_relative_to(root / "1.7"))
    if args.summary:
        print(f"Prepared inputs checked: {len(prepared_paths)}")
    else:
        print("Prepared inputs: " + json.dumps(sorted(prepared_paths)))
    print("IsoRim validation passed.")
    return 0


if __name__ == "__main__":
    sys.exit(main())

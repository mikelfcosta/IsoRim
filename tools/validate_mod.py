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


def validate_prepared_objs(root: Path) -> tuple[list[str], list[str]]:
    errors = []
    checked = []
    for obj in sorted((root / "1.7/Models").rglob("*.obj", case_sensitive=False)):
        relative = obj.relative_to(root).as_posix()
        checked.append(relative)
        positions = textures = normals = 0
        faces = 0
        defined = set()
        used = set()
        for number, raw in enumerate(obj.read_text(encoding="utf-8").splitlines(), 1):
            parts = raw.split("#", 1)[0].split()
            if not parts:
                continue
            label = f"{relative}:{number}"
            kind = parts[0]
            if kind in ("v", "vt", "vn"):
                if kind == "v":
                    positions += 1
                elif kind == "vt":
                    textures += 1
                else:
                    normals += 1
                count = 2 if kind == "vt" else 3
                try:
                    values = [float(token) for token in parts[1:count + 1]]
                    if len(values) != count or not all(math.isfinite(value) for value in values):
                        raise ValueError()
                    if kind == "vn":
                        # ObjLoader parses Singles and evaluates Vector3.sqrMagnitude in float32.
                        values = [_float32(value) for value in values]
                        squares = [_float32(value * value) for value in values]
                        length_squared = _float32(_float32(squares[0] + squares[1]) + squares[2])
                        if not math.isfinite(length_squared) or length_squared < _float32(1e-12):
                            raise ValueError()
                except (ValueError, OverflowError):
                    errors.append(f"{label}: {kind} must contain finite" + (" nonzero normal values with usable float32 magnitude" if kind == "vn" else " coordinates"))
            elif kind == "mtllib":
                library = obj.parent / raw.strip()[6:].strip()
                if not library.resolve().is_relative_to(root.resolve()) or not library.is_file():
                    errors.append(f"{label}: adjacent material library missing or escaping: {library}")
                    continue
                for line_number, line in enumerate(library.read_text(encoding="utf-8").splitlines(), 1):
                    material = line.split("#", 1)[0].split()
                    if not material:
                        continue
                    if material[0] == "newmtl":
                        if len(material) < 2:
                            errors.append(f"{library}:{line_number}: missing material name")
                        else:
                            defined.add(material[1])
                    elif material[0] == "Kd":
                        try:
                            colour = [float(token) for token in material[1:4]]
                            if len(colour) != 3 or not all(math.isfinite(value) for value in colour):
                                raise ValueError()
                        except ValueError:
                            errors.append(f"{library}:{line_number}: invalid material colour")
            elif kind == "usemtl" and len(parts) > 1:
                used.add(parts[1])
            elif kind == "f":
                valid_positions = len(parts) >= 4
                if len(parts) < 4:
                    errors.append(f"{label}: face needs three corners")
                for corner in parts[1:]:
                    indices = corner.split("/")
                    for index, (name, count) in enumerate((("position", positions), ("texture", textures), ("normal", normals))):
                        token = indices[index] if index < len(indices) else ""
                        if not token and index > 0:
                            continue  # Missing authored normals explicitly select geometric fallback.
                        try:
                            value = int(token)
                            resolved = count + value if value < 0 else value - 1
                            if value == 0 or not 0 <= resolved < count:
                                raise ValueError()
                        except ValueError:
                            if index == 0:
                                valid_positions = False
                            errors.append(f"{label}: invalid {name} index '{token}'")
                if valid_positions:
                    faces += 1
        if faces == 0:
            errors.append(f"{relative}: no face with at least three valid position references")
        for material in sorted(used - defined):
            errors.append(f"{relative}: undefined adjacent material '{material}'")
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
    args = parser.parse_args()

    root = (args.mod_dir or Path(__file__).resolve().parent.parent).resolve()

    all_errors = []
    all_errors.extend(validate_xml_files(root))
    all_errors.extend(validate_model_defs(root))
    all_errors.extend(validate_source_obj_strings(root))
    all_errors.extend(validate_materials(root))
    all_errors.extend(validate_source_textures(root))
    all_errors.extend(validate_luts(root))
    prepared_errors, prepared_paths = validate_prepared_objs(root)
    all_errors.extend(prepared_errors)

    if args.check_assemblies:
        all_errors.extend(validate_assemblies(root))

    if all_errors:
        for err in all_errors:
            print(f"Validation error: {err}", file=sys.stderr)
        return 1

    prepared_paths.extend(path.relative_to(root).as_posix() for path in _models_xml_paths(root) if path.is_relative_to(root / "1.7"))
    print("Prepared inputs: " + json.dumps(sorted(prepared_paths)))
    print("IsoRim validation passed.")
    return 0


if __name__ == "__main__":
    sys.exit(main())

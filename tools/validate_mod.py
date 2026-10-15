#!/usr/bin/env python3
"""Validates IsoRim mod structure and content.

Checks:
- Well-formed XML in About.xml, LoadFolders.xml, Models.xml, KeyBindings.xml
- Every model path named in Defs/Models.xml exists on disk
- Every .obj string in Source/*.cs exists under Models/
- Every material referenced by usemtl in Models/*.obj exists in Models/isorim.mtl
- Every IsoRim/ texture referenced in Source/*.cs exists under Textures/IsoRim/
- Every LUT filename referenced in Source/Looks.cs exists under LUTs/
- When --check-assemblies is passed, 1.5/Assemblies/IsoRim.dll and 1.6/Assemblies/IsoRim.dll exist and are non-empty
"""

import argparse
import glob
import os
from pathlib import Path
import re
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
    return errors


def validate_model_defs(root: Path) -> list[str]:
    errors = []
    models_xml = root / "Defs" / "Models.xml"
    if not models_xml.exists():
        return [f"Missing {models_xml}"]
    try:
        tree = ET.parse(models_xml)
    except Exception as e:
        return [f"Malformed XML in {models_xml}: {e}"]

    for elem in tree.iter():
        if elem.text and ".obj" in elem.text:
            rel_path = elem.text.strip().replace("\\", "/")
            target = root / rel_path
            if not target.exists():
                errors.append(f"Model path in Defs/Models.xml not found on disk: {rel_path}")
    return errors


def validate_source_obj_strings(root: Path) -> list[str]:
    errors = []
    source_dir = root / "Source"
    obj_pattern = re.compile(r'"([^"]+\.obj)"')
    for cs_file in source_dir.glob("*.cs"):
        try:
            content = cs_file.read_text(encoding="utf-8")
        except Exception as e:
            errors.append(f"Could not read {cs_file}: {e}")
            continue
        for match in obj_pattern.finditer(content):
            val = match.group(1).replace("\\", "/")
            if (root / val).exists() or (root / "Models" / Path(val).name).exists():
                continue
            errors.append(f"Source file {cs_file.name} references missing .obj: {val}")
    return errors


def validate_materials(root: Path) -> list[str]:
    errors = []
    mtl_path = root / "Models" / "isorim.mtl"
    if not mtl_path.exists():
        return [f"Missing {mtl_path}"]

    defined_mtls = set()
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
    source_dir = root / "Source"
    tex_pattern = re.compile(r'"(IsoRim/[^"]+)"')
    textures_dir = root / "Textures"

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
                    errors.append(f"Source file {cs_file.name} references missing texture folder: {val}")
            else:
                target_png = textures_dir / (val + ".png")
                target_raw = textures_dir / val
                if not (target_png.exists() or target_raw.exists()):
                    errors.append(f"Source file {cs_file.name} references missing texture: {val}")
    return errors


def validate_luts(root: Path) -> list[str]:
    errors = []
    looks_cs = root / "Source" / "Looks.cs"
    if not looks_cs.exists():
        return [f"Missing {looks_cs}"]

    try:
        content = looks_cs.read_text(encoding="utf-8")
    except Exception as e:
        return [f"Could not read {looks_cs}: {e}"]

    luts_dir = root / "LUTs"
    names = re.findall(r'Add\("([^"]+)"', content)
    for name in names:
        if name == "Custom":
            continue
        target = luts_dir / (name + ".png")
        if not target.exists():
            errors.append(f"Looks.cs references missing LUT file: {target.name}")
    return errors


def validate_assemblies(root: Path) -> list[str]:
    errors = []
    load = root / "About" / "LoadFolders.xml"
    if not load.exists():
        return [f"Missing {load}"]
    try:
        tree = ET.parse(load)
    except Exception as e:
        return [f"Malformed XML in {load}: {e}"]
    versions = []
    for el in list(tree.getroot()):
        tag = el.tag
        if tag.startswith("v") and len(tag) > 1 and tag[1].isdigit():
            versions.append(tag[1:])
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
    args = parser.parse_args()

    root = Path(__file__).resolve().parent.parent

    all_errors = []
    all_errors.extend(validate_xml_files(root))
    all_errors.extend(validate_model_defs(root))
    all_errors.extend(validate_source_obj_strings(root))
    all_errors.extend(validate_materials(root))
    all_errors.extend(validate_source_textures(root))
    all_errors.extend(validate_luts(root))

    if args.check_assemblies:
        all_errors.extend(validate_assemblies(root))

    if all_errors:
        for err in all_errors:
            print(f"Validation error: {err}", file=sys.stderr)
        return 1

    print("IsoRim validation passed.")
    return 0


if __name__ == "__main__":
    sys.exit(main())

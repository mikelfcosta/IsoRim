"""Generated, disposable inputs for the real-game asset intake witnesses."""

from pathlib import Path

INTERNAL_FILES = [
    "probe.obj", "hair_probe.obj", "hair_version_only.obj", "roof_square.obj", "roof_triangle.obj", "roof_rim.obj",
    "roof_rim90.obj", "roof_rim_diagonal.obj", "wind_rotor.obj", "door_leaf.obj",
    "door_lintel.obj", "gate_leaf.obj", "gate_posts.obj", "fx_flame.obj",
    "fx_flame_core.obj", "fx_puff.obj",
]


def mesh(width: int, normals: bool = True) -> str:
    # Cell scale 1, ground y=0, origin pivot, OBJ Z reflected by the production loader.
    text = f"mtllib sample.mtl\nv 0 0 0\nv {width} 0 0\nv 0 1 0\nvt 0 0\nvt 1 0\nvt 0 1\n"
    text += "vn 0 0.6 -0.8\n" if normals else ""
    return text + "usemtl sample\n" + ("f 1/1/1 2/2/1 3/3/1\n" if normals else "f 1/1 2/2 3/3\n")


def write_model(directory: Path, name: str, width: int, normals: bool = True) -> None:
    directory.mkdir(parents=True, exist_ok=True)
    (directory / name).write_text(mesh(width, normals), encoding="utf-8")
    (directory / "sample.mtl").write_text("newmtl sample\nKd 0.2 0.3 0.4\n", encoding="utf-8")


def stage_intake(staged: Path, external: Path) -> None:
    diagnostics = staged / "1.7/Models/Diagnostics"
    for context in ("InternalPreferred", "InternalFallback"):
        root = diagnostics / context
        for name in INTERNAL_FILES:
            if context == "InternalPreferred":
                if name not in {"hair_probe.obj", "hair_version_only.obj"}:
                    write_model(root / "Models", name, 1, False)
                write_model(root / "1.7/Models", name, 2)
            elif name != "hair_version_only.obj":
                write_model(root / "Models", name, 3, False)
    write_model(diagnostics, "internal.obj", 2)
    write_model(diagnostics, "shared.obj", 2)
    write_model(diagnostics, "legacy.obj", 2, False)
    (external / "About").mkdir(parents=True, exist_ok=True)
    (external / "About/About.xml").write_text(
        "<ModMetaData><name>Normal proof external root</name><author>IsoRim tests</author>"
        "<packageId>isorim.normalproof.external</packageId><supportedVersions><li>1.7</li></supportedVersions>"
        "<modDependencies><li><packageId>connor.isorim</packageId><displayName>IsoRim</displayName></li></modDependencies></ModMetaData>")
    # A counterfactual About carrier proves the engine ignores that inactive location.
    (external / "About/LoadFolders.xml").write_text("<loadFolders><v1.7><li>IgnoredAboutFolder</li></v1.7></loadFolders>")
    write_model(external / "1.7/Models/Diagnostics", "shared.obj", 7)
    write_model(external / "1.7/Models/Diagnostics", "part.obj", 8)
    external_data = external / "1.7/Models/Diagnostics"
    (external / "sample.mtl").write_text("newmtl sample\nKd 0.9 0.8 0.7\n")
    (external_data / "missing-material.obj").write_text(mesh(2).replace("sample.mtl", "absent.mtl"))
    (external_data / "malformed.obj").write_text(mesh(2).replace("sample.mtl", "malformed.mtl"))
    (external_data / "malformed.mtl").write_text("newmtl sample\nKd broken 0.3 0.4\n")
    # Invalid normals belong to loader fault fixtures, outside the passing staged mod.
    for name, normal in (("normalize-below", "0.000002 0 0"), ("normalize-at", "0.00001 0 0"), ("normalize-above", "0.00002 0 0"),
                         ("normalize-next-below", "0.00000999999883788405 0 0"), ("normalize-next-above", "0.00001000000065687345 0 0"),
                         ("normalize-diagonal-below", "0.000006 0.000006 0"), ("normalize-diagonal-above", "0.000008 0.000008 0")):
        (external_data / (name + ".obj")).write_text(mesh(2).replace("vn 0 0.6 -0.8", "vn " + normal))
    for root, prefix in ((staged, "Internal"), (external, "External")):
        defs = root / "1.7/Defs/Diagnostics"
        defs.mkdir(parents=True, exist_ok=True)
        path = "1.7/Models/Diagnostics/" + ("internal.obj" if prefix == "Internal" else "shared.obj")
        parts = "<parts><li>1.7/Models/Diagnostics/part.obj</li></parts>" if prefix == "External" else ""
        (defs / "Models.xml").write_text(
            f'<Defs><ThingDef ParentName="ResourceBase"><defName>NormalProof{prefix}Thing</defName>'
            f'<label>normal proof {prefix.lower()}</label><description>Diagnostic item.</description>'
            '<statBases><Mass>1</Mass></statBases></ThingDef>'
            f'<IsoRim.ModelDef><defName>NormalProof{prefix}Model</defName><thingDef>NormalProof{prefix}Thing</thingDef>'
            f'<model>{path}</model>{parts}</IsoRim.ModelDef></Defs>')
        (defs / "WornGear.xml").write_text(
            f'<Defs><IsoRim.WornGearDef><defName>NormalProof{prefix}Gear</defName>'
            f'<apparel>NormalProof{prefix}Thing</apparel><model>{path}</model></IsoRim.WornGearDef></Defs>')

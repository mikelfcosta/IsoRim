"""Generated, disposable inputs for the real-game asset intake witnesses."""

from pathlib import Path
import hashlib
import json

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
    recovery = external_data / "Recovery"
    recovery.mkdir(exist_ok=True)
    cases = recovery_cases()
    for case in cases:
        directory = recovery / case["id"]
        directory.mkdir(exist_ok=True)
        (directory / "input.obj").write_bytes(bytes.fromhex(case["objHex"]))
        if case["mtlHex"] is not None:
            (directory / "sample.mtl").write_bytes(bytes.fromhex(case["mtlHex"]))
    (recovery / "ledger.json").write_text(json.dumps(cases, indent=2), encoding="utf-8")


def recovery_cases():
    """Authored R01–R29 inputs and literal expected outcomes; no validator imports."""
    result = []
    base = mesh(2).replace("vn 0 0.6 -0.8", "vn 1 0 0")
    material = "newmtl sample\nKd 0.2 0.3 0.4\n"

    def add(group, suffix, text, cli, loader, *, mtl=material, token=None, bits=None, field="vn.x", index=None):
        raw = text.encode("utf-8")
        mraw = None if mtl is None else mtl.encode("utf-8")
        result.append({"id": group + "-" + suffix, "class": group, "cli": cli, "loader": loader,
                       "strictProfile": cli == 1 and loader == "A", "field": field,
                       "objHex": raw.hex(), "objHash": hashlib.sha256(raw).hexdigest().upper(),
                       "mtlHex": None if mraw is None else mraw.hex(),
                       "mtlHash": None if mraw is None else hashlib.sha256(mraw).hexdigest().upper(),
                       "token": token, "bits": bits, "indexToken": index})

    def normal(group, suffix, value, cli, loader, **kwargs):
        add(group, suffix, base.replace("vn 1 0 0", "vn " + value), cli, loader, **kwargs)

    normal("R01", "unit", "1 0 0", 0, "A")
    normal("R01", "original", "0 0.6 -0.8", 0, "A")
    normal("R02", "positive", "+1 -0 +0", 0, "A", token="-0", bits="80000000", field="vn.y")
    normal("R02", "negative", "-1 +0 0", 0, "A", token="-1", bits="BF800000")
    for suffix, value in (("leading-dot", ".5"), ("trailing-dot", "1."), ("leading-zeros", "0001.0")):
        normal("R03", suffix, value + " 0 0", 0, "A", token=value)
    for suffix, value in (("lower", "1e0"), ("signed", "1E+0"), ("negative", "10e-1")):
        normal("R04", suffix, value + " 0 0", 0, "A", token=value, bits="3F800000")
    for suffix, value in (("empty", ""), ("sign", "+"), ("dot", "."), ("exponent", "1e"), ("exponent-sign", "1e+"), ("signs", "--1"), ("dots", "1.2.3")):
        normal("R05", suffix, value + " 0 0", 1, "F", token=value)
    for suffix, value in (("integer", "1_0"), ("fraction", "1.0_0"), ("exponent", "1e0_0")):
        normal("R06", suffix, value + " 0 0", 1, "F", token=value)
    for suffix, value in (("arabic", "١"), ("fullwidth", "１"), ("arabic-exp", "1e٠"), ("fullwidth-exp", "1e０"), ("minus", "−1")):
        normal("R07", suffix, value + " 0 0", 1, "F", token=value)
    for suffix, value in (("comma", "1,0"), ("group", "1,000"), ("currency", "$1"), ("parens", "(1)"), ("trailing-sign", "1-"), ("hex", "0x1")):
        normal("R08", suffix, value + " 0 0", 1, "F", token=value)
    for suffix, value in (("nan", "NaN"), ("nan-lower", "nan"), ("nan-sign", "+NaN"), ("inf", "Infinity"), ("inf-lower", "infinity"), ("inf-negative", "-Infinity"), ("overflow", "1e39")):
        normal("R09", suffix, value + " 0 0", 1, "F", token=value)
    for suffix, value in (("zero", "0 0 0"), ("signed-zero", "-0 +0 -0"), ("underflow", "1e-50 0 0")):
        normal("R10", suffix, value, 1, "F")
    normal("R11", "magnitude", "1e20 0 0", 1, "F", token="1e20")
    for suffix, value, cli, loader in (("below", "0.000002 0 0", 1, "F"), ("at", "0.00001 0 0", 1, "F"),
                                     ("above", "0.00002 0 0", 0, "A"), ("next-below", "0.00000999999883788405 0 0", 1, "F"),
                                     ("next-above", "0.00001000000065687345 0 0", 0, "A"),
                                     ("diagonal-below", "0.000006 0.000006 0", 1, "F"), ("diagonal-above", "0.000008 0.000008 0", 0, "A")):
        normal("R12", suffix, value, cli, loader)
    for suffix, value, bits in (("below", "1.000000059604644775390624999999", "3F800000"),
                                ("tie", "1.000000059604644775390625", "3F800000"),
                                ("above", "1.000000059604644775390625000001", "3F800001")):
        normal("R13", suffix, value + " 0 0", 0, "A", token=value, bits=bits)
    boundaries = (("max", "3.40282346638528859811704183484516925440e38", "7F7FFFFF", 0),
                  ("overflow", "3.4028236e38", "7F800000", 1),
                  ("subnormal", "1.401298464324817070923729583289916131280e-45", "00000001", 0),
                  ("underflow", "1e-50", "00000000", 0),
                  ("min-normal", "1.17549435082228750796873653722224568e-38", "00800000", 0))
    for label, value, bits, overflow in boundaries:
        for sign, prefix, expected in (("positive", "", bits), ("negative", "-", f"{int(bits, 16) | 0x80000000:08X}")):
            token = prefix + value
            normal("R14", label + "-" + sign + "-vn", token + " 0 0", 1, "F", token=token, bits=expected)
            for role in ("v", "vt", "Kd"):
                text = base + "v " + token + " 0 0\n" if role == "v" else base.replace("vt 0 0", "vt " + token + " 0")
                mtl = material if role != "Kd" else material.replace("0.2 0.3 0.4", token + " 0.3 0.4")
                if role == "Kd": text = base
                add("R14", label + "-" + sign + "-" + role, text, overflow, "A", mtl=mtl, token=token, bits=expected, field=role + ".x")
    rounding_boundaries = (
        ('normal-mid-below', '1.175494280757364291727882991035766513322858992758990427682963118425003064965173038558532425668090581893920898437499999999999999999999999999999999999999999999999999E-38', '007FFFFF'), ('normal-mid-tie', '1.1754942807573642917278829910357665133228589927589904276829631184250030649651730385585324256680905818939208984375E-38', '00800000'), ('normal-mid-above', '1.175494280757364291727882991035766513322858992758990427682963118425003064965173038558532425668090581893920898437500000000000000000000000000000000000000000000000001E-38', '00800000'), ('sub-mid-below', '7.0064923216240853546186479164495806564013097093825788587853414194489554134293030074331909418106079101562499999999999999999999999999999999999999999999999999E-46', '00000000'), ('sub-mid-tie', '7.00649232162408535461864791644958065640130970938257885878534141944895541342930300743319094181060791015625E-46', '00000000'), ('sub-mid-above', '7.0064923216240853546186479164495806564013097093825788587853414194489554134293030074331909418106079101562500000000000000000000000000000000000000000000000001E-46', '00000001'), ('overflow-mid-below', '340282356779733661637539395458142568447', '7F7FFFFF'), ('overflow-mid-tie', '340282356779733661637539395458142568448', '7F800000'), ('overflow-mid-above', '340282356779733661637539395458142568449', '7F800000'))
    for label, token, bits in rounding_boundaries:
        normal("R14", label, token + " 0 0", 1, "F", token=token, bits=bits)
        normal("R14", label + "-negative", "-" + token + " 0 0", 1, "F", token="-" + token, bits=f"{int(bits, 16) | 0x80000000:08X}")
    for suffix, token, bits in (("huge-exponent-zero", "0e999999999999999999999999999999", "00000000"),
                                ("huge-exponent-negative-zero", "-0e-999999999999999999999999999999", "80000000"),
                                ("huge-exponent-underflow", "1e-999999999999999999999999999999", "00000000"),
                                ("huge-exponent-overflow", "1e999999999999999999999999999999", "7F800000")):
        normal("R14", suffix, token + " 0 0", 1, "F", token=token, bits=bits)
    normal("R14", "long-leading-zeros", "0" * 5000 + "1 0 0", 0, "A", token="0" * 5000 + "1", bits="3F800000")
    normal("R15", "short", "1 0", 1, "F")
    add("R15", "later-valid", base.replace("vn 1 0 0", "vn broken 0 0\nvn 1 0 0").replace("/1 ", "/2 ").replace("/1\n", "/2\n"), 1, "A", field="vn.ordinal / face.normal")
    add("R15", "invalid-first", base.replace("vn 1 0 0", "vn broken 0 0\nvn 1 0 0"), 1, "F", field="vn.ordinal / face.normal")
    for suffix, old, new in (("vn-numeric", "vn 1 0 0", "vn 1 0 0 4"), ("vn-junk", "vn 1 0 0", "vn 1 0 0 junk"),
                              ("v-surplus", "v 0 0 0", "v 0 0 0 1 1 1 9"), ("vt-junk", "vt 0 0", "vt 0 0 junk")):
        add("R16", suffix, base.replace(old, new), 1, "A", field=old.split(" ", 1)[0] + ".payload")
    add("R16", "kd-surplus", base, 1, "A", mtl=material.replace("Kd 0.2 0.3 0.4", "Kd 0.2 0.3 0.4 junk"), field="Kd.payload")
    add("R16", "kd-numeric", base, 1, "A", mtl=material.replace("Kd 0.2 0.3 0.4", "Kd 0.2 0.3 0.4 .5"), field="Kd.payload")
    add("R16", "vt-surplus", base.replace("vt 0 0", "vt 0 0 .5 .25"), 1, "A", field="vt.payload")
    add("R16", "vt-third", base.replace("vt 0 0", "vt 0 0 +1e0"), 0, "A", field="vt.third")
    for suffix, text in (("spaces", base.replace(" ", "  \t ")), ("crlf", base.replace("\n", "\r\n")), ("cr", base.replace("\n", "\r")),
                         ("bom", "\ufeff" + base), ("comments", "\n# full comment\n\t\n" + base)):
        add("R17", suffix, text, 0, "A", field="physical records")
    for suffix, line, loader in (("nbsp-record", "vn\u00a01 0 0", "F"), ("nbsp-token", "vn 1\u00a0 0 0", "F"),
                                  ("nbsp-outer", "\u00a0vn 1 0 0\u00a0", "A"), ("nul-trailing", "vn 1 0 0\x00", "A"),
                                  ("nul-embedded", "vn 1 0\x00x 0", "F")):
        add("R18", suffix, base.replace("vn 1 0 0", line), 1, loader, field="vn.physical record")
    for suffix, char in (("vt", "\v"), ("ff", "\f"), ("nel", "\u0085"), ("ls", "\u2028"), ("ps", "\u2029")):
        normal("R18", suffix, "1" + char + "x 0 0", 1, "F", field="vn.physical record")
    for suffix, old, new, loader in (("attached", "vn 1 0 0", "vn 1#x 0 0", "F"), ("vn-inline", "vn 1 0 0", "vn 1 0 0 # note", "A"),
                                     ("face-inline", "f 1/1/1 2/2/1 3/3/1", "f 1/1/1 2/2/1 3/3/1 # note", "N"),
                                     ("mtl-inline", "mtllib sample.mtl", "mtllib sample.mtl # note", "A")):
        add("R19", suffix, base.replace(old, new), 1, loader, field=old.split(" ", 1)[0] + ".payload")
    for suffix, text in (("bare", base.replace("vn 1 0 0\n", "").replace("f 1/1/1 2/2/1 3/3/1", "f 1 2 3")),
                         ("uv", base.replace("vn 1 0 0\n", "").replace("/1 ", " ").replace("/1\n", "\n")),
                         ("mixed", base.replace("2/2/1", "2/2/"))):
        add("R20", suffix, text, 0, "F", field="face.normal slots")
    for suffix, face in (("full", "1/1/1 2/2/1 3/3/1"), ("no-uv", "1//1 2//1 3//1"),
                         ("negative", "-3/-3/-1 -2/-2/-1 -1/-1/-1"), ("leading", "+001/+001/+01 +002/+002/+01 +003/+003/+01")):
        add("R21", suffix, base.replace("1/1/1 2/2/1 3/3/1", face), 0, "A", field="face.indices")
    for group, tokens in (("R22", (("underscore", "1_0"), ("unicode", "١"), ("exponent", "1e0"), ("decimal", "1.0"), ("empty", ""), ("zero", "0"), ("positive-zero", "+0"), ("negative-zero", "-0"))),
                          ("R23", (("min", "-2147483648"), ("max", "2147483647"), ("below-min", "-2147483649"), ("above-max", "2147483648"), ("forward", "4"), ("too-negative", "-4")))):
        for suffix, token in tokens:
            for role, corner, loader in (("position", token + "/1/1", "N"), ("texture", "1/" + token + "/1", "A"), ("normal", "1/1/" + token, "F")):
                # An empty optional slot is legal and differs from an explicit invalid index.
                cli = 0 if token == "" and role != "position" else 1
                text = base.replace("f 1/1/1", "f " + corner)
                if role == "texture":
                    text = text.replace("vt 0 0", "vt .25 .75")
                add(group, suffix + "-" + role, text, cli, loader, field=role, index=token)
    add("R23", "leading-zeros-position", base.replace("f 1/1/1", "f " + "0" * 5000 + "1/1/1"), 0, "A", index="0" * 5000 + "1", field="position")
    add("R23", "long-overflow-position", base.replace("f 1/1/1", "f 10000000000/1/1"), 1, "N", index="10000000000", field="position")
    add("R24", "slash-surplus", base.replace("1/1/1", "1/1/1/junk"), 1, "A", field="face.corner layout")
    add("R24", "short-face", base.replace("f 1/1/1 2/2/1 3/3/1", "f 1/1/1 2/2/1"), 1, "N", field="face.corners")
    for role in ("v", "vt", "rgb"):
        for suffix, token, cli in (("underscore", "1_0", 1), ("unicode", "１", 1), ("overflow", "1e39", 1), ("zero", "0", 0), ("underflow", "1e-50", 0), ("finite", ".25", 0)):
            text = base.replace("v 0 0 0", "v " + token + " 0 0") if role == "v" else base.replace("vt 0 0", "vt " + token + " 0")
            if role == "rgb": text = base.replace("v 0 0 0", "v 0 0 0 " + token + " 0 0")
            add("R25", role + "-" + suffix, text, cli, "A", token=token, field=role + ".x")
    for suffix, token, cli in (("underscore", "1_0", 1), ("unicode", "１", 1), ("overflow", "1e39", 1), ("zero", "0", 0), ("underflow", "1e-50", 0), ("finite", ".25", 0), ("short", "", 1)):
        add("R26", suffix, base, cli, "A", mtl=material.replace("Kd 0.2 0.3 0.4", "Kd " + token + " 0 0"), token=token, field="Kd.x")
    add("R27", "missing-mtl", base, 1, "A", mtl=None, field="mtllib.path")
    add("R27", "escaping-mtl", base.replace("sample.mtl", "../../../../../../outside.mtl"), 1, "A", mtl=None, field="mtllib.path")
    add("R27", "undefined", base.replace("usemtl sample", "usemtl absent"), 1, "A", field="usemtl.name")
    add("R28", "empty", "", 1, "N", field="file.triangles")
    add("R28", "vertices", "v 0 0 0\nv 2 0 0\nv 0 1 0\n", 1, "N", field="file.triangles")
    add("R29", "collinear", base.replace("v 0 1 0", "v 4 0 0"), 1, "N", field="file.triangles")
    add("R29", "valid", base, 0, "A", field="file.triangles")
    return result


def recovery_mod(root, assembly):
    """Minimal otherwise-valid referenced mod for full-CLI mutation controls."""
    import shutil

    for relative in ("About", "Defs", "Models", "1.7/Assemblies", "1.7/Models/Diagnostics", "1.7/Defs/Diagnostics"):
        (root / relative).mkdir(parents=True, exist_ok=True)
    (root / "About/About.xml").write_text("<ModMetaData><supportedVersions><li>1.7</li></supportedVersions></ModMetaData>")
    (root / "About/LoadFolders.xml").write_text("<loadFolders/>")
    (root / "Defs/Models.xml").write_text("<Defs/>")
    (root / "Defs/KeyBindings.xml").write_text("<Defs/>")
    (root / "Models/isorim.mtl").write_text("newmtl sample\n")
    (root / "1.7/Defs/Diagnostics/Models.xml").write_text(
        '<Defs><ThingDef ParentName="ResourceBase"><defName>RecoveryThing</defName><label>recovery fixture</label>'
        '<description>Disposable validation input.</description><statBases><Mass>1</Mass></statBases></ThingDef>'
        '<IsoRim.ModelDef><defName>RecoveryModel</defName><thingDef>RecoveryThing</thingDef>'
        '<model>1.7/Models/Diagnostics/internal.obj</model></IsoRim.ModelDef></Defs>')
    (root / "1.7/Defs/Diagnostics/WornGear.xml").write_text(
        '<Defs><IsoRim.WornGearDef><defName>RecoveryGear</defName><apparel>RecoveryThing</apparel>'
        '<model>1.7/Models/Diagnostics/internal.obj</model></IsoRim.WornGearDef></Defs>')
    shutil.copy2(assembly, root / "1.7/Assemblies/IsoRim.dll")

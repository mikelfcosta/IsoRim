# Prepared models for RimWorld 1.7

IsoRim accepts small OBJ models with adjacent MTL files. This note defines the prepared profile for later material work.
The profile does not require studio studies, production exports or a rebuild of existing models.
The production loader still accepts legacy files that the strict prepared validator rejects.

## Stable contract

The diagnostic fixture and downstream preparation use these values. `tools/prepared_models/contract.json` carries the same machine-readable contract.

| Field | Value |
| --- | --- |
| profile | IsoRim prepared OBJ 1.7 v1 |
| units | one cell per unit |
| axes | Y up; importer reflects position and normal Z |
| ground | y=0 |
| pivot | explicit model or definition pivot; no implicit recenter |
| normalSpace | OBJ model-local before import; true-space shading after import |
| triangleValidity | all three corners usable or whole-triangle geometric fallback |
| winding | closed negative-volume repair; authored N faces geometric G hemisphere |
| transform | inverse transpose before artificial Y squash; merged local then figure |
| channels | Normal=shading; UV0=texture; UV1=cutaway; binding4=instance matrices |
| geometric | positions; face normals; hull; texture lift; damage; cutaway; glow ownership |
| paths | internal 1.7/Models preferred; definition paths relative to defining mod root |
| materials | adjacent mtllib; case-insensitive names; stuff/leaf/twig/glow/flame/ember prefixes |
| numeric | ASCII decimal to exact binary32 ties-to-even; signed Int32 indices |
| fallback | absent vn is legal; explicit invalid vn is a prepared error |
| exports | generated asymmetric fixture; no production export required |

## Geometry, normals and transforms

Use cells as units, with Y up and ground at `y=0`. Document the pivot in each definition or model preparation record.
The loader preserves positions. Definition translation, rotation, scale and pivot establish placement.
The loader reflects OBJ Z for positions and `vn` directions. Do not apply a second reflection during export preparation.

`Model.normals` contains geometric face directions. `Model.shadingNormals` and `Model.authoredNormals` carry separate shading data and corner validity.
An authored triangle needs usable normal references on all three corners. Missing or invalid references select geometric fallback for the entire triangle.
An independent valid triangle keeps its authored data. Missing `vn` remains legal for legacy appearance.

The importer preserves normal declaration ordinals, including invalid declarations. Never compact the `vn` list before resolving face indices.
The importer repairs a closed mesh with clearly negative volume. It aligns each authored normal with its final geometric hemisphere.
Mirroring uses the same geometric orientation contract. Do not invert authored normals independently to compensate for reflection.

Static baking applies the physical transform's inverse transpose to shading normals. The uploaded stream then contains true-space directions.
Moving meshes retain model-local shading normals. The shader applies the inverse transpose of the final figure matrix.
Merged parts first apply each part's local inverse transpose. The figure transform follows at draw time.
The artificial height expansion and camera Y squash cancel for geometry. They must not enter physical shading-normal transforms.
Supported squash values are `0.002` through `0.004`. Camera pitch is `30` through `85` degrees.

Positions, geometric normals, hulls, outline extrusion, texture lift, damage and cutaway keep their geometric duties.
Glow marks use geometric cell ownership. They do not use authored normals to choose a lighting cell.
The normal stream does not replace UV0, UV1 cutaway tags or binding 4 instance matrices.
The existing normal-space material states are geometric-only, baked true-space and model-local. They are internal conventions, not a public material API.

## Paths, UVs and materials

Internal logical model readers prefer `1.7/Models/<file>` when that exact file exists. They otherwise use the legacy `Models/<file>`.
Model and gear definitions resolve explicit file paths from their defining mod's root. Part paths use that same root.
Use an explicit `1.7/Models/...` path in a new versioned definition. Do not prefix another mod's path with IsoRim's root.
The accepted OBJ resolves `mtllib` beside that OBJ. Keep the MTL inside the prepared model directory.

Use `vt u v` for UV0. The loader accepts a numeric third component but ignores it.
Do not use OBJ fields to supply UV1 cutaway tags. IsoRim writes those tags during baking.
Keep every referenced material declared in the adjacent MTL. Material names compare without case.
`Kd r g b` supplies colour unless vertex RGB overrides it. Optional vertex RGB follows XYZ on the `v` record.
`map_Kd` names a content texture. The loader removes the `Textures/` prefix and file extension from exported texture paths.
Use a packaged texture content path. The proof uses its own opaque white texel, so Core sprite discovery cannot determine its pixel results.

The `stuff` prefix selects stuff tint. The `leaf` and `twig` prefixes retain cutaway meanings.
`glow`, `flame` and `ember` select unlit regions. Flame and ember retain their existing fire meaning.
Material names and alpha are not a new general material framework. Later matte/clay styling must preserve these ownership rules.

## Strict prepared syntax

Write UTF-8, with an optional BOM at file start. Use physical LF, CRLF or CR lines.
Separate record tokens with ASCII space or tab. Blank lines and full-line `#` comments are legal.
Do not append inline comments to consumed records. Do not include Unicode separators, embedded controls or unsupported trailing payload.
Ordinary ignored OBJ metadata remains legal. The validator does not invent support for additional OBJ features.

The scalar grammar is `[+-]?(?:[0-9]+(?:\.[0-9]*)?|\.[0-9]+)(?:[eE][+-]?[0-9]+)?`.
Signs, leading zeros, leading or trailing decimal points and signed exponents are legal.
Underscores, Unicode digits or minus signs, group separators, currency, hexadecimal forms and malformed exponents are errors.
Each nonempty face index uses `[+-]?[0-9]+` and the signed Int32 range, `-2147483648` through `2147483647`.
Index zero is invalid. Positive and negative relative references must resolve against the declarations already read.

Use `v x y z`, optionally followed by exactly RGB. Use `vt u v`, optionally followed by one numeric third component.
Use exactly three components for `vn` and `Kd`. Face corners have at most three slash fields.
Positions are required. Empty optional texture or normal slots remain legal, including `v//vn` and absent normals.
Explicit invalid optional indices are prepared errors. The legacy loader can instead default UVs or use whole-triangle normal fallback.
Empty, vertices-only and collinear-only files are errors because they emit no triangle. A valid neighbouring triangle remains legal.

The validator converts decimal scalars directly to binary32 with round-to-nearest, ties-to-even.
It does not use a binary64 intermediate as the rounding oracle. Signed zero and component underflow remain legal for finite coordinates, UVs and colours.
Consumed nonfinite coordinates and colours are errors. An explicit unusable `vn` is an error even when no face references it.
Normal validity uses rounded binary32 products and a left-associated squared sum. The squared length must be finite and at least `1e-12f`.
The rounded square root must pass the engine's strict `>1e-5f` normalization branch. Otherwise the loader produces zero and selects fallback.
Large finite components can overflow the squared sum. Tiny nonzero components can normalize to zero. Neither is a usable prepared normal.

The R01–R29 recovery matrix distinguishes strict-profile rejection from actual legacy-loader tolerance.
It covers token spelling, record separation, declaration ordinals, exact rounding, index roles, materials and emitted geometry.
Its 230 subcases retain independent literal CLI and actual-game outcomes. The 23 earlier staged controls remain required.
Runtime observations include model corners, geometric and shading streams, material diagnostics and supplementary bundled-CLR parse bits.
The CLI and actual loader must both be observed. A host-only success cannot establish the loader contract.

## Repeatable proof and distribution

The gate generates an asymmetric triangulated OBJ, adjacent MTL and missing/invalid controls. It uses the same geometry across static, moving, instanced and merged paths.
Standing, unequal-scale, laid-flat and mirrored transforms have independent expected vectors and framebuffer references.
Ordinary-lighting proof disables normal diagnostics. It rejects geometric-only and quantized-authored lighting faults while transport diagnostics remain correct.
It then resets each fault and requires another positive capture. Each receipt binds fresh raw pixels, PNGs, process identity and module hashes.

The validated distribution module is `1.7/Assemblies/IsoRim.dll`. The exact-reference build, staged module, committed module and actual loaded module must have identical SHA-256 hashes.
Local compilation uses approved installed RimWorld 1.7.4980 rev60 references and the explicit SDK 10.0.402.
Acceptance runs under the game's bundled `10.0.10-waylithclr.dev` runtime. Portable 4979-alpha package compilation is compatibility-only.
The final whole T1 runs at the frozen integrated epic head. Hosted CI is a separate compatibility check.
The proof does not certify production asset exports, Core sprite discovery, material art or performance capacity.

# Asset status for IsoRim 1.7

This inventory records the selected Clay deliveries and current authored-model routes. Frozen root, 1.5 and 1.6 assets retain their original files.

## Scope and counting units

The authored counting unit is one active OBJ path, including each separate body, leg, fit, state or assembly part. Shared aliases share one row.
The no-authored counting unit is one concrete installed XML ThingDef without a bound authored model. These totals use different units.
Coverage includes Core, Royalty, Ideology, Biotech, Anomaly and Odyssey. The census resolves XML inheritance and checks product mappings and hardcoded parts.
This is a static census. Game patch operations, implied runtime-generated definitions and live DefDatabase selection were not executed.
Models remain subject to the existing graphics settings. Items can prefer sprite extrusion when itemsFromArt is enabled.
Food stacks follow the existing raw-model/art setting. Stone blocks use overArt and still require rawModels. Nothing in this report calls extrusion authored geometry.

Active Clay: **1238** shared model identities/parts. Active original: **158** shared model identities/parts.
No authored 3D yet: **538** concrete XML identities. See procedural exceptions below.

## Archive sources

Z codes below name exact ZIP files from the pinned commission inventory. All archives passed path-safety checks before private extraction.
The SHA-256 column identifies preserved input bytes. No included script, macro or installer ran.

| Code | ZIP | SHA-256 |
|---|---|---|
| Z01 | IsoRim_30_Asset_Clay_Trial.zip | `c8c6501532efa9c9fd6a164f7dac7f7efa397278d102e997fd3083ff31fcd4d9` |
| Z02 | IsoRim_Batch_04_Final_Support_Models.zip | `34525b555635992eb4ef0b310d6bbe861e2f9ee74d0b6ef7087961b725ba3ea4` |
| Z03 | IsoRim_Batch_05_Anomaly_Models_First.zip | `0d763df1416264b707da2a8dea3b6166296f583ce84c47a6a30b676fa0c99ff9` |
| Z04 | IsoRim_Batch_05_Biotech_Models_First.zip | `c0572426e9e6149bdf64c4ef3fee133c6cc5faa79a1961648e18dbaec2cd5772` |
| Z05 | IsoRim_Batch_05_Odyssey_Models_First.zip | `93b80e8a27e3e6448f4ee804d47094b26ee80d97624710bff823c24f76addbad` |
| Z06 | IsoRim_Clay_30_Trial.zip | `c8c6501532efa9c9fd6a164f7dac7f7efa397278d102e997fd3083ff31fcd4d9` |
| Z07 | IsoRim_Clay_Batch200_Animals_121-160.zip | `b0b00619aa798529b108756380a7d51c3f5ed737b74a191a85b40ff792102eca` |
| Z08 | IsoRim_Clay_Batch200_Animals_161-200.zip | `e7880cf161267dd1a9a635e8d121048e9be6a07f2a5a7cf231878038b8ffe03d` |
| Z09 | IsoRim_Clay_Batch200_Hairstyles_091-120.zip | `b0706e2a10368b5a4cc3645485e29e6c91ffe8def4b5b5512115c66c9fd1b45a` |
| Z10 | IsoRim_Clay_Batch200_Manifest_and_Handoff.zip | `b2af986e16a3cb18a1de702c7674b5df7cc7cdb00471901d18c69eb23cc76543` |
| Z11 | IsoRim_Clay_Batch200_Objects_001-030.zip | `6bfac93034cfd2bb219582e62e51c70e8de14d78d60a336db7256b28e2e41f41` |
| Z12 | IsoRim_Clay_Batch200_Resources_and_Plants_031-090.zip | `57d99ebc722169e06bb31bac0f92839ddf0d4f3d99bd9816cb54ae5e8d80bf8e` |
| Z13 | IsoRim_Clay_Batch_02_Models.zip | `bd46b8aeae4d66b889508029c4ecf8f88e5e6e02576e59298d1539f6177646e7` |
| Z14 | IsoRim_Clay_Batch_03_Models.zip | `63c299bad8d5dcfee59ae9920e56a2eb374334f2221bd223e1ead31ae67d88b2` |
| Z15 | IsoRim_Economical_60_Assets.zip | `31993c09d6751279d0192edaf708bfb62e62a75acde1f82e9074061d9269dfc4` |
| Z16 | IsoRim_Efficient_90_Assets.zip | `22f968f831859dd36e2704a8549f9cb897a4e9aee720c7826fab692170a28fd7` |
| Z17 | IsoRim_Five_Asset_Clay_Trial.zip | `1db88872a8a593f9499fba6341ad7e72b0260405ac4b29fdf32f9300791cd024` |
| Z18 | IsoRim_Royalty_DLC_Models_Only.zip | `d0e48ec4699c8811955be816137383252e9d4f0ef7a181f6d70205e5e8628a97` |

## Clay 3D

Every row below has an active 1.7 definition path or an existing InternalPath/People route. ZIP members identify selected geometry before dependency-path rewrites.
The import preserves vertex, UV, normal, face, material-region and assembly data. Only mtllib references and texture content keys change.
The prepared-model contract loads authored normals in 1.7. Older archive warnings about ignored normals describe the pre-importer-update source.
The handoffs report incomplete studio review for Biotech, Anomaly and Odyssey models-first deliveries. This import does not claim a live visual result.

| Identity / part and shared aliases | Active path | ZIP / member | Geometry provenance |
|---|---|---|---|
| Biotech:Mech_Agrihand; Mech_Agrihand:model | `1.7/Models/agrihand.obj` | Z04 / `Models/agrihand.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Agrihand; Mech_Agrihand:model | `1.7/Models/agrihand_cutter.obj` | Z04 / `Models/agrihand_cutter.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Agrihand; Mech_Agrihand:model | `1.7/Models/agrihand_track_left.obj` | Z04 / `Models/agrihand_track_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Agrihand; Mech_Agrihand:model | `1.7/Models/agrihand_track_right.obj` | Z04 / `Models/agrihand_track_right.obj` | Clay delivery with retained geometry |
| Core:Plant_Alocasia; Plant_Alocasia:model | `1.7/Models/alocasia.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/alocasia.obj` | Clay delivery with retained geometry |
| AncientAirConditioner:model; Core:AncientAirConditioner | `1.7/Models/ancient_ac.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ancient_ac.obj` | Clay delivery, selected runtime geometry |
| AncientATM:model; Core:AncientATM | `1.7/Models/ancient_atm.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ancient_atm.obj` | Clay delivery, selected runtime geometry |
| AncientCryptosleepPod:model; Core:AncientCryptosleepPod | `1.7/Models/ancient_cryptosleep_pod.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ancient_cryptosleep_pod.obj` | Clay delivery, selected runtime geometry |
| AncientFence:li; Core:AncientFence | `1.7/Models/ancient_fence_arm.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ancient_fence_arm.obj` | Clay delivery with retained geometry |
| AncientFence:li; Core:AncientFence | `1.7/Models/ancient_fence_post.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ancient_fence_post.obj` | Clay delivery, selected runtime geometry |
| AncientHydrant:model; Core:AncientHydrant | `1.7/Models/ancient_hydrant.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ancient_hydrant.obj` | Clay delivery with retained geometry |
| Alpaca:model; Core:Alpaca | `1.7/Models/animal_alpaca.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_alpaca.obj` | Clay delivery, selected runtime geometry |
| Alpaca:legModel; Core:Alpaca | `1.7/Models/animal_alpaca_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_alpaca_leg.obj` | Clay delivery with retained geometry |
| Bear_Grizzly:model; Core:Bear_Grizzly | `1.7/Models/animal_bear_grizzly.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_bear_grizzly.obj` | Clay delivery, selected runtime geometry |
| Bear_Grizzly:legModel; Core:Bear_Grizzly | `1.7/Models/animal_bear_grizzly_leg.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_bear_grizzly_leg.obj` | Clay delivery, selected runtime geometry |
| Bear_Polar:model; Core:Bear_Polar | `1.7/Models/animal_bear_polar.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_bear_polar.obj` | Clay delivery, selected runtime geometry |
| Bear_Polar:legModel; Core:Bear_Polar | `1.7/Models/animal_bear_polar_leg.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_bear_polar_leg.obj` | Clay delivery, selected runtime geometry |
| AnimalBed:model; Core:AnimalBed | `1.7/Models/animal_bed.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/animal_bed.obj` | Clay delivery, selected runtime geometry |
| Bison:model; Core:Bison | `1.7/Models/animal_bison.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_bison.obj` | Clay delivery, selected runtime geometry |
| Bison:legModel; Core:Bison | `1.7/Models/animal_bison_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_bison_leg.obj` | Clay delivery with retained geometry |
| Boomalope:model; Core:Boomalope | `1.7/Models/animal_boomalope.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_boomalope.obj` | Clay delivery, selected runtime geometry |
| Boomalope:legModel; Core:Boomalope | `1.7/Models/animal_boomalope_leg.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_boomalope_leg.obj` | Clay delivery, selected runtime geometry |
| Boomrat:model; Core:Boomrat | `1.7/Models/animal_boomrat.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_boomrat.obj` | Clay delivery, selected runtime geometry |
| Boomrat:legModel; Core:Boomrat | `1.7/Models/animal_boomrat_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_boomrat_leg.obj` | Clay delivery, selected runtime geometry |
| Capybara:model; Core:Capybara | `1.7/Models/animal_capybara.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_capybara.obj` | Clay delivery, selected runtime geometry |
| Capybara:legModel; Core:Capybara | `1.7/Models/animal_capybara_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_capybara_leg.obj` | Clay delivery, selected runtime geometry |
| Caribou:model; Core:Caribou | `1.7/Models/animal_caribou.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_caribou.obj` | Clay delivery, selected runtime geometry |
| Caribou:legModel; Core:Caribou | `1.7/Models/animal_caribou_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_caribou_leg.obj` | Clay delivery, selected runtime geometry |
| Cassowary:model; Core:Cassowary | `1.7/Models/animal_cassowary.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/animal_cassowary.obj` | Clay delivery, selected runtime geometry |
| Cassowary:legModel; Core:Cassowary | `1.7/Models/animal_cassowary_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_cassowary_leg.obj` | Clay delivery with retained geometry |
| Cat:model; Core:Cat | `1.7/Models/animal_cat.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_cat.obj` | Clay delivery, selected runtime geometry |
| Cat:legModel; Core:Cat | `1.7/Models/animal_cat_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_cat_leg.obj` | Clay delivery with retained geometry |
| Chicken:model; Core:Chicken | `1.7/Models/animal_chicken.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/animals/animal_chicken.obj` | Clay delivery, selected runtime geometry |
| Chicken:legModel; Core:Chicken | `1.7/Models/animal_chicken_leg.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/animals/animal_chicken_leg.obj` | Clay delivery, selected runtime geometry |
| Chinchilla:model; Core:Chinchilla | `1.7/Models/animal_chinchilla.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_chinchilla.obj` | Clay delivery, selected runtime geometry |
| Chinchilla:legModel; Core:Chinchilla | `1.7/Models/animal_chinchilla_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_chinchilla_leg.obj` | Clay delivery, selected runtime geometry |
| Cobra:model; Core:Cobra | `1.7/Models/animal_cobra.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_cobra.obj` | Clay delivery, selected runtime geometry |
| Core:Cougar; Cougar:model | `1.7/Models/animal_cougar.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_cougar.obj` | Clay delivery, selected runtime geometry |
| Core:Cougar; Cougar:legModel | `1.7/Models/animal_cougar_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_cougar_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Cow; Cow:model | `1.7/Models/animal_cow.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/animals/animal_cow.obj` | Clay delivery, selected runtime geometry |
| Core:Cow; Cow:legModel | `1.7/Models/animal_cow_leg.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/animals/animal_cow_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Deer; Deer:model | `1.7/Models/animal_deer.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_deer.obj` | Clay delivery, selected runtime geometry |
| Core:Deer; Deer:legModel | `1.7/Models/animal_deer_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_deer_leg.obj` | Clay delivery with retained geometry |
| Core:Donkey; Donkey:model | `1.7/Models/animal_donkey.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_donkey.obj` | Clay delivery, selected runtime geometry |
| Core:Donkey; Donkey:legModel | `1.7/Models/animal_donkey_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_donkey_leg.obj` | Clay delivery with retained geometry |
| Core:Dromedary; Dromedary:model | `1.7/Models/animal_dromedary.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_dromedary.obj` | Clay delivery, selected runtime geometry |
| Core:Dromedary; Dromedary:legModel | `1.7/Models/animal_dromedary_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_dromedary_leg.obj` | Clay delivery with retained geometry |
| Core:Duck; Duck:model | `1.7/Models/animal_duck.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_duck.obj` | Clay delivery, selected runtime geometry |
| Core:Duck; Duck:legModel | `1.7/Models/animal_duck_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_duck_leg.obj` | Clay delivery with retained geometry |
| Core:Elephant; Elephant:model | `1.7/Models/animal_elephant.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_elephant.obj` | Clay delivery, selected runtime geometry |
| Core:Elephant; Elephant:legModel | `1.7/Models/animal_elephant_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_elephant_leg.obj` | Clay delivery with retained geometry |
| Core:Elk; Elk:model | `1.7/Models/animal_elk.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_elk.obj` | Clay delivery, selected runtime geometry |
| Core:Elk; Elk:legModel | `1.7/Models/animal_elk_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_elk_leg.obj` | Clay delivery with retained geometry |
| Core:Emu; Emu:model | `1.7/Models/animal_emu.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_emu.obj` | Clay delivery, selected runtime geometry |
| Core:Emu; Emu:legModel | `1.7/Models/animal_emu_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_emu_leg.obj` | Clay delivery with retained geometry |
| Core:Fox_Arctic; Fox_Arctic:model | `1.7/Models/animal_fox_arctic.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_fox_arctic.obj` | Clay delivery, selected runtime geometry |
| Core:Fox_Arctic; Fox_Arctic:legModel | `1.7/Models/animal_fox_arctic_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_fox_arctic_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Fox_Fennec; Fox_Fennec:model | `1.7/Models/animal_fox_fennec.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_fox_fennec.obj` | Clay delivery, selected runtime geometry |
| Core:Fox_Fennec; Fox_Fennec:legModel | `1.7/Models/animal_fox_fennec_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_fox_fennec_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Fox_Red; Fox_Red:model | `1.7/Models/animal_fox_red.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_fox_red.obj` | Clay delivery, selected runtime geometry |
| Core:Fox_Red; Fox_Red:legModel | `1.7/Models/animal_fox_red_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_fox_red_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Gazelle; Gazelle:model | `1.7/Models/animal_gazelle.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_gazelle.obj` | Clay delivery, selected runtime geometry |
| Core:Gazelle; Gazelle:legModel | `1.7/Models/animal_gazelle_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_gazelle_leg.obj` | Clay delivery with retained geometry |
| Core:Goat; Goat:model | `1.7/Models/animal_goat.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_goat.obj` | Clay delivery, selected runtime geometry |
| Core:Goat; Goat:legModel | `1.7/Models/animal_goat_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_goat_leg.obj` | Clay delivery with retained geometry |
| Core:Goose; Goose:model | `1.7/Models/animal_goose.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_goose.obj` | Clay delivery, selected runtime geometry |
| Core:Goose; Goose:legModel | `1.7/Models/animal_goose_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_goose_leg.obj` | Clay delivery with retained geometry |
| Core:GuineaPig; GuineaPig:model | `1.7/Models/animal_guineapig.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_guineapig.obj` | Clay delivery, selected runtime geometry |
| Core:GuineaPig; GuineaPig:legModel | `1.7/Models/animal_guineapig_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_guineapig_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Hare; Hare:model | `1.7/Models/animal_hare.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_hare.obj` | Clay delivery, selected runtime geometry |
| Core:Hare; Hare:legModel | `1.7/Models/animal_hare_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_hare_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Horse; Horse:model | `1.7/Models/animal_horse.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_horse.obj` | Clay delivery, prior approved export reused |
| Core:Horse; Horse:legModel | `1.7/Models/animal_horse_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_horse_leg.obj` | Clay delivery, prior approved export reused |
| Core:Husky; Husky:model | `1.7/Models/animal_husky.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_husky.obj` | Clay delivery, selected runtime geometry |
| Core:Husky; Husky:legModel | `1.7/Models/animal_husky_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_husky_leg.obj` | Clay delivery with retained geometry |
| Core:Ibex; Ibex:model | `1.7/Models/animal_ibex.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_ibex.obj` | Clay delivery, selected runtime geometry |
| Core:Ibex; Ibex:legModel | `1.7/Models/animal_ibex_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_ibex_leg.obj` | Clay delivery with retained geometry |
| Core:Iguana; Iguana:model | `1.7/Models/animal_iguana.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_iguana.obj` | Clay delivery, selected runtime geometry |
| Core:Iguana; Iguana:legModel | `1.7/Models/animal_iguana_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_iguana_leg.obj` | Clay delivery, selected runtime geometry |
| Core:LabradorRetriever; LabradorRetriever:model | `1.7/Models/animal_labrador.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_labrador.obj` | Clay delivery, selected runtime geometry |
| Core:LabradorRetriever; LabradorRetriever:legModel | `1.7/Models/animal_labrador_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_labrador_leg.obj` | Clay delivery with retained geometry |
| Core:Lynx; Lynx:model | `1.7/Models/animal_lynx.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_lynx.obj` | Clay delivery, selected runtime geometry |
| Core:Lynx; Lynx:legModel | `1.7/Models/animal_lynx_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_lynx_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Megascarab; Megascarab:model | `1.7/Models/animal_megascarab.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_megascarab.obj` | Clay delivery, selected runtime geometry |
| Core:Megascarab; Megascarab:legModel | `1.7/Models/animal_megascarab_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_megascarab_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Megasloth; Megasloth:model | `1.7/Models/animal_megasloth.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_megasloth.obj` | Clay delivery, selected runtime geometry |
| Core:Megasloth; Megasloth:legModel | `1.7/Models/animal_megasloth_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_megasloth_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Megaspider; Megaspider:model | `1.7/Models/animal_megaspider.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_megaspider.obj` | Clay delivery, selected runtime geometry |
| Core:Megaspider; Megaspider:legModel | `1.7/Models/animal_megaspider_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_megaspider_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Monkey; Monkey:model | `1.7/Models/animal_monkey.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_monkey.obj` | Clay delivery, selected runtime geometry |
| Core:Monkey; Monkey:legModel | `1.7/Models/animal_monkey_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_monkey_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Muffalo; Muffalo:model | `1.7/Models/animal_muffalo.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/animal_muffalo.obj` | Clay delivery, selected runtime geometry |
| Core:Muffalo; Muffalo:legModel | `1.7/Models/animal_muffalo_leg.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/animal_muffalo_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Ostrich; Ostrich:model | `1.7/Models/animal_ostrich.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_ostrich.obj` | Clay delivery, selected runtime geometry |
| Core:Ostrich; Ostrich:legModel | `1.7/Models/animal_ostrich_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_ostrich_leg.obj` | Clay delivery with retained geometry |
| Core:Panther; Panther:model | `1.7/Models/animal_panther.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_panther.obj` | Clay delivery, selected runtime geometry |
| Core:Panther; Panther:legModel | `1.7/Models/animal_panther_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_panther_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Pig; Pig:model | `1.7/Models/animal_pig.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_pig.obj` | Clay delivery, selected runtime geometry |
| Core:Pig; Pig:legModel | `1.7/Models/animal_pig_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_pig_leg.obj` | Clay delivery with retained geometry |
| Core:Raccoon; Raccoon:model | `1.7/Models/animal_raccoon.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_raccoon.obj` | Clay delivery, selected runtime geometry |
| Core:Raccoon; Raccoon:legModel | `1.7/Models/animal_raccoon_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_raccoon_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Rat; Rat:model | `1.7/Models/animal_rat.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_rat.obj` | Clay delivery, selected runtime geometry |
| Core:Rat; Rat:legModel | `1.7/Models/animal_rat_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_rat_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Rhinoceros; Rhinoceros:model | `1.7/Models/animal_rhinoceros.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_rhinoceros.obj` | Clay delivery, selected runtime geometry |
| Core:Rhinoceros; Rhinoceros:legModel | `1.7/Models/animal_rhinoceros_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_rhinoceros_leg.obj` | Clay delivery with retained geometry |
| Core:Sheep; Sheep:model | `1.7/Models/animal_sheep.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_sheep.obj` | Clay delivery, selected runtime geometry |
| Core:Sheep; Sheep:legModel | `1.7/Models/animal_sheep_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_sheep_leg.obj` | Clay delivery with retained geometry |
| AnimalSleepingBox:model; Core:AnimalSleepingBox | `1.7/Models/animal_sleeping_box.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/animal_sleeping_box.obj` | Clay delivery, selected runtime geometry |
| Core:Snowhare; Snowhare:model | `1.7/Models/animal_snowhare.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_snowhare.obj` | Clay delivery, selected runtime geometry |
| Core:Snowhare; Snowhare:legModel | `1.7/Models/animal_snowhare_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_snowhare_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Spelopede; Spelopede:model | `1.7/Models/animal_spelopede.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_spelopede.obj` | Clay delivery, selected runtime geometry |
| Core:Spelopede; Spelopede:legModel | `1.7/Models/animal_spelopede_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_spelopede_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Squirrel; Squirrel:model | `1.7/Models/animal_squirrel.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_squirrel.obj` | Clay delivery, selected runtime geometry |
| Core:Squirrel; Squirrel:legModel | `1.7/Models/animal_squirrel_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_squirrel_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Thrumbo; Thrumbo:model | `1.7/Models/animal_thrumbo.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_thrumbo.obj` | Clay delivery, selected runtime geometry |
| Core:Thrumbo; Thrumbo:legModel | `1.7/Models/animal_thrumbo_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_thrumbo_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Tortoise; Tortoise:model | `1.7/Models/animal_tortoise.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_tortoise.obj` | Clay delivery, selected runtime geometry |
| Core:Tortoise; Tortoise:legModel | `1.7/Models/animal_tortoise_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_tortoise_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Turkey; Turkey:model | `1.7/Models/animal_turkey.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_turkey.obj` | Clay delivery, selected runtime geometry |
| Core:Turkey; Turkey:legModel | `1.7/Models/animal_turkey_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_turkey_leg.obj` | Clay delivery with retained geometry |
| Core:Warg; Warg:model | `1.7/Models/animal_warg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_warg.obj` | Clay delivery, selected runtime geometry |
| Core:Warg; Warg:legModel | `1.7/Models/animal_warg_leg.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/animal_warg_leg.obj` | Clay delivery, selected runtime geometry |
| Core:WildBoar; WildBoar:model | `1.7/Models/animal_wildboar.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_wildboar.obj` | Clay delivery, selected runtime geometry |
| Core:WildBoar; WildBoar:legModel | `1.7/Models/animal_wildboar_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_wildboar_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Wolf_Arctic; Wolf_Arctic:model | `1.7/Models/animal_wolf_arctic.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_wolf_arctic.obj` | Clay delivery, selected runtime geometry |
| Core:Wolf_Arctic; Wolf_Arctic:legModel | `1.7/Models/animal_wolf_arctic_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_wolf_arctic_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Wolf_Timber; Wolf_Timber:model | `1.7/Models/animal_wolf_timber.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_wolf_timber.obj` | Clay delivery, selected runtime geometry |
| Core:Wolf_Timber; Wolf_Timber:legModel | `1.7/Models/animal_wolf_timber_leg.obj` | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_wolf_timber_leg.obj` | Clay delivery, selected runtime geometry |
| Core:Yak; Yak:model | `1.7/Models/animal_yak.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_yak.obj` | Clay delivery, selected runtime geometry |
| Core:Yak; Yak:legModel | `1.7/Models/animal_yak_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_yak_leg.obj` | Clay delivery with retained geometry |
| Core:YorkshireTerrier; YorkshireTerrier:model | `1.7/Models/animal_yorkie.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_yorkie.obj` | Clay delivery, selected runtime geometry |
| Core:YorkshireTerrier; YorkshireTerrier:legModel | `1.7/Models/animal_yorkie_leg.obj` | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_yorkie_leg.obj` | Clay delivery with retained geometry |
| AnimusStone:model; Royalty:AnimusStone | `1.7/Models/animus_stone.obj` | Z18 / `Models/animus_stone.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton.obj` | Z04 / `Models/apocriton.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_head.obj` | Z04 / `Models/apocriton_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_leg_left_1.obj` | Z04 / `Models/apocriton_leg_left_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_leg_left_2.obj` | Z04 / `Models/apocriton_leg_left_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_leg_right_1.obj` | Z04 / `Models/apocriton_leg_right_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_leg_right_2.obj` | Z04 / `Models/apocriton_leg_right_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_tail.obj` | Z04 / `Models/apocriton_tail.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Apocriton; Mech_Apocriton:model | `1.7/Models/apocriton_weapon.obj` | Z04 / `Models/apocriton_weapon.obj` | Clay delivery with retained geometry |
| Armchair:model; Core:Armchair | `1.7/Models/armchair.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/armchair.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gun_HellcatRifle; Anomaly:Gun_Incinerator; Anomaly:NerveSpiker; Biotech:Flamebow; Biotech:Gun_BeamGraser; Biotech:Gun_HellsphereCannon; Biotech:Gun_MiniFlameblaster; Biotech:Gun_MiniShotgun; Biotech:Gun_NeedleLauncher; Biotech:Gun_Slugthrower; Biotech:Gun_Spiner; Biotech:Gun_ToxbombLauncher; Biotech:Gun_ToxicNeedle; Core:Gun_AssaultRifle; Core:Gun_ChargeBlasterHeavy; Core:Gun_ChargeBlasterHeavyTurret; Core:Gun_InfernoCannon; Core:Gun_InfernoCannonTurret; Core:Gun_Needle; Core:Gun_ThumpCannon; Gun_AssaultRifle:model; Odyssey:Gun_BeamRepeater; Odyssey:Gun_Scattergun; WeaponsRanged:model | `1.7/Models/assault_rifle.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/assault_rifle.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Astragalus; Plant_Astragalus:li | `1.7/Models/astragalus_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/astragalus_1.obj` | Clay delivery with retained geometry |
| Core:Plant_Astragalus; Plant_Astragalus:li | `1.7/Models/astragalus_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/astragalus_2.obj` | Clay delivery with retained geometry |
| Core:MeleeWeapon_BreachAxe; MeleeWeapon_Axe:model; MeleeWeapon_BreachAxe:model; Royalty:MeleeWeapon_Axe | `1.7/Models/axe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/axe.obj` | Clay delivery, selected runtime geometry |
| BabyDecoration:li; Biotech:BabyDecoration | `1.7/Models/baby_car.obj` | Z04 / `Models/baby_car.obj` | Clay delivery, selected runtime geometry |
| BabyDecoration:li; Biotech:BabyDecoration | `1.7/Models/baby_horse.obj` | Z04 / `Models/baby_horse.obj` | Clay delivery with retained geometry |
| BabyDecoration:li; Biotech:BabyDecoration | `1.7/Models/baby_phone.obj` | Z04 / `Models/baby_phone.obj` | Clay delivery, selected runtime geometry |
| BabyDecoration:li; Biotech:BabyDecoration | `1.7/Models/baby_piano.obj` | Z04 / `Models/baby_piano.obj` | Clay delivery, selected runtime geometry |
| BabyDecoration:li; Biotech:BabyDecoration | `1.7/Models/baby_rings.obj` | Z04 / `Models/baby_rings.obj` | Clay delivery with retained geometry |
| BandNode:model; Biotech:BandNode | `1.7/Models/band_node.obj` | Z04 / `Models/band_node.obj` | Clay delivery with retained geometry |
| Core:FermentingBarrel; FermentingBarrel:model | `1.7/Models/barrel.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/barrel.obj` | Clay delivery with retained geometry |
| BasicRecharger:model; Biotech:BasicRecharger | `1.7/Models/basic_recharger.obj` | Z04 / `Models/basic_recharger.obj` | Clay delivery with retained geometry |
| Battery:model; Core:Battery | `1.7/Models/battery.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/battery.obj` | Clay delivery, selected runtime geometry |
| Bed:model; Core:Bed | `1.7/Models/bed.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bed.obj` | Clay delivery, selected runtime geometry |
| Core:DoubleBed; DoubleBed:model | `1.7/Models/bed_double.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/bed_double.obj` | Clay delivery, selected runtime geometry |
| Bedroll:model; Core:Bedroll | `1.7/Models/bedroll.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/bedroll.obj` | Clay delivery, selected runtime geometry |
| BedrollDouble:model; Core:BedrollDouble | `1.7/Models/bedroll_double.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/bedroll_double.obj` | Clay delivery, selected runtime geometry |
| Brewery:model; Core:Brewery | `1.7/Models/bench_brewery.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/bench_brewery.obj` | Clay delivery, selected runtime geometry |
| Core:TableButcher; TableButcher:model | `1.7/Models/bench_butcher.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/bench_butcher.obj` | Clay delivery, selected runtime geometry |
| Core:DrugLab; DrugLab:model | `1.7/Models/bench_drug.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/bench_drug.obj` | Clay delivery, selected runtime geometry |
| Core:ElectricSmithy; ElectricSmithy:model | `1.7/Models/bench_electric_smithy.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bench_electric_smithy.obj` | Clay delivery, selected runtime geometry |
| Core:ElectricTailoringBench; ElectricTailoringBench:model | `1.7/Models/bench_electric_tailor.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/bench_electric_tailor.obj` | Clay delivery, selected runtime geometry |
| Core:FueledStove; FueledStove:model | `1.7/Models/bench_fueled_stove.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/bench_fueled_stove.obj` | Clay delivery, selected runtime geometry |
| Core:HiTechResearchBench; HiTechResearchBench:model | `1.7/Models/bench_hitech_research.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/bench_hitech_research.obj` | Clay delivery, selected runtime geometry |
| Core:TableMachining; TableMachining:model | `1.7/Models/bench_machining.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/bench_machining.obj` | Clay delivery, selected runtime geometry |
| Core:SimpleResearchBench; SimpleResearchBench:model | `1.7/Models/bench_research.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bench_research.obj` | Clay delivery, selected runtime geometry |
| Core:TableSculpting; TableSculpting:model | `1.7/Models/bench_sculpting.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/bench_sculpting.obj` | Clay delivery, selected runtime geometry |
| Core:ElectricSmelter; ElectricSmelter:model | `1.7/Models/bench_smelter.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/bench_smelter.obj` | Clay delivery, selected runtime geometry |
| Core:FueledSmithy; FueledSmithy:model | `1.7/Models/bench_smithy.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/bench_smithy.obj` | Clay delivery, selected runtime geometry |
| Core:TableStonecutter; TableStonecutter:model | `1.7/Models/bench_stonecutter.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/bench_stonecutter.obj` | Clay delivery, selected runtime geometry |
| Core:ElectricStove; ElectricStove:model | `1.7/Models/bench_stove.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bench_stove.obj` | Clay delivery, selected runtime geometry |
| Core:HandTailoringBench; HandTailoringBench:model | `1.7/Models/bench_tailor.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/bench_tailor.obj` | Clay delivery, selected runtime geometry |
| BilliardsTable:model; Core:BilliardsTable | `1.7/Models/billiards_table.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/billiards_table.obj` | Clay delivery, selected runtime geometry |
| Anomaly:BioferriteGenerator; BioferriteGenerator:model | `1.7/Models/bioferrite_generator.obj` | Z03 / `Models/bioferrite_generator.obj` | Clay delivery, selected runtime geometry |
| Anomaly:BioferriteHarvester; BioferriteHarvester:model | `1.7/Models/bioferrite_harvester.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/bioferrite_harvester.obj` | Clay delivery, selected runtime geometry |
| Anomaly:BioferriteShaper; BioferriteShaper:model | `1.7/Models/bioferrite_shaper.obj` | Z03 / `Models/bioferrite_shaper.obj` | Clay delivery, selected runtime geometry |
| BiofuelRefinery:model; Core:BiofuelRefinery | `1.7/Models/biofuel_refinery.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/biofuel_refinery.obj` | Clay delivery, selected runtime geometry |
| Biotech:Blackboard; Blackboard:model | `1.7/Models/blackboard.obj` | Z04 / `Models/blackboard.obj` | Clay delivery with retained geometry |
| AncientBlastDoor:model; Odyssey:AncientBlastDoor | `1.7/Models/blast_door.obj` | Z05 / `Models/blast_door.obj` | Clay delivery with retained geometry |
| AncientBlastDoor:doorLeafModel; Odyssey:AncientBlastDoor | `1.7/Models/blast_leaf.obj` | Z05 / `Models/blast_leaf.obj` | Clay delivery with retained geometry |
| AncientBlastDoor:hackTerminalModel; Odyssey:AncientBlastDoor | `1.7/Models/blast_terminal.obj` | Z05 / `Models/blast_terminal.obj` | Clay delivery with retained geometry |
| AncientBlastDoor:doorTopModel; Odyssey:AncientBlastDoor | `1.7/Models/blast_top.obj` | Z05 / `Models/blast_top.obj` | Clay delivery with retained geometry |
| Core:Gun_BoltActionRifle; Gun_BoltActionRifle:model | `1.7/Models/bolt_rifle.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/bolt_rifle.obj` | Clay delivery, selected runtime geometry |
| Bookcase:model; Core:Bookcase | `1.7/Models/bookcase.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/bookcase.obj` | Clay delivery, selected runtime geometry |
| BookcaseSmall:model; Core:BookcaseSmall | `1.7/Models/bookcase_small.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/bookcase_small.obj` | Clay delivery, selected runtime geometry |
| Core:ChunkGranite; Core:ChunkLimestone; Core:ChunkMarble; Core:ChunkSandstone; Core:ChunkSlate; Odyssey:ChunkVacstone; StoneChunks:li | `1.7/Models/boulder_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/boulder_1.obj` | Clay delivery, selected runtime geometry |
| Core:ChunkGranite; Core:ChunkLimestone; Core:ChunkMarble; Core:ChunkSandstone; Core:ChunkSlate; Odyssey:ChunkVacstone; StoneChunks:li | `1.7/Models/boulder_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/boulder_2.obj` | Clay delivery, selected runtime geometry |
| Core:ChunkGranite; Core:ChunkLimestone; Core:ChunkMarble; Core:ChunkSandstone; Core:ChunkSlate; Odyssey:ChunkVacstone; StoneChunks:li | `1.7/Models/boulder_3.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/boulder_3.obj` | Clay delivery, selected runtime geometry |
| Core:ChunkGranite; Core:ChunkLimestone; Core:ChunkMarble; Core:ChunkSandstone; Core:ChunkSlate; Odyssey:ChunkVacstone; StoneChunks:li | `1.7/Models/boulder_4.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/boulder_4.obj` | Clay delivery, selected runtime geometry |
| Core:ChunkGranite; Core:ChunkLimestone; Core:ChunkMarble; Core:ChunkSandstone; Core:ChunkSlate; Odyssey:ChunkVacstone; StoneChunks:li | `1.7/Models/boulder_5.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/boulder_5.obj` | Clay delivery, selected runtime geometry |
| Core:ChunkGranite; Core:ChunkLimestone; Core:ChunkMarble; Core:ChunkSandstone; Core:ChunkSlate; Odyssey:ChunkVacstone; StoneChunks:li | `1.7/Models/boulder_6.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/boulder_6.obj` | Clay delivery, selected runtime geometry |
| Bow_Great:model; Core:Bow_Great | `1.7/Models/bow_great.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/bow_great.obj` | Clay delivery, selected runtime geometry |
| Bow_Recurve:model; Core:Bow_Recurve | `1.7/Models/bow_recurve.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/bow_recurve.obj` | Clay delivery, selected runtime geometry |
| Bow_Short:model; Core:Bow_Short | `1.7/Models/bow_short.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/bow_short.obj` | Clay delivery, selected runtime geometry |
| BroadshieldProjector:model; Royalty:BroadshieldProjector | `1.7/Models/broadshield_projector.obj` | Z18 / `Models/broadshield_projector.obj` | Clay delivery, selected runtime geometry |
| AncientMechDropBeacon:li; Core:AncientMechDropBeacon | `1.7/Models/broken_beacon_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/broken_beacon_a.obj` | Clay delivery with retained geometry |
| AncientMechDropBeacon:li; Core:AncientMechDropBeacon | `1.7/Models/broken_beacon_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/broken_beacon_b.obj` | Clay delivery with retained geometry |
| AncientMechDropBeacon:li; Core:AncientMechDropBeacon | `1.7/Models/broken_beacon_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/broken_beacon_c.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_appendage_0.obj` | Z03 / `Models/bulbfreak_appendage_0.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_appendage_1.obj` | Z03 / `Models/bulbfreak_appendage_1.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_appendage_2.obj` | Z03 / `Models/bulbfreak_appendage_2.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_appendage_3.obj` | Z03 / `Models/bulbfreak_appendage_3.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_appendage_4.obj` | Z03 / `Models/bulbfreak_appendage_4.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_body.obj` | Z03 / `Models/bulbfreak_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_head.obj` | Z03 / `Models/bulbfreak_head.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_skirt_0.obj` | Z03 / `Models/bulbfreak_skirt_0.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_skirt_1.obj` | Z03 / `Models/bulbfreak_skirt_1.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_skirt_2.obj` | Z03 / `Models/bulbfreak_skirt_2.obj` | Clay delivery with retained geometry |
| Anomaly:Bulbfreak; Bulbfreak:model | `1.7/Models/bulbfreak_skirt_3.obj` | Z03 / `Models/bulbfreak_skirt_3.obj` | Clay delivery with retained geometry |
| Campfire:model; Core:Campfire | `1.7/Models/campfire.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/campfire.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Mech_CentipedeBlaster:model | `1.7/Models/centipede_blaster_head.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_blaster_head.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Mech_CentipedeBlaster:model | `1.7/Models/centipede_blaster_weapon.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_blaster_weapon.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBurner; Mech_CentipedeBurner:model | `1.7/Models/centipede_burner_head.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_burner_head.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBurner; Mech_CentipedeBurner:model | `1.7/Models/centipede_burner_weapon.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_burner_weapon.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeGunner; Mech_CentipedeGunner:model | `1.7/Models/centipede_gunner_head.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_gunner_head.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeGunner; Mech_CentipedeGunner:model | `1.7/Models/centipede_gunner_weapon.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_gunner_weapon.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_left_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_left_1.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_left_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_left_2.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_left_3.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_left_3.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_left_4.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_left_4.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_left_5.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_left_5.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_left_6.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_left_6.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_right_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_right_1.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_right_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_right_2.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_right_3.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_right_3.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_right_4.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_right_4.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_right_5.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_right_5.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_legs_right_6.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_legs_right_6.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_segment_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_segment_2.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_segment_3.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_segment_3.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_segment_4.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_segment_4.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_segment_5.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_segment_5.obj` | Clay delivery with retained geometry |
| Core:Mech_CentipedeBlaster; Core:Mech_CentipedeBurner; Core:Mech_CentipedeGunner; Mech_CentipedeBlaster:model; Mech_CentipedeBurner:model; Mech_CentipedeGunner:model | `1.7/Models/centipede_segment_6.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/centipede_segment_6.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Centurion; Mech_Centurion:model | `1.7/Models/centurion.obj` | Z04 / `Models/centurion.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Centurion; Mech_Centurion:model | `1.7/Models/centurion_generator.obj` | Z04 / `Models/centurion_generator.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Centurion; Mech_Centurion:model | `1.7/Models/centurion_head.obj` | Z04 / `Models/centurion_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Centurion; Mech_Centurion:model | `1.7/Models/centurion_turret_weapon.obj` | Z04 / `Models/centurion_turret_weapon.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Centurion; Mech_Centurion:model | `1.7/Models/centurion_wheel.obj` | Z04 / `Models/centurion_wheel.obj` | Clay delivery with retained geometry |
| Core:Gun_ChainShotgun; Gun_ChainShotgun:model | `1.7/Models/chain_shotgun.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/chain_shotgun.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_ChargeLance; Gun_ChargeLance:model | `1.7/Models/charge_lance.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/charge_lance.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_ChargeRifle; Gun_ChargeRifle:model | `1.7/Models/charge_rifle.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/charge_rifle.obj` | Clay delivery, selected runtime geometry |
| ChemfuelPoweredGenerator:model; Core:ChemfuelPoweredGenerator | `1.7/Models/chemfuel_generator.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/chemfuel_generator.obj` | Clay delivery, selected runtime geometry |
| ChemfuelTank:model; Odyssey:ChemfuelTank | `1.7/Models/chemfuel_tank.obj` | Z05 / `Models/chemfuel_tank.obj` | Clay delivery with retained geometry |
| ChessTable:model; Core:ChessTable | `1.7/Models/chess_table.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/chess_table.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a.obj` | Z03 / `Models/chimera_a.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_body.obj` | Z03 / `Models/chimera_a_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_head.obj` | Z03 / `Models/chimera_a_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_leg_-1_0.obj` | Z03 / `Models/chimera_a_leg_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_leg_-1_1.obj` | Z03 / `Models/chimera_a_leg_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_leg_1_0.obj` | Z03 / `Models/chimera_a_leg_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_leg_1_1.obj` | Z03 / `Models/chimera_a_leg_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_a_tail.obj` | Z03 / `Models/chimera_a_tail.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b.obj` | Z03 / `Models/chimera_b.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_body.obj` | Z03 / `Models/chimera_b_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_head.obj` | Z03 / `Models/chimera_b_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_leg_-1_0.obj` | Z03 / `Models/chimera_b_leg_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_leg_-1_1.obj` | Z03 / `Models/chimera_b_leg_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_leg_1_0.obj` | Z03 / `Models/chimera_b_leg_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_leg_1_1.obj` | Z03 / `Models/chimera_b_leg_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_b_tail.obj` | Z03 / `Models/chimera_b_tail.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c.obj` | Z03 / `Models/chimera_c.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_body.obj` | Z03 / `Models/chimera_c_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_head.obj` | Z03 / `Models/chimera_c_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_leg_-1_0.obj` | Z03 / `Models/chimera_c_leg_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_leg_-1_1.obj` | Z03 / `Models/chimera_c_leg_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_leg_1_0.obj` | Z03 / `Models/chimera_c_leg_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_leg_1_1.obj` | Z03 / `Models/chimera_c_leg_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_c_tail.obj` | Z03 / `Models/chimera_c_tail.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_body.obj` | Z03 / `Models/chimera_d_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_head.obj` | Z03 / `Models/chimera_d_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_leg_-1_0.obj` | Z03 / `Models/chimera_d_leg_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_leg_-1_1.obj` | Z03 / `Models/chimera_d_leg_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_leg_1_0.obj` | Z03 / `Models/chimera_d_leg_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_leg_1_1.obj` | Z03 / `Models/chimera_d_leg_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Chimera; Chimera:model | `1.7/Models/chimera_d_tail.obj` | Z03 / `Models/chimera_d_tail.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Cleansweeper; Mech_Cleansweeper:model | `1.7/Models/cleansweeper.obj` | Z04 / `Models/cleansweeper.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Cleansweeper; Mech_Cleansweeper:model | `1.7/Models/cleansweeper_brush.obj` | Z04 / `Models/cleansweeper_brush.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Cleansweeper; Mech_Cleansweeper:model | `1.7/Models/cleansweeper_wheel_left.obj` | Z04 / `Models/cleansweeper_wheel_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Cleansweeper; Mech_Cleansweeper:model | `1.7/Models/cleansweeper_wheel_right.obj` | Z04 / `Models/cleansweeper_wheel_right.obj` | Clay delivery with retained geometry |
| ClimateAdjuster:model; Royalty:ClimateAdjuster | `1.7/Models/climate_adjuster.obj` | Z18 / `Models/climate_adjuster.obj` | Clay delivery, selected runtime geometry |
| Core:MeleeWeapon_Club; MeleeWeapon_Club:model | `1.7/Models/club.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/club.obj` | Clay delivery, selected runtime geometry |
| Column:model; Core:Column | `1.7/Models/column.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/column.obj` | Clay delivery, selected runtime geometry |
| CommsConsole:model; Core:CommsConsole | `1.7/Models/comms_console.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/comms_console.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Constructoid; Mech_Constructoid:model | `1.7/Models/constructoid.obj` | Z04 / `Models/constructoid.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Constructoid; Mech_Constructoid:model | `1.7/Models/constructoid_sensor.obj` | Z04 / `Models/constructoid_sensor.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Constructoid; Mech_Constructoid:model | `1.7/Models/constructoid_wheel_left.obj` | Z04 / `Models/constructoid_wheel_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Constructoid; Mech_Constructoid:model | `1.7/Models/constructoid_wheel_right.obj` | Z04 / `Models/constructoid_wheel_right.obj` | Clay delivery with retained geometry |
| Cooler:model; Core:Cooler | `1.7/Models/cooler.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/cooler.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Corn; Plant_Corn:li | `1.7/Models/corn_1.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/corn_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Corn; Plant_Corn:li | `1.7/Models/corn_2.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/corn_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Cotton; Plant_Cotton:model | `1.7/Models/cotton.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/cotton.obj` | Clay delivery, selected runtime geometry |
| Core:Couch; Couch:model | `1.7/Models/couch.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/couch.obj` | Clay delivery with retained geometry |
| ActivatorCountdown:model; Royalty:ActivatorCountdown | `1.7/Models/countdown.obj` | Z18 / `Models/countdown.obj` | Clay delivery with retained geometry |
| Core:ElectricCrematorium; ElectricCrematorium:model | `1.7/Models/crematorium.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/crematorium.obj` | Clay delivery, selected runtime geometry |
| Biotech:Crib; Crib:model | `1.7/Models/crib.obj` | Z04 / `Models/crib.obj` | Clay delivery with retained geometry |
| AncientCryptosleepCasket:model; Core:AncientCryptosleepCasket; Core:CryptosleepCasket; CryptosleepCasket:model | `1.7/Models/cryptosleep_casket.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/cryptosleep_casket.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/cult_escapee_eye_negative_x.obj` | Z03 / `Models/cult_escapee_eye_negative_x.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/cult_escapee_eye_positive_x.obj` | Z03 / `Models/cult_escapee_eye_positive_x.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/cult_escapee_face.obj` | Z03 / `Models/cult_escapee_face.obj` | Clay delivery with retained geometry |
| Biotech:DeathrestAccelerator; DeathrestAccelerator:model | `1.7/Models/deathrest_accelerator.obj` | Z04 / `Models/deathrest_accelerator.obj` | Clay delivery with retained geometry |
| Biotech:DeathrestCasket; DeathrestCasket:model | `1.7/Models/deathrest_casket.obj` | Z04 / `Models/deathrest_casket.obj` | Clay delivery with retained geometry |
| Core:DeepDrill; DeepDrill:model | `1.7/Models/deep_drill.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/deep_drill.obj` | Clay delivery, selected runtime geometry |
| Defoliator:model; Royalty:Defoliator | `1.7/Models/defoliator.obj` | Z18 / `Models/defoliator.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Devilstrand; Plant_Devilstrand:model | `1.7/Models/devilstrand.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/devilstrand.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Devourer; Devourer:model | `1.7/Models/devourer_body.obj` | Z03 / `Models/devourer_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Devourer; Devourer:model | `1.7/Models/devourer_fin_-1.obj` | Z03 / `Models/devourer_fin_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Devourer; Devourer:model | `1.7/Models/devourer_fin_1.obj` | Z03 / `Models/devourer_fin_1.obj` | Clay delivery with retained geometry |
| Anomaly:Devourer; Devourer:model | `1.7/Models/devourer_head.obj` | Z03 / `Models/devourer_head.obj` | Clay delivery with retained geometry |
| Anomaly:Devourer; Devourer:model | `1.7/Models/devourer_mouth_closed.obj` | Z03 / `Models/devourer_mouth_closed.obj` | Clay delivery with retained geometry |
| Anomaly:Devourer; Devourer:model | `1.7/Models/devourer_mouth_open.obj` | Z03 / `Models/devourer_mouth_open.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus.obj` | Z04 / `Models/diabolus.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_head.obj` | Z04 / `Models/diabolus_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_left_1.obj` | Z04 / `Models/diabolus_leg_left_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_left_2.obj` | Z04 / `Models/diabolus_leg_left_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_left_3.obj` | Z04 / `Models/diabolus_leg_left_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_left_4.obj` | Z04 / `Models/diabolus_leg_left_4.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_left_5.obj` | Z04 / `Models/diabolus_leg_left_5.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_left_6.obj` | Z04 / `Models/diabolus_leg_left_6.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_right_1.obj` | Z04 / `Models/diabolus_leg_right_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_right_2.obj` | Z04 / `Models/diabolus_leg_right_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_right_3.obj` | Z04 / `Models/diabolus_leg_right_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_right_4.obj` | Z04 / `Models/diabolus_leg_right_4.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_right_5.obj` | Z04 / `Models/diabolus_leg_right_5.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_leg_right_6.obj` | Z04 / `Models/diabolus_leg_right_6.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_segment_2.obj` | Z04 / `Models/diabolus_segment_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_segment_3.obj` | Z04 / `Models/diabolus_segment_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_segment_4.obj` | Z04 / `Models/diabolus_segment_4.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_segment_5.obj` | Z04 / `Models/diabolus_segment_5.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_segment_6.obj` | Z04 / `Models/diabolus_segment_6.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_turret_weapon.obj` | Z04 / `Models/diabolus_turret_weapon.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Diabolus; Mech_Diabolus:model | `1.7/Models/diabolus_weapon.obj` | Z04 / `Models/diabolus_weapon.obj` | Clay delivery with retained geometry |
| Core:DiningChair; DiningChair:model | `1.7/Models/dining_chair.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/dining_chair.obj` | Clay delivery, selected runtime geometry |
| Anomaly:GrayDoor; Core:AnimalFlap; Core:Autodoor; Core:Door; Core:FenceGate; Source17 internal part (literal filename) | `1.7/Models/door_leaf.obj` | Z02 / `Models/door_leaf.obj` | Clay delivery, prior approved export reused |
| Source17 internal part (literal filename) | `1.7/Models/door_lintel.obj` | Z02 / `Models/door_lintel.obj` | Clay delivery, prior approved export reused |
| Drape:model; Royalty:Drape | `1.7/Models/drape.obj` | Z18 / `Models/drape.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_appendage_0.obj` | Z03 / `Models/dreadmeld_appendage_0.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_appendage_1.obj` | Z03 / `Models/dreadmeld_appendage_1.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_body.obj` | Z03 / `Models/dreadmeld_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_head.obj` | Z03 / `Models/dreadmeld_head.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_skirt_0.obj` | Z03 / `Models/dreadmeld_skirt_0.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_skirt_1.obj` | Z03 / `Models/dreadmeld_skirt_1.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_skirt_2.obj` | Z03 / `Models/dreadmeld_skirt_2.obj` | Clay delivery with retained geometry |
| Anomaly:Dreadmeld; Dreadmeld:model | `1.7/Models/dreadmeld_skirt_3.obj` | Z03 / `Models/dreadmeld_skirt_3.obj` | Clay delivery with retained geometry |
| Core:Dresser; Dresser:model | `1.7/Models/dresser.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/dresser.obj` | Clay delivery, selected runtime geometry |
| MechDropBeacon:model; Royalty:MechDropBeacon | `1.7/Models/drop_beacon.obj` | Z18 / `Models/drop_beacon.obj` | Clay delivery with retained geometry |
| Core:Flake; Flake:model | `1.7/Models/drug_flake.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/drug_flake.obj` | Clay delivery, selected runtime geometry |
| Core:SmokeleafJoint; SmokeleafJoint:model | `1.7/Models/drug_joints.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/drug_joints.obj` | Clay delivery, selected runtime geometry |
| Core:Luciferium; Luciferium:model | `1.7/Models/drug_luciferium.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/drug_luciferium.obj` | Clay delivery, selected runtime geometry |
| Anomaly:GhoulResurrectionSerum; Anomaly:JuggernautSerum; Anomaly:MetalbloodSerum; Anomaly:MindNumbSerum; Anomaly:VoidsightSerum; Biotech:DeathrestCapacitySerum; Core:MechSerumHealer; Core:MechSerumResurrector; Core:Penoxycyline; Core:WakeUp; Core:Yayo; Drugs:model; Odyssey:Drug_Psilocap; Odyssey:SentienceCatalyst; Penoxycyline:model; WakeUp:model; Yayo:model | `1.7/Models/drug_pills.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/drug_pills.obj` | Clay delivery, selected runtime geometry |
| Core:GoJuice; GoJuice:model | `1.7/Models/drug_syringe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/drug_syringe.obj` | Clay delivery, selected runtime geometry |
| Core:PsychiteTea; PsychiteTea:model | `1.7/Models/drug_tea.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/drug_tea.obj` | Clay delivery, selected runtime geometry |
| Core:EggBox; EggBox:model | `1.7/Models/egg_box.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/egg_box.obj` | Clay delivery, selected runtime geometry |
| Anomaly:ElectricInhibitor; ElectricInhibitor:model | `1.7/Models/electric_inhibitor.obj` | Z03 / `Models/electric_inhibitor.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Electroharvester; Electroharvester:model | `1.7/Models/electroharvester.obj` | Z03 / `Models/electroharvester.obj` | Clay delivery, selected runtime geometry |
| EMIDynamo:model; Royalty:EMIDynamo | `1.7/Models/emi_dynamo.obj` | Z18 / `Models/emi_dynamo.obj` | Clay delivery, selected runtime geometry |
| Core:EndTable; EndTable:model | `1.7/Models/end_table.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/end_table.obj` | Clay delivery, selected runtime geometry |
| Core:FabricationBench; FabricationBench:model | `1.7/Models/fabrication_bench.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/fabrication_bench.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Fabricor; Mech_Fabricor:model | `1.7/Models/fabricor.obj` | Z04 / `Models/fabricor.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Fabricor; Mech_Fabricor:model | `1.7/Models/fabricor_head.obj` | Z04 / `Models/fabricor_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Fabricor; Mech_Fabricor:model | `1.7/Models/fabricor_wheel.obj` | Z04 / `Models/fabricor_wheel.obj` | Clay delivery with retained geometry |
| Core:Fence; Fence:li | `1.7/Models/fence_arm.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/fence_arm.obj` | Clay delivery, selected runtime geometry |
| Core:Fence; Fence:li | `1.7/Models/fence_post.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/fence_post.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_appendage_0.obj` | Z03 / `Models/fingerspike_appendage_0.obj` | Clay delivery with retained geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_body.obj` | Z03 / `Models/fingerspike_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_head.obj` | Z03 / `Models/fingerspike_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_skirt_0.obj` | Z03 / `Models/fingerspike_skirt_0.obj` | Clay delivery with retained geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_skirt_1.obj` | Z03 / `Models/fingerspike_skirt_1.obj` | Clay delivery with retained geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_skirt_2.obj` | Z03 / `Models/fingerspike_skirt_2.obj` | Clay delivery with retained geometry |
| Anomaly:Fingerspike; Fingerspike:model | `1.7/Models/fingerspike_skirt_3.obj` | Z03 / `Models/fingerspike_skirt_3.obj` | Clay delivery with retained geometry |
| Core:FirefoamPopper; FirefoamPopper:model | `1.7/Models/firefoam_popper.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/firefoam_popper.obj` | Clay delivery with retained geometry |
| Core:FloodLight; FloodLight:model | `1.7/Models/flood_light.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/flood_light.obj` | Clay delivery with retained geometry |
| Core:Turret_FoamTurret; Turret_FoamTurret:model | `1.7/Models/foam_base.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/foam_base.obj` | Clay delivery with retained geometry |
| Core:Turret_FoamTurret; Turret_FoamTurret:topModel | `1.7/Models/foam_top.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/foam_top.obj` | Clay delivery, selected runtime geometry |
| Core:RawAgave; RawAgave:li | `1.7/Models/food_agave_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_agave_a.obj` | Clay delivery, selected runtime geometry |
| Core:RawAgave; RawAgave:li | `1.7/Models/food_agave_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_agave_b.obj` | Clay delivery, selected runtime geometry |
| Core:RawAgave; RawAgave:li; RawAgave:model | `1.7/Models/food_agave_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_agave_c.obj` | Clay delivery, selected runtime geometry |
| Beer:model; Core:Beer | `1.7/Models/food_beer.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_beer.obj` | Clay delivery, selected runtime geometry |
| Core:RawBerries; RawBerries:li | `1.7/Models/food_berries_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_berries_a.obj` | Clay delivery, selected runtime geometry |
| Core:RawBerries; RawBerries:li | `1.7/Models/food_berries_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_berries_b.obj` | Clay delivery, selected runtime geometry |
| Core:RawBerries; RawBerries:li; RawBerries:model | `1.7/Models/food_berries_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_berries_c.obj` | Clay delivery, selected runtime geometry |
| Chocolate:model; Core:Chocolate | `1.7/Models/food_chocolate.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_chocolate.obj` | Clay delivery, selected runtime geometry |
| Core:RawCorn; RawCorn:li | `1.7/Models/food_corn_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_corn_a.obj` | Clay delivery, selected runtime geometry |
| Core:RawCorn; RawCorn:li | `1.7/Models/food_corn_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_corn_b.obj` | Clay delivery, selected runtime geometry |
| Core:RawCorn; RawCorn:li; RawCorn:model | `1.7/Models/food_corn_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_corn_c.obj` | Clay delivery, selected runtime geometry |
| Core:EggCassowaryFertilized; Core:EggCassowaryUnfertilized; Core:EggChickenFertilized; Core:EggChickenUnfertilized; Core:EggCobraFertilized; Core:EggCobraUnfertilized; Core:EggDuckFertilized; Core:EggDuckUnfertilized; Core:EggEmuFertilized; Core:EggEmuUnfertilized; Core:EggGooseFertilized; Core:EggGooseUnfertilized; Core:EggIguanaFertilized; Core:EggIguanaUnfertilized; Core:EggOstrichFertilized; Core:EggOstrichUnfertilized; Core:EggTortoiseFertilized; Core:EggTortoiseUnfertilized; Core:EggTurkeyFertilized; Core:EggTurkeyUnfertilized; EggsFertilized:model; EggsUnfertilized:model; Odyssey:EggAlligatorFertilized; Odyssey:EggAlligatorUnfertilized; Odyssey:EggBluebirdFertilized; Odyssey:EggBluebirdUnfertilized; Odyssey:EggCrowFertilized; Odyssey:EggCrowUnfertilized; Odyssey:EggFlamingoFertilized; Odyssey:EggFlamingoUnfertilized; Odyssey:EggHeronFertilized; Odyssey:EggHeronUnfertilized; Odyssey:EggMacawFertilized; Odyssey:EggMacawUnfertilized; Odyssey:EggMonitorLizardFertilized; Odyssey:EggMonitorLizardUnfertilized; Odyssey:EggPeacockFertilized; Odyssey:EggPeacockUnfertilized; Odyssey:EggPenguinFertilized; Odyssey:EggPenguinUnfertilized; Odyssey:EggQuailFertilized; Odyssey:EggQuailUnfertilized; Odyssey:EggSeaTurtleFertilized; Odyssey:EggSeaTurtleUnfertilized; Odyssey:EggSparrowFertilized; Odyssey:EggSparrowUnfertilized; Odyssey:EggSwanFertilized; Odyssey:EggSwanUnfertilized; Odyssey:EggVultureFertilized; Odyssey:EggVultureUnfertilized | `1.7/Models/food_eggs.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/food_eggs.obj` | Clay delivery, selected runtime geometry |
| Core:RawFungus; RawFungus:model | `1.7/Models/food_fungus.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_fungus.obj` | Clay delivery, selected runtime geometry |
| Core:Hay; Hay:li | `1.7/Models/food_hay_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_hay_a.obj` | Clay delivery, selected runtime geometry |
| Core:Hay; Hay:li | `1.7/Models/food_hay_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_hay_b.obj` | Clay delivery, selected runtime geometry |
| Core:Hay; Hay:li; Hay:model | `1.7/Models/food_hay_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_hay_c.obj` | Clay delivery, selected runtime geometry |
| Core:RawHops; RawHops:model | `1.7/Models/food_hops.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_hops.obj` | Clay delivery, selected runtime geometry |
| Core:InsectJelly; InsectJelly:model | `1.7/Models/food_jelly.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_jelly.obj` | Clay delivery, selected runtime geometry |
| Core:SmokeleafLeaves; SmokeleafLeaves:model | `1.7/Models/food_leaves.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_leaves.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Meat_Twisted; MeatRaw:model | `1.7/Models/food_meat.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/food_meat.obj` | Clay delivery, selected runtime geometry |
| Core:Milk; Milk:model | `1.7/Models/food_milk.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_milk.obj` | Clay delivery, selected runtime geometry |
| Core:RawPotatoes; RawPotatoes:li | `1.7/Models/food_potato_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_potato_a.obj` | Clay delivery, selected runtime geometry |
| Core:RawPotatoes; RawPotatoes:li | `1.7/Models/food_potato_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_potato_b.obj` | Clay delivery, selected runtime geometry |
| Core:RawPotatoes; RawPotatoes:li; RawPotatoes:model | `1.7/Models/food_potato_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_potato_c.obj` | Clay delivery, selected runtime geometry |
| Core:PsychoidLeaves; PsychoidLeaves:model | `1.7/Models/food_psychoid.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/food_psychoid.obj` | Clay delivery, selected runtime geometry |
| Core:RawRice; RawRice:li | `1.7/Models/food_rice_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_rice_a.obj` | Clay delivery, selected runtime geometry |
| Core:RawRice; RawRice:li | `1.7/Models/food_rice_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_rice_b.obj` | Clay delivery, selected runtime geometry |
| Core:RawRice; RawRice:li; RawRice:model | `1.7/Models/food_rice_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/food_rice_c.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/fx_flame.obj` | Z02 / `Models/fx_flame.obj` | Clay delivery, prior approved export reused |
| Source17 internal part (literal filename) | `1.7/Models/fx_flame_core.obj` | Z02 / `Models/fx_flame_core.obj` | Clay delivery, prior approved export reused |
| Source17 internal part (literal filename) | `1.7/Models/fx_puff.obj` | Z02 / `Models/fx_puff.obj` | Clay delivery, prior approved export reused |
| Core:GameOfUrBoard; GameOfUrBoard:model | `1.7/Models/game_of_ur.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/game_of_ur.obj` | Clay delivery, selected runtime geometry |
| Apparel:li; ApparelNoble:li; Core:Apparel_Cape; Royalty:Apparel_Corset; Royalty:Apparel_RobeRoyal; Royalty:Apparel_ShirtRuffle; Royalty:Apparel_VestRoyal | `1.7/Models/garment_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/garment_1.obj` | Clay delivery, selected runtime geometry |
| Apparel:li; ApparelNoble:li; Core:Apparel_Cape; Royalty:Apparel_Corset; Royalty:Apparel_RobeRoyal; Royalty:Apparel_ShirtRuffle; Royalty:Apparel_VestRoyal | `1.7/Models/garment_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/garment_2.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/gate_leaf.obj` | Z02 / `Models/gate_leaf.obj` | Clay delivery, prior approved export reused |
| Source17 internal part (literal filename) | `1.7/Models/gate_posts.obj` | Z02 / `Models/gate_posts.obj` | Clay delivery, prior approved export reused |
| Biotech:GeneAssembler; GeneAssembler:model | `1.7/Models/gene_assembler.obj` | Z04 / `Models/gene_assembler.obj` | Clay delivery with retained geometry |
| Biotech:GeneBank; GeneBank:model | `1.7/Models/gene_bank.obj` | Z04 / `Models/gene_bank.obj` | Clay delivery with retained geometry |
| Biotech:GeneExtractor; GeneExtractor:model | `1.7/Models/gene_extractor.obj` | Z04 / `Models/gene_extractor.obj` | Clay delivery with retained geometry |
| Biotech:GeneProcessor; GeneProcessor:model | `1.7/Models/gene_processor.obj` | Z04 / `Models/gene_processor.obj` | Clay delivery with retained geometry |
| Core:GeothermalGenerator; GeothermalGenerator:model | `1.7/Models/geothermal.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/geothermal.obj` | Clay delivery, selected runtime geometry |
| Core:SteamGeyser; SteamGeyser:model | `1.7/Models/geyser.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/geyser.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_fat_body_overlay.obj` | Z03 / `Models/ghoul_fat_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_female_body_overlay.obj` | Z03 / `Models/ghoul_female_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_head_heavy.obj` | Z03 / `Models/ghoul_head_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_head_narrow.obj` | Z03 / `Models/ghoul_head_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_head_normal.obj` | Z03 / `Models/ghoul_head_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_head_wide.obj` | Z03 / `Models/ghoul_head_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_hulk_body_overlay.obj` | Z03 / `Models/ghoul_hulk_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_male_body_overlay.obj` | Z03 / `Models/ghoul_male_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/ghoul_thin_body_overlay.obj` | Z03 / `Models/ghoul_thin_body_overlay.obj` | Clay delivery with retained geometry |
| Core:MeleeWeapon_Gladius; MeleeWeapon_Gladius:model; Royalty:MeleeWeapon_MonoSword; Royalty:MeleeWeapon_PlasmaSword; Royalty:MeleeWeapon_PsyfocusStaff; Royalty:MeleeWeapon_Zeushammer; WeaponsMelee:model | `1.7/Models/gladius.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/gladius.obj` | Clay delivery, selected runtime geometry |
| Core:Glowstool; Glowstool:model; Plant_Agarilux:model; Plant_Bryolux:model | `1.7/Models/glow_mushroom.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/glow_mushroom.obj` | Clay delivery with retained geometry |
| Biotech:GlucosoidPump; GlucosoidPump:model | `1.7/Models/glucosoid_pump.obj` | Z04 / `Models/glucosoid_pump.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_a_arm_-1.obj` | Z03 / `Models/gorehulk_a_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_a_arm_1.obj` | Z03 / `Models/gorehulk_a_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_a_body.obj` | Z03 / `Models/gorehulk_a_body.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_a_head.obj` | Z03 / `Models/gorehulk_a_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_a_leg_-1.obj` | Z03 / `Models/gorehulk_a_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_a_leg_1.obj` | Z03 / `Models/gorehulk_a_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_b_arm_-1.obj` | Z03 / `Models/gorehulk_b_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_b_arm_1.obj` | Z03 / `Models/gorehulk_b_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_b_body.obj` | Z03 / `Models/gorehulk_b_body.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_b_head.obj` | Z03 / `Models/gorehulk_b_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_b_leg_-1.obj` | Z03 / `Models/gorehulk_b_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_b_leg_1.obj` | Z03 / `Models/gorehulk_b_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_c_arm_-1.obj` | Z03 / `Models/gorehulk_c_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_c_arm_1.obj` | Z03 / `Models/gorehulk_c_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_c_body.obj` | Z03 / `Models/gorehulk_c_body.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_c_head.obj` | Z03 / `Models/gorehulk_c_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_c_leg_-1.obj` | Z03 / `Models/gorehulk_c_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_c_leg_1.obj` | Z03 / `Models/gorehulk_c_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_d_arm_-1.obj` | Z03 / `Models/gorehulk_d_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_d_arm_1.obj` | Z03 / `Models/gorehulk_d_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_d_body.obj` | Z03 / `Models/gorehulk_d_body.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_d_head.obj` | Z03 / `Models/gorehulk_d_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_d_leg_-1.obj` | Z03 / `Models/gorehulk_d_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_d_leg_1.obj` | Z03 / `Models/gorehulk_d_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_e_arm_-1.obj` | Z03 / `Models/gorehulk_e_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_e_arm_1.obj` | Z03 / `Models/gorehulk_e_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_e_body.obj` | Z03 / `Models/gorehulk_e_body.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_e_head.obj` | Z03 / `Models/gorehulk_e_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_e_leg_-1.obj` | Z03 / `Models/gorehulk_e_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_e_leg_1.obj` | Z03 / `Models/gorehulk_e_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_f_arm_-1.obj` | Z03 / `Models/gorehulk_f_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_f_arm_1.obj` | Z03 / `Models/gorehulk_f_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_f_body.obj` | Z03 / `Models/gorehulk_f_body.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_f_head.obj` | Z03 / `Models/gorehulk_f_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_f_leg_-1.obj` | Z03 / `Models/gorehulk_f_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Gorehulk; Gorehulk:model | `1.7/Models/gorehulk_f_leg_1.obj` | Z03 / `Models/gorehulk_f_leg_1.obj` | Clay delivery with retained geometry |
| Core:GroundPenetratingScanner; GroundPenetratingScanner:model | `1.7/Models/gp_scanner.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/gp_scanner.obj` | Clay delivery, selected runtime geometry |
| GrandThrone:model; Royalty:GrandThrone | `1.7/Models/grand_throne.obj` | Z18 / `Models/grand_throne.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Grass; Plant_Grass:li | `1.7/Models/grass_1.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/grass_1.obj` | Clay delivery, selected runtime geometry |
| GravFieldExtender:model; Odyssey:GravFieldExtender | `1.7/Models/grav_extender.obj` | Z05 / `Models/grav_extender.obj` | Clay delivery, selected runtime geometry |
| Biotech:Weapon_GrenadeTox; Core:Weapon_GrenadeEMP; Core:Weapon_GrenadeFrag; Grenades:model; Weapon_GrenadeEMP:model; Weapon_GrenadeFrag:model | `1.7/Models/grenade.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/grenade.obj` | Clay delivery, selected runtime geometry |
| Biotech:GrowthVat; GrowthVat:model | `1.7/Models/growth_vat.obj` | Z04 / `Models/growth_vat.obj` | Clay delivery with retained geometry |
| Biotech:GrowthVat; GrowthVat:glassModel | `1.7/Models/growth_vat_glass.obj` | Z04 / `Models/growth_vat_glass.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_furious_heavy.obj` | Z03 / `Models/hairs_furious_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_furious_narrow.obj` | Z03 / `Models/hairs_furious_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_furious_normal.obj` | Z03 / `Models/hairs_furious_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_furious_wide.obj` | Z03 / `Models/hairs_furious_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_hungry_heavy.obj` | Z03 / `Models/hairs_hungry_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_hungry_narrow.obj` | Z03 / `Models/hairs_hungry_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_hungry_normal.obj` | Z03 / `Models/hairs_hungry_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_hungry_wide.obj` | Z03 / `Models/hairs_hungry_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_ravenous_heavy.obj` | Z03 / `Models/hairs_ravenous_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_ravenous_narrow.obj` | Z03 / `Models/hairs_ravenous_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_ravenous_normal.obj` | Z03 / `Models/hairs_ravenous_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_ravenous_wide.obj` | Z03 / `Models/hairs_ravenous_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_twisted_heavy.obj` | Z03 / `Models/hairs_twisted_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_twisted_narrow.obj` | Z03 / `Models/hairs_twisted_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_twisted_normal.obj` | Z03 / `Models/hairs_twisted_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_twisted_wide.obj` | Z03 / `Models/hairs_twisted_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_vengeful_heavy.obj` | Z03 / `Models/hairs_vengeful_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_vengeful_narrow.obj` | Z03 / `Models/hairs_vengeful_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_vengeful_normal.obj` | Z03 / `Models/hairs_vengeful_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_vengeful_wide.obj` | Z03 / `Models/hairs_vengeful_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_warped_heavy.obj` | Z03 / `Models/hairs_warped_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_warped_narrow.obj` | Z03 / `Models/hairs_warped_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_warped_normal.obj` | Z03 / `Models/hairs_warped_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_warped_wide.obj` | Z03 / `Models/hairs_warped_wide.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_wicked_heavy.obj` | Z03 / `Models/hairs_wicked_heavy.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_wicked_narrow.obj` | Z03 / `Models/hairs_wicked_narrow.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_wicked_normal.obj` | Z03 / `Models/hairs_wicked_normal.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/hairs_wicked_wide.obj` | Z03 / `Models/hairs_wicked_wide.obj` | Clay delivery with retained geometry |
| Harp:model; Royalty:Harp | `1.7/Models/harp.obj` | Z18 / `Models/harp.obj` | Clay delivery, selected runtime geometry |
| Harpsichord:model; Royalty:Harpsichord | `1.7/Models/harpsichord.obj` | Z18 / `Models/harpsichord.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Apparel_CeremonialCultistMask; Anomaly:Apparel_CultistMask; Biotech:Apparel_AirwireHeadset; Biotech:Apparel_ArrayHeadset; Biotech:Apparel_GasMask; Core:Apparel_BowlerHat; Core:Apparel_ClothMask; Core:Apparel_CowboyHat; Core:Apparel_HatHood; Core:Apparel_PsychicFoilHelmet; Core:Apparel_TribalHeaddress; Core:Apparel_Tuque; Core:Apparel_WarMask; Core:Apparel_WarVeil; Headgear:model; Ideology:Apparel_AuthorityCap; Ideology:Apparel_Blindfold; Ideology:Apparel_Broadwrap; Ideology:Apparel_Collar; Ideology:Apparel_Flophat; Ideology:Apparel_Headwrap; Ideology:Apparel_Shadecone; Ideology:Apparel_Slicecap; Ideology:Apparel_Tailcap; Ideology:Apparel_TortureCrown; Ideology:Apparel_VisageMask; Royalty:Apparel_EltexSkullcap; Royalty:Apparel_PsyfocusHelmet | `1.7/Models/hat.obj` | Z02 / `Models/hat.obj` | Clay delivery, selected runtime geometry |
| AncientHatch:li; Odyssey:AncientHatch | `1.7/Models/hatch_cover.obj` | Z05 / `Models/hatch_cover.obj` | Clay delivery with retained geometry |
| AncientHatch:model; AncientHatch:openModel; Odyssey:AncientHatch | `1.7/Models/hatch_entrance.obj` | Z05 / `Models/hatch_entrance.obj` | Clay delivery with retained geometry |
| AncientHatchExit:model; Odyssey:AncientHatchExit | `1.7/Models/hatch_exit.obj` | Z05 / `Models/hatch_exit.obj` | Clay delivery with retained geometry |
| AncientHatchExit:li; Odyssey:AncientHatchExit | `1.7/Models/hatch_ladder.obj` | Z05 / `Models/hatch_ladder.obj` | Clay delivery with retained geometry |
| AncientHatch:li; Odyssey:AncientHatch | `1.7/Models/hatch_readout.obj` | Z05 / `Models/hatch_readout.obj` | Clay delivery with retained geometry |
| AncientHatch:li; Odyssey:AncientHatch | `1.7/Models/hatch_well.obj` | Z05 / `Models/hatch_well.obj` | Clay delivery with retained geometry |
| Core:Plant_Healroot; Core:Plant_HealrootWild; Plant_Healroot:li; Plant_HealrootWild:li | `1.7/Models/healroot_1.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/healroot_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Healroot; Core:Plant_HealrootWild; Plant_Healroot:li; Plant_HealrootWild:li | `1.7/Models/healroot_2.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/healroot_2.obj` | Clay delivery, selected runtime geometry |
| Core:Heater; Heater:model | `1.7/Models/heater.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/heater.obj` | Clay delivery, selected runtime geometry |
| ArmorHeadgear:model; Biotech:Apparel_ArmorHelmetMechCommander; Biotech:Apparel_ArmorHelmetMechlordHelmet; Biotech:Apparel_IntegratorHeadset; Biotech:Apparel_KidHelmet; Core:Apparel_AdvancedHelmet; Core:Apparel_ArmorHelmetRecon; Core:Apparel_PowerArmorHelmet; Core:Apparel_SimpleHelmet; Odyssey:Apparel_VacsuitHelmet; Royalty:Apparel_ArmorHelmetCataphract; Royalty:Apparel_ArmorHelmetCataphractPrestige; Royalty:Apparel_ArmorHelmetReconPrestige; Royalty:Apparel_ArmorMarineHelmetPrestige; Royalty:Apparel_Gunlink; Source17 internal part (literal filename) | `1.7/Models/helmet.obj` | Z02 / `Models/helmet.obj` | Clay delivery, selected runtime geometry |
| Biotech:HemogenAmplifier; HemogenAmplifier:model | `1.7/Models/hemogen_amplifier.obj` | Z04 / `Models/hemogen_amplifier.obj` | Clay delivery with retained geometry |
| Biotech:Hemopump; Hemopump:model | `1.7/Models/hemopump.obj` | Z04 / `Models/hemopump.obj` | Clay delivery with retained geometry |
| Biotech:Plant_GrayGrass; Biotech:Plant_Ripthorn; Biotech:Plant_Toxipotato; Core:Agarilux; Core:Bryolux; Core:Plant_Ambrosia; Core:Plant_Chokevine; Core:Plant_Rafflesia; Core:Plant_ShrubLow; Core:Plant_Tinctoria; Ideology:Plant_Fibercorn; Ideology:Plant_MossGauranlen; Ideology:Plant_Nutrifungus; Odyssey:Boomshroom; Odyssey:Plant_Berry_Leafless; Odyssey:Plant_Brambles_Leafless; Odyssey:Plant_Bulrush; Odyssey:Plant_Bush_Leafless; Odyssey:Plant_Cotton_Wild; Odyssey:Plant_Fibercorn_Wild; Odyssey:Plant_Fireweed; Odyssey:Plant_LilyPad; Odyssey:Plant_Lotus; Odyssey:Plant_MagmaCactus; Odyssey:Plant_NightRafflesia; Odyssey:Plant_Nightgrass; Odyssey:Plant_Psilocap; Odyssey:Plant_Psychoid_Wild; Odyssey:Plant_Reeds; Odyssey:Plant_Smokeleaf_Wild; Odyssey:Plant_Strawberry_Wild; Odyssey:Plant_Thornvine; Odyssey:Plant_Tinctoria_Wild; Odyssey:Plant_Willowgill; Odyssey:Plant_YellowGrass; Odyssey:Plant_YellowTallGrass; R3D_Fallback_SmallPlants:li; Royalty:Plant_GrassAnima | `1.7/Models/herb_1.obj` | Z02 / `Models/herb_1.obj` | Clay delivery, selected runtime geometry |
| Biotech:Plant_GrayGrass; Biotech:Plant_Ripthorn; Biotech:Plant_Toxipotato; Core:Agarilux; Core:Bryolux; Core:Plant_Ambrosia; Core:Plant_Chokevine; Core:Plant_Rafflesia; Core:Plant_ShrubLow; Core:Plant_Tinctoria; Ideology:Plant_Fibercorn; Ideology:Plant_MossGauranlen; Ideology:Plant_Nutrifungus; Odyssey:Boomshroom; Odyssey:Plant_Berry_Leafless; Odyssey:Plant_Brambles_Leafless; Odyssey:Plant_Bulrush; Odyssey:Plant_Bush_Leafless; Odyssey:Plant_Cotton_Wild; Odyssey:Plant_Fibercorn_Wild; Odyssey:Plant_Fireweed; Odyssey:Plant_LilyPad; Odyssey:Plant_Lotus; Odyssey:Plant_MagmaCactus; Odyssey:Plant_NightRafflesia; Odyssey:Plant_Nightgrass; Odyssey:Plant_Psilocap; Odyssey:Plant_Psychoid_Wild; Odyssey:Plant_Reeds; Odyssey:Plant_Smokeleaf_Wild; Odyssey:Plant_Strawberry_Wild; Odyssey:Plant_Thornvine; Odyssey:Plant_Tinctoria_Wild; Odyssey:Plant_Willowgill; Odyssey:Plant_YellowGrass; Odyssey:Plant_YellowTallGrass; R3D_Fallback_SmallPlants:li; Royalty:Plant_GrassAnima | `1.7/Models/herb_2.obj` | Z02 / `Models/herb_2.obj` | Clay delivery, selected runtime geometry |
| Biotech:Plant_GrayGrass; Biotech:Plant_Ripthorn; Biotech:Plant_Toxipotato; Core:Agarilux; Core:Bryolux; Core:Plant_Ambrosia; Core:Plant_Chokevine; Core:Plant_Rafflesia; Core:Plant_ShrubLow; Core:Plant_Tinctoria; Ideology:Plant_Fibercorn; Ideology:Plant_MossGauranlen; Ideology:Plant_Nutrifungus; Odyssey:Boomshroom; Odyssey:Plant_Berry_Leafless; Odyssey:Plant_Brambles_Leafless; Odyssey:Plant_Bulrush; Odyssey:Plant_Bush_Leafless; Odyssey:Plant_Cotton_Wild; Odyssey:Plant_Fibercorn_Wild; Odyssey:Plant_Fireweed; Odyssey:Plant_LilyPad; Odyssey:Plant_Lotus; Odyssey:Plant_MagmaCactus; Odyssey:Plant_NightRafflesia; Odyssey:Plant_Nightgrass; Odyssey:Plant_Psilocap; Odyssey:Plant_Psychoid_Wild; Odyssey:Plant_Reeds; Odyssey:Plant_Smokeleaf_Wild; Odyssey:Plant_Strawberry_Wild; Odyssey:Plant_Thornvine; Odyssey:Plant_Tinctoria_Wild; Odyssey:Plant_Willowgill; Odyssey:Plant_YellowGrass; Odyssey:Plant_YellowTallGrass; R3D_Fallback_SmallPlants:li; Royalty:Plant_GrassAnima | `1.7/Models/herb_3.obj` | Z02 / `Models/herb_3.obj` | Clay delivery, selected runtime geometry |
| Core:Hive; Hive:li | `1.7/Models/hive_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/hive_a.obj` | Clay delivery with retained geometry |
| Core:Hive; Hive:li | `1.7/Models/hive_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/hive_b.obj` | Clay delivery with retained geometry |
| Core:Hive; Hive:li | `1.7/Models/hive_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/hive_c.obj` | Clay delivery with retained geometry |
| Anomaly:HoldingPlatform; HoldingPlatform:model | `1.7/Models/holding_platform.obj` | Z03 / `Models/holding_platform.obj` | Clay delivery with retained geometry |
| Core:HoopstoneRing; HoopstoneRing:model | `1.7/Models/hoopstone_ring.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/hoopstone_ring.obj` | Clay delivery, selected runtime geometry |
| Core:Hopper; Hopper:model | `1.7/Models/hopper.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/hopper.obj` | Clay delivery, selected runtime geometry |
| Core:HorseshoesPin; HorseshoesPin:model | `1.7/Models/horseshoes_pin.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/horseshoes_pin.obj` | Clay delivery, selected runtime geometry |
| Core:HospitalBed; HospitalBed:model | `1.7/Models/hospital_bed.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/hospital_bed.obj` | Clay delivery, selected runtime geometry |
| Core:HydroponicsBasin; HydroponicsBasin:model | `1.7/Models/hydroponics.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/hydroponics.obj` | Clay delivery, selected runtime geometry |
| Core:TrapIED_AntigrainWarhead; TrapIED_AntigrainWarhead:model | `1.7/Models/ied_antigrainwarhead.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ied_antigrainwarhead.obj` | Clay delivery, selected runtime geometry |
| Anomaly:TrapIED_Deadlife; TrapIED_Deadlife:model | `1.7/Models/ied_deadlife.obj` | Z03 / `Models/ied_deadlife.obj` | Clay delivery with retained geometry |
| Core:TrapIED_EMP; TrapIED_EMP:model | `1.7/Models/ied_emp.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ied_emp.obj` | Clay delivery, selected runtime geometry |
| Core:TrapIED_Firefoam; TrapIED_Firefoam:model | `1.7/Models/ied_firefoam.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ied_firefoam.obj` | Clay delivery, selected runtime geometry |
| Core:TrapIED_HighExplosive; TrapIED_HighExplosive:model | `1.7/Models/ied_highexplosive.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ied_highexplosive.obj` | Clay delivery, selected runtime geometry |
| Core:TrapIED_Incendiary; TrapIED_Incendiary:model | `1.7/Models/ied_incendiary.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ied_incendiary.obj` | Clay delivery, selected runtime geometry |
| Core:TrapIED_Smoke; TrapIED_Smoke:model | `1.7/Models/ied_smoke.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ied_smoke.obj` | Clay delivery, selected runtime geometry |
| Biotech:TrapIED_ToxGas; TrapIED_ToxGas:model | `1.7/Models/ied_toxgas.obj` | Z04 / `Models/ied_toxgas.obj` | Clay delivery with retained geometry |
| Core:MeleeWeapon_Ikwa; MeleeWeapon_Ikwa:model | `1.7/Models/ikwa.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/ikwa.obj` | Clay delivery, selected runtime geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_fat_barbs.obj` | Z03 / `Models/implants_fat_barbs.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_fat_plating.obj` | Z03 / `Models/implants_fat_plating.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_female_barbs.obj` | Z03 / `Models/implants_female_barbs.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_female_plating.obj` | Z03 / `Models/implants_female_plating.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_hulk_barbs.obj` | Z03 / `Models/implants_hulk_barbs.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_hulk_plating.obj` | Z03 / `Models/implants_hulk_plating.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_male_barbs.obj` | Z03 / `Models/implants_male_barbs.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_male_plating.obj` | Z03 / `Models/implants_male_plating.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_thin_barbs.obj` | Z03 / `Models/implants_thin_barbs.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/implants_thin_plating.obj` | Z03 / `Models/implants_thin_plating.obj` | Clay delivery with retained geometry |
| Anomaly:BiomutationPulser; Anomaly:ShardAnimalPulser; Artifacts:model; Core:PsychicAnimalPulser; Core:PsychicSoothePulser | `1.7/Models/item_artifact.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/item_artifact.obj` | Clay delivery, selected runtime geometry |
| Anomaly:RevenantVertebrae; Biotech:DetoxifierKidney; Biotech:DetoxifierLung; BodyPartsArchotech:model; BodyPartsBionic:model; BodyPartsProsthetic:model; BodyPartsUltra:model; Core:ArchotechArm; Core:ArchotechEye; Core:ArchotechLeg; Core:BionicArm; Core:BionicEar; Core:BionicEye; Core:BionicHeart; Core:BionicJaw; Core:BionicLeg; Core:BionicSpine; Core:BionicStomach; Core:BionicTongue; Core:CochlearImplant; Core:DeathAcidifier; Core:Joywire; Core:Painstopper; Core:PowerClaw; Core:PsychicAmplifier; Core:SimpleProstheticArm; Core:SimpleProstheticHeart; Core:SimpleProstheticLeg; Odyssey:PilotAssistant; Odyssey:VacskinGland; Royalty:AestheticNose; Royalty:AestheticShaper; Royalty:ArmorskinGland; Royalty:CircadianAssistant; Royalty:CircadianHalfCycler; Royalty:Coagulator; Royalty:DetoxifierStomach; Royalty:DrillArm; Royalty:ElbowBlade; Royalty:FieldHand; Royalty:GastroAnalyzer; Royalty:HandTalon; Royalty:HealingEnhancer; Royalty:Immunoenhancer; Royalty:KneeSpike; Royalty:LearningAssistant; Royalty:LoveEnhancer; Royalty:Mindscrew; Royalty:Neurocalculator; Royalty:NuclearStomach; Royalty:PsychicHarmonizer; Royalty:PsychicReader; Royalty:PsychicSensitizer; Royalty:ReprocessorStomach; Royalty:StoneskinGland; Royalty:ToughskinGland; Royalty:VenomFangs; Royalty:VenomTalon | `1.7/Models/item_bionic.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/item_bionic.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineHerbal; MedicineHerbal:li | `1.7/Models/item_medicine_herbal_a.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/item_medicine_herbal_a.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineHerbal; MedicineHerbal:li | `1.7/Models/item_medicine_herbal_b.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/item_medicine_herbal_b.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineHerbal; MedicineHerbal:li | `1.7/Models/item_medicine_herbal_c.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/item_medicine_herbal_c.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineIndustrial; MedicineIndustrial:li | `1.7/Models/item_medicine_industrial_a.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_medicine_industrial_a.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineIndustrial; MedicineIndustrial:li | `1.7/Models/item_medicine_industrial_b.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_medicine_industrial_b.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineIndustrial; MedicineIndustrial:li | `1.7/Models/item_medicine_industrial_c.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_medicine_industrial_c.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineUltratech; MedicineUltratech:li | `1.7/Models/item_medicine_ultratech_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_medicine_ultratech_a.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineUltratech; MedicineUltratech:li | `1.7/Models/item_medicine_ultratech_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_medicine_ultratech_b.obj` | Clay delivery, selected runtime geometry |
| Core:MedicineUltratech; MedicineUltratech:li | `1.7/Models/item_medicine_ultratech_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_medicine_ultratech_c.obj` | Clay delivery, selected runtime geometry |
| BodyPartsNatural:model; Core:Heart; Core:Kidney; Core:Liver; Core:Lung | `1.7/Models/item_organ.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/item_organ.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Bioferrite; Bioferrite:li | `1.7/Models/item_rm_bioferrite_a.obj` | Z03 / `Models/item_rm_bioferrite_a.obj` | Clay delivery with retained geometry |
| Anomaly:Bioferrite; Bioferrite:li | `1.7/Models/item_rm_bioferrite_b.obj` | Z03 / `Models/item_rm_bioferrite_b.obj` | Clay delivery with retained geometry |
| Anomaly:Bioferrite; Bioferrite:li | `1.7/Models/item_rm_bioferrite_c.obj` | Z03 / `Models/item_rm_bioferrite_c.obj` | Clay delivery with retained geometry |
| Chemfuel:li; Core:Chemfuel | `1.7/Models/item_rm_chemfuel_a.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/item_rm_chemfuel_a.obj` | Clay delivery, selected runtime geometry |
| ComponentIndustrial:li; Core:ComponentIndustrial | `1.7/Models/item_rm_component_a.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/item_rm_component_a.obj` | Clay delivery, selected runtime geometry |
| ComponentSpacer:li; Core:ComponentSpacer | `1.7/Models/item_rm_component_spacer_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_component_spacer_a.obj` | Clay delivery, selected runtime geometry |
| Core:Gold; Gold:li | `1.7/Models/item_rm_gold_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_gold_a.obj` | Clay delivery, selected runtime geometry |
| Core:Gold; Gold:li | `1.7/Models/item_rm_gold_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_gold_b.obj` | Clay delivery, selected runtime geometry |
| Core:Gold; Gold:li | `1.7/Models/item_rm_gold_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_gold_c.obj` | Clay delivery, selected runtime geometry |
| BlocksGranite:li; BlocksLimestone:li; BlocksSandstone:li; BlocksSlate:li; Core:BlocksGranite; Core:BlocksLimestone; Core:BlocksSandstone; Core:BlocksSlate | `1.7/Models/item_rm_granite_blocks_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_slate_blocks_a.obj` | Clay delivery, selected runtime geometry |
| BlocksGranite:li; BlocksLimestone:li; BlocksSandstone:li; BlocksSlate:li; Core:BlocksGranite; Core:BlocksLimestone; Core:BlocksSandstone; Core:BlocksSlate | `1.7/Models/item_rm_granite_blocks_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_slate_blocks_b.obj` | Clay delivery, selected runtime geometry |
| BlocksGranite:li; BlocksLimestone:li; BlocksSandstone:li; BlocksSlate:li; Core:BlocksGranite; Core:BlocksLimestone; Core:BlocksSandstone; Core:BlocksSlate | `1.7/Models/item_rm_granite_blocks_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_slate_blocks_c.obj` | Clay delivery, selected runtime geometry |
| GravlitePanel:li; Odyssey:GravlitePanel | `1.7/Models/item_rm_gravlite_a.obj` | Z05 / `Models/item_rm_gravlite_a.obj` | Clay delivery with retained geometry |
| GravlitePanel:li; Odyssey:GravlitePanel | `1.7/Models/item_rm_gravlite_b.obj` | Z05 / `Models/item_rm_gravlite_b.obj` | Clay delivery with retained geometry |
| GravlitePanel:li; Odyssey:GravlitePanel | `1.7/Models/item_rm_gravlite_c.obj` | Z05 / `Models/item_rm_gravlite_c.obj` | Clay delivery with retained geometry |
| Core:Jade; Jade:li | `1.7/Models/item_rm_jade_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_jade_a.obj` | Clay delivery, selected runtime geometry |
| Core:Jade; Jade:li | `1.7/Models/item_rm_jade_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_jade_b.obj` | Clay delivery, selected runtime geometry |
| Core:Jade; Jade:li | `1.7/Models/item_rm_jade_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_jade_c.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Leather_Dread; Core:Leather_Bear; Core:Leather_Bird; Core:Leather_Bluefur; Core:Leather_Camel; Core:Leather_Chinchilla; Core:Leather_Dog; Core:Leather_Elephant; Core:Leather_Fox; Core:Leather_GuineaPig; Core:Leather_Heavy; Core:Leather_Human; Core:Leather_Light; Core:Leather_Lizard; Core:Leather_Panthera; Core:Leather_Patch; Core:Leather_Pig; Core:Leather_Plain; Core:Leather_Rhinoceros; Core:Leather_Thrumbo; Core:Leather_Wolf; Leathers:li; Odyssey:Leather_AlphaThrumbo; Odyssey:Leather_Armadillo; Odyssey:Leather_Hippo; Odyssey:Leather_Mastodon; Odyssey:Leather_Mink; Odyssey:Leather_Sealskin | `1.7/Models/item_rm_leather_a.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/item_rm_leather_a.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Leather_Dread; Core:Leather_Bear; Core:Leather_Bird; Core:Leather_Bluefur; Core:Leather_Camel; Core:Leather_Chinchilla; Core:Leather_Dog; Core:Leather_Elephant; Core:Leather_Fox; Core:Leather_GuineaPig; Core:Leather_Heavy; Core:Leather_Human; Core:Leather_Light; Core:Leather_Lizard; Core:Leather_Panthera; Core:Leather_Patch; Core:Leather_Pig; Core:Leather_Plain; Core:Leather_Rhinoceros; Core:Leather_Thrumbo; Core:Leather_Wolf; Leathers:li; Odyssey:Leather_AlphaThrumbo; Odyssey:Leather_Armadillo; Odyssey:Leather_Hippo; Odyssey:Leather_Mastodon; Odyssey:Leather_Mink; Odyssey:Leather_Sealskin | `1.7/Models/item_rm_leather_b.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/item_rm_leather_b.obj` | Clay delivery, selected runtime geometry |
| Core:Neutroamine; Neutroamine:li | `1.7/Models/item_rm_neutroamine_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_neutroamine_a.obj` | Clay delivery, selected runtime geometry |
| Core:Neutroamine; Neutroamine:li | `1.7/Models/item_rm_neutroamine_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_neutroamine_b.obj` | Clay delivery, selected runtime geometry |
| Core:Neutroamine; Neutroamine:li | `1.7/Models/item_rm_neutroamine_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_neutroamine_c.obj` | Clay delivery, selected runtime geometry |
| Obsidian:li; Odyssey:Obsidian | `1.7/Models/item_rm_obsidian_a.obj` | Z05 / `Models/item_rm_obsidian_a.obj` | Clay delivery with retained geometry |
| Obsidian:li; Odyssey:Obsidian | `1.7/Models/item_rm_obsidian_b.obj` | Z05 / `Models/item_rm_obsidian_b.obj` | Clay delivery with retained geometry |
| Obsidian:li; Odyssey:Obsidian | `1.7/Models/item_rm_obsidian_c.obj` | Z05 / `Models/item_rm_obsidian_c.obj` | Clay delivery with retained geometry |
| Core:Plasteel; Plasteel:li | `1.7/Models/item_rm_plasteel_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_plasteel_a.obj` | Clay delivery, selected runtime geometry |
| Core:Plasteel; Plasteel:li | `1.7/Models/item_rm_plasteel_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_plasteel_b.obj` | Clay delivery, selected runtime geometry |
| Core:Plasteel; Plasteel:li | `1.7/Models/item_rm_plasteel_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_plasteel_c.obj` | Clay delivery, selected runtime geometry |
| Core:Silver; Silver:li | `1.7/Models/item_rm_silver_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_silver_a.obj` | Clay delivery, selected runtime geometry |
| Core:Silver; Silver:li | `1.7/Models/item_rm_silver_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_silver_b.obj` | Clay delivery, selected runtime geometry |
| Core:Silver; Silver:li | `1.7/Models/item_rm_silver_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_silver_c.obj` | Clay delivery, selected runtime geometry |
| Core:Steel; Steel:li | `1.7/Models/item_rm_steel_a.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/item_rm_steel_a.obj` | Clay delivery, selected runtime geometry |
| Core:Steel; Steel:li | `1.7/Models/item_rm_steel_b.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/item_rm_steel_b.obj` | Clay delivery, selected runtime geometry |
| Core:Steel; Steel:li | `1.7/Models/item_rm_steel_c.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/item_rm_steel_c.obj` | Clay delivery, selected runtime geometry |
| Cloth:li; Core:Cloth; Core:DevilstrandCloth; Core:Hyperweave; Core:Synthread; DevilstrandCloth:li; Hyperweave:li; Synthread:li | `1.7/Models/item_rm_textile_a.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_textile_a.obj` | Clay delivery, selected runtime geometry |
| Cloth:li; Core:Cloth; Core:DevilstrandCloth; Core:Hyperweave; Core:Synthread; DevilstrandCloth:li; Hyperweave:li; Synthread:li | `1.7/Models/item_rm_textile_b.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_textile_b.obj` | Clay delivery, selected runtime geometry |
| Cloth:li; Core:Cloth; Core:DevilstrandCloth; Core:Hyperweave; Core:Synthread; DevilstrandCloth:li; Hyperweave:li; Synthread:li | `1.7/Models/item_rm_textile_c.obj` | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/item_rm_textile_c.obj` | Clay delivery, selected runtime geometry |
| Core:Uranium; Uranium:li | `1.7/Models/item_rm_uranium_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_uranium_a.obj` | Clay delivery, selected runtime geometry |
| Core:Uranium; Uranium:li | `1.7/Models/item_rm_uranium_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_uranium_b.obj` | Clay delivery, selected runtime geometry |
| Core:Uranium; Uranium:li | `1.7/Models/item_rm_uranium_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/item_rm_uranium_c.obj` | Clay delivery, selected runtime geometry |
| Core:WoolAlpaca; Core:WoolBison; Core:WoolMegasloth; Core:WoolMuffalo; Core:WoolSheep; Odyssey:WoolMastodon; Odyssey:WoolMuskox; Wools:li | `1.7/Models/item_rm_wool_a.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/item_rm_wool_a.obj` | Clay delivery, selected runtime geometry |
| Building_KidOutfitStand:model; Odyssey:Building_KidOutfitStand | `1.7/Models/kid_outfit_stand.obj` | Z05 / `Models/kid_outfit_stand.obj` | Clay delivery with retained geometry |
| Building_KidOutfitStand:headModel; Odyssey:Building_KidOutfitStand | `1.7/Models/kid_outfit_stand_head.obj` | Z05 / `Models/kid_outfit_stand_head.obj` | Clay delivery with retained geometry |
| Core:MeleeWeapon_Knife; MeleeWeapon_Knife:model | `1.7/Models/knife.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/knife.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer_arm_left.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer_arm_left.obj` | Clay delivery with retained geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer_arm_right.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer_arm_right.obj` | Clay delivery with retained geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer_head.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer_head.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer_weapon.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer_weapon.obj` | Clay delivery with retained geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer_wheel_left.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer_wheel_left.obj` | Clay delivery with retained geometry |
| Core:Mech_Lancer; Mech_Lancer:model | `1.7/Models/lancer_wheel_right.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/lancer_wheel_right.obj` | Clay delivery with retained geometry |
| Royalty:ShipLandingBeacon; ShipLandingBeacon:model | `1.7/Models/landing_beacon.obj` | Z18 / `Models/landing_beacon.obj` | Clay delivery with retained geometry |
| LargeChemfuelTank:model; Odyssey:LargeChemfuelTank | `1.7/Models/large_chemfuel_tank.obj` | Z05 / `Models/large_chemfuel_tank.obj` | Clay delivery with retained geometry |
| Biotech:LargeMechGestator; LargeMechGestator:model | `1.7/Models/large_mech_gestator.obj` | Z04 / `Models/large_mech_gestator.obj` | Clay delivery with retained geometry |
| Biotech:LargeMechGestator; LargeMechGestator:glassModel | `1.7/Models/large_mech_gestator_glass.obj` | Z04 / `Models/large_mech_gestator_glass.obj` | Clay delivery with retained geometry |
| LargeThruster:model; Odyssey:LargeThruster | `1.7/Models/large_thruster.obj` | Z05 / `Models/large_thruster.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_EmpLauncher; Core:Gun_IncendiaryLauncher; Core:Gun_SmokeLauncher; Gun_EmpLauncher:model; Gun_IncendiaryLauncher:model; Gun_SmokeLauncher:model | `1.7/Models/launcher.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/launcher.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/leathery_female_face.obj` | Z03 / `Models/leathery_female_face.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/leathery_male_face.obj` | Z03 / `Models/leathery_male_face.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Legionary; Mech_Legionary:model | `1.7/Models/legionary.obj` | Z04 / `Models/legionary.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Legionary; Mech_Legionary:model | `1.7/Models/legionary_leg_left.obj` | Z04 / `Models/legionary_leg_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Legionary; Mech_Legionary:model | `1.7/Models/legionary_leg_right.obj` | Z04 / `Models/legionary_leg_right.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Legionary; Mech_Legionary:model | `1.7/Models/legionary_upper.obj` | Z04 / `Models/legionary_upper.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Legionary; Mech_Legionary:model | `1.7/Models/legionary_weapon.obj` | Z04 / `Models/legionary_weapon.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Lifter; Mech_Lifter:model | `1.7/Models/lifter.obj` | Z04 / `Models/lifter.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Lifter; Mech_Lifter:model | `1.7/Models/lifter_forks.obj` | Z04 / `Models/lifter_forks.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Lifter; Mech_Lifter:model | `1.7/Models/lifter_wheel.obj` | Z04 / `Models/lifter_wheel.obj` | Clay delivery with retained geometry |
| Core:Gun_LMG; Gun_LMG:model | `1.7/Models/lmg.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/lmg.obj` | Clay delivery, selected runtime geometry |
| Core:MeleeWeapon_LongSword; MeleeWeapon_LongSword:model | `1.7/Models/longsword.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/longsword.obj` | Clay delivery, selected runtime geometry |
| Core:MeleeWeapon_Mace; MeleeWeapon_Mace:model | `1.7/Models/mace.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/mace.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_MachinePistol; Gun_MachinePistol:model | `1.7/Models/machine_pistol.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/machine_pistol.obj` | Clay delivery, selected runtime geometry |
| Core:MealFine; Core:MealFine_Meat; Core:MealFine_Veg; MealFine:model; MealFine_Meat:model; MealFine_Veg:model | `1.7/Models/meal_fine.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/meal_fine.obj` | Clay delivery, selected runtime geometry |
| Core:Kibble; Kibble:model | `1.7/Models/meal_kibble.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/meal_kibble.obj` | Clay delivery, selected runtime geometry |
| Core:MealLavish; Core:MealLavish_Meat; Core:MealLavish_Veg; MealLavish:model; MealLavish_Meat:model; MealLavish_Veg:model | `1.7/Models/meal_lavish.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/meal_lavish.obj` | Clay delivery, selected runtime geometry |
| Core:MealNutrientPaste; MealNutrientPaste:model | `1.7/Models/meal_paste.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/meal_paste.obj` | Clay delivery with retained geometry |
| Core:Pemmican; Pemmican:model | `1.7/Models/meal_pemmican.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/meal_pemmican.obj` | Clay delivery, selected runtime geometry |
| Core:MealSimple; FoodMeals:model; MealSimple:model | `1.7/Models/meal_simple.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/meal_simple.obj` | Clay delivery, selected runtime geometry |
| Core:MealSurvivalPack; MealSurvivalPack:model | `1.7/Models/meal_survival.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/meal_survival.obj` | Clay delivery, selected runtime geometry |
| MechAssembler:model; Royalty:MechAssembler | `1.7/Models/mech_assembler.obj` | Z18 / `Models/mech_assembler.obj` | Clay delivery, selected runtime geometry |
| Biotech:MechBooster; MechBooster:model | `1.7/Models/mech_booster.obj` | Z04 / `Models/mech_booster.obj` | Clay delivery with retained geometry |
| MechCapsule:model; Royalty:MechCapsule | `1.7/Models/mech_capsule_closed.obj` | Z18 / `Models/mech_capsule_closed.obj` | Clay delivery, selected runtime geometry |
| MechCapsule:emptyModel; Royalty:MechCapsule | `1.7/Models/mech_capsule_open.obj` | Z18 / `Models/mech_capsule_open.obj` | Clay delivery, selected runtime geometry |
| Biotech:MechGestator; MechGestator:model | `1.7/Models/mech_gestator.obj` | Z04 / `Models/mech_gestator.obj` | Clay delivery with retained geometry |
| Biotech:MechGestator; MechGestator:glassModel | `1.7/Models/mech_gestator_glass.obj` | Z04 / `Models/mech_gestator_glass.obj` | Clay delivery with retained geometry |
| Royalty:ShieldGeneratorMortar; ShieldGeneratorMortar:model | `1.7/Models/mech_high_shield.obj` | Z18 / `Models/mech_high_shield.obj` | Clay delivery, selected runtime geometry |
| Royalty:ShieldGeneratorBullets; ShieldGeneratorBullets:model | `1.7/Models/mech_low_shield.obj` | Z18 / `Models/mech_low_shield.obj` | Clay delivery, selected runtime geometry |
| Biotech:BurnoutMechlinkBooster; BurnoutMechlinkBooster:model | `1.7/Models/mechband_antenna.obj` | Z04 / `Models/mechband_antenna.obj` | Clay delivery with retained geometry |
| Biotech:MechbandDish; MechbandDish:model | `1.7/Models/mechband_dish.obj` | Z04 / `Models/mechband_dish.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_blade_-1_0.obj` | Z03 / `Models/metalhorror_juvenile_blade_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_blade_-1_1.obj` | Z03 / `Models/metalhorror_juvenile_blade_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_blade_-1_2.obj` | Z03 / `Models/metalhorror_juvenile_blade_-1_2.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_blade_1_0.obj` | Z03 / `Models/metalhorror_juvenile_blade_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_blade_1_1.obj` | Z03 / `Models/metalhorror_juvenile_blade_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_blade_1_2.obj` | Z03 / `Models/metalhorror_juvenile_blade_1_2.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_body.obj` | Z03 / `Models/metalhorror_juvenile_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_head.obj` | Z03 / `Models/metalhorror_juvenile_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_support_-1.obj` | Z03 / `Models/metalhorror_juvenile_support_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_juvenile_support_1.obj` | Z03 / `Models/metalhorror_juvenile_support_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_blade_-1_0.obj` | Z03 / `Models/metalhorror_larva_blade_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_blade_-1_1.obj` | Z03 / `Models/metalhorror_larva_blade_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_blade_1_0.obj` | Z03 / `Models/metalhorror_larva_blade_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_blade_1_1.obj` | Z03 / `Models/metalhorror_larva_blade_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_body.obj` | Z03 / `Models/metalhorror_larva_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_head.obj` | Z03 / `Models/metalhorror_larva_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_support_-1.obj` | Z03 / `Models/metalhorror_larva_support_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_larva_support_1.obj` | Z03 / `Models/metalhorror_larva_support_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_blade_-1_0.obj` | Z03 / `Models/metalhorror_mature_blade_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_blade_-1_1.obj` | Z03 / `Models/metalhorror_mature_blade_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_blade_-1_2.obj` | Z03 / `Models/metalhorror_mature_blade_-1_2.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_blade_1_0.obj` | Z03 / `Models/metalhorror_mature_blade_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_blade_1_1.obj` | Z03 / `Models/metalhorror_mature_blade_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_blade_1_2.obj` | Z03 / `Models/metalhorror_mature_blade_1_2.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_body.obj` | Z03 / `Models/metalhorror_mature_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_head.obj` | Z03 / `Models/metalhorror_mature_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_support_-1.obj` | Z03 / `Models/metalhorror_mature_support_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Metalhorror; Metalhorror:model | `1.7/Models/metalhorror_mature_support_1.obj` | Z03 / `Models/metalhorror_mature_support_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Militor; Mech_Militor:model | `1.7/Models/militor.obj` | Z04 / `Models/militor.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Militor; Mech_Militor:model | `1.7/Models/militor_upper.obj` | Z04 / `Models/militor_upper.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Militor; Mech_Militor:model | `1.7/Models/militor_weapon.obj` | Z04 / `Models/militor_weapon.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Militor; Mech_Militor:model | `1.7/Models/militor_wheel.obj` | Z04 / `Models/militor_wheel.obj` | Clay delivery with retained geometry |
| Core:LongRangeMineralScanner; LongRangeMineralScanner:model | `1.7/Models/mineral_scanner.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/mineral_scanner.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_Minigun; Gun_Minigun:model | `1.7/Models/minigun.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/minigun.obj` | Clay delivery, selected runtime geometry |
| Core:MoisturePump; MoisturePump:model | `1.7/Models/moisture_pump.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/moisture_pump.obj` | Clay delivery, selected runtime geometry |
| Core:Weapon_GrenadeMolotov; Weapon_GrenadeMolotov:model | `1.7/Models/molotov.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/molotov.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Moss; Plant_Moss:li | `1.7/Models/moss_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/moss_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Moss; Plant_Moss:li | `1.7/Models/moss_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/moss_2.obj` | Clay delivery, selected runtime geometry |
| Core:MultiAnalyzer; MultiAnalyzer:model | `1.7/Models/multi_analyzer.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/multi_analyzer.obj` | Clay delivery, selected runtime geometry |
| NatureShrine_Large:model; Royalty:NatureShrine_Large | `1.7/Models/nature_shrine_large_smooth.obj` | Z18 / `Models/nature_shrine_large_smooth.obj` | Clay delivery with retained geometry |
| NatureShrine_Large:model; Royalty:NatureShrine_Large | `1.7/Models/nature_shrine_large_wood.obj` | Z18 / `Models/nature_shrine_large_wood.obj` | Clay delivery with retained geometry |
| NatureShrine_Small:model; Royalty:NatureShrine_Small | `1.7/Models/nature_shrine_small_smooth.obj` | Z18 / `Models/nature_shrine_small_smooth.obj` | Clay delivery with retained geometry |
| NatureShrine_Small:model; Royalty:NatureShrine_Small | `1.7/Models/nature_shrine_small_wood.obj` | Z18 / `Models/nature_shrine_small_wood.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_closed_lining.obj` | Z03 / `Models/nociosphere_closed_lining.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_core.obj` | Z03 / `Models/nociosphere_core.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_A.obj` | Z03 / `Models/nociosphere_shell_A.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_B.obj` | Z03 / `Models/nociosphere_shell_B.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_C.obj` | Z03 / `Models/nociosphere_shell_C.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_D.obj` | Z03 / `Models/nociosphere_shell_D.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_E.obj` | Z03 / `Models/nociosphere_shell_E.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_F.obj` | Z03 / `Models/nociosphere_shell_F.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_G.obj` | Z03 / `Models/nociosphere_shell_G.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_H.obj` | Z03 / `Models/nociosphere_shell_H.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_I.obj` | Z03 / `Models/nociosphere_shell_I.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_J.obj` | Z03 / `Models/nociosphere_shell_J.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_K.obj` | Z03 / `Models/nociosphere_shell_K.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_L.obj` | Z03 / `Models/nociosphere_shell_L.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_M.obj` | Z03 / `Models/nociosphere_shell_M.obj` | Clay delivery with retained geometry |
| Anomaly:Nociosphere; Nociosphere:model | `1.7/Models/nociosphere_shell_N.obj` | Z03 / `Models/nociosphere_shell_N.obj` | Clay delivery with retained geometry |
| Anomaly:Noctol; Noctol:model | `1.7/Models/noctol_body.obj` | Z03 / `Models/noctol_body.obj` | Clay delivery with retained geometry |
| Anomaly:Noctol; Noctol:model | `1.7/Models/noctol_head.obj` | Z03 / `Models/noctol_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Noctol; Noctol:model | `1.7/Models/noctol_leg_-1_0.obj` | Z03 / `Models/noctol_leg_-1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Noctol; Noctol:model | `1.7/Models/noctol_leg_-1_1.obj` | Z03 / `Models/noctol_leg_-1_1.obj` | Clay delivery with retained geometry |
| Anomaly:Noctol; Noctol:model | `1.7/Models/noctol_leg_1_0.obj` | Z03 / `Models/noctol_leg_1_0.obj` | Clay delivery with retained geometry |
| Anomaly:Noctol; Noctol:model | `1.7/Models/noctol_leg_1_1.obj` | Z03 / `Models/noctol_leg_1_1.obj` | Clay delivery with retained geometry |
| Anomaly:FleshmassNucleus; FleshmassNucleus:model | `1.7/Models/nucleus_a_assembled.obj` | Z03 / `Models/nucleus_a_assembled.obj` | Clay delivery with retained geometry |
| Anomaly:FleshmassNucleus; FleshmassNucleus:model | `1.7/Models/nucleus_b_assembled.obj` | Z03 / `Models/nucleus_b_assembled.obj` | Clay delivery with retained geometry |
| Anomaly:FleshmassNucleus; FleshmassNucleus:model | `1.7/Models/nucleus_c_assembled.obj` | Z03 / `Models/nucleus_c_assembled.obj` | Clay delivery with retained geometry |
| Anomaly:FleshmassNucleus; FleshmassNucleus:model | `1.7/Models/nucleus_d_assembled.obj` | Z03 / `Models/nucleus_d_assembled.obj` | Clay delivery with retained geometry |
| Anomaly:FleshmassNucleus; FleshmassNucleus:model | `1.7/Models/nucleus_e_assembled.obj` | Z03 / `Models/nucleus_e_assembled.obj` | Clay delivery with retained geometry |
| Anomaly:FleshmassNucleus; FleshmassNucleus:model | `1.7/Models/nucleus_f_assembled.obj` | Z03 / `Models/nucleus_f_assembled.obj` | Clay delivery with retained geometry |
| Odyssey:OrbitalScanner; OrbitalScanner:model | `1.7/Models/orbital_scanner.obj` | Z05 / `Models/orbital_scanner.obj` | Clay delivery, selected runtime geometry |
| Core:OrnateDoor; OrnateDoor:model | `1.7/Models/ornate_door_frame.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ornate_door_frame.obj` | Clay delivery, selected runtime geometry |
| Core:OrnateDoor; OrnateDoor:doorLeafModel | `1.7/Models/ornate_door_leaf.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ornate_door_leaf.obj` | Clay delivery, selected runtime geometry |
| Core:OrnateDoor; OrnateDoor:doorTopModel | `1.7/Models/ornate_door_top.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/ornate_door_top.obj` | Clay delivery with retained geometry |
| Building_OutfitStand:model; Odyssey:Building_OutfitStand | `1.7/Models/outfit_stand.obj` | Z05 / `Models/outfit_stand.obj` | Clay delivery with retained geometry |
| Building_OutfitStand:headModel; Odyssey:Building_OutfitStand | `1.7/Models/outfit_stand_head.obj` | Z05 / `Models/outfit_stand_head.obj` | Clay delivery with retained geometry |
| Odyssey:OxygenPump; OxygenPump:model | `1.7/Models/oxygen_pump.obj` | Z05 / `Models/oxygen_pump.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Apparel_BiomutationLance; Anomaly:Apparel_DeadlifePack; Anomaly:Apparel_DisruptorFlarePack; Anomaly:Apparel_PackTurret; Anomaly:Apparel_ShardPsychicInsanityLance; Anomaly:Apparel_ShardPsychicShockLance; ApparelUtility:model; Biotech:Apparel_PackBandwidth; Biotech:Apparel_PackControl; Biotech:Apparel_PackTox; Core:Apparel_FirefoampopPack; Core:Apparel_PsychicInsanityLance; Core:Apparel_PsychicShockLance; Core:Apparel_ShieldBelt; Core:Apparel_SmokepopBelt; Core:OrbitalTargeterBombardment; Core:OrbitalTargeterPowerBeam; Core:TornadoGenerator; Odyssey:Apparel_CerebrexNode; Odyssey:Apparel_PackHunter; Royalty:Apparel_PackBroadshield; Royalty:Apparel_PackJump; Royalty:OrbitalTargeterMechCluster; Source17 internal part (literal filename) | `1.7/Models/pack.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pack.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Paramedic; Mech_Paramedic:model | `1.7/Models/paramedic.obj` | Z04 / `Models/paramedic.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Paramedic; Mech_Paramedic:model | `1.7/Models/paramedic_head.obj` | Z04 / `Models/paramedic_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Paramedic; Mech_Paramedic:model | `1.7/Models/paramedic_pack.obj` | Z04 / `Models/paramedic_pack.obj` | Clay delivery with retained geometry |
| Core:PassiveCooler; PassiveCooler:model | `1.7/Models/passive_cooler.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/passive_cooler.obj` | Clay delivery, selected runtime geometry |
| Core:NutrientPasteDispenser; NutrientPasteDispenser:model | `1.7/Models/paste_dispenser.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/paste_dispenser.obj` | Clay delivery, selected runtime geometry |
| Core:PenMarker; PenMarker:model | `1.7/Models/pen_marker.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/pen_marker.obj` | Clay delivery, selected runtime geometry |
| HairParts.Beard: shared beard shape | `1.7/Models/person_beard_full.obj` | Z02 / `Models/person_beard_full.obj` | Clay delivery, selected runtime geometry |
| HairParts.Beard: shared beard shape | `1.7/Models/person_beard_goatee.obj` | Z02 / `Models/person_beard_goatee.obj` | Clay delivery, selected runtime geometry |
| Apparel_ShieldBelt:model | `1.7/Models/person_belt_shield.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_belt_shield.obj` | Clay delivery, selected runtime geometry |
| Apparel_SmokepopBelt:model | `1.7/Models/person_belt_smoke.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_belt_smoke.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_cataphract | `1.7/Models/person_fat_armor_cataphract.obj` | Z18 / `Models/person_fat_armor_cataphract.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_flakjacket | `1.7/Models/person_fat_armor_flakjacket.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_fat_armor_flakjacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_flakpants | `1.7/Models/person_fat_armor_flakpants.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_fat_armor_flakpants.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_flakvest | `1.7/Models/person_fat_armor_flakvest.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_fat_armor_flakvest.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_flakvest_front | `1.7/Models/person_fat_armor_flakvest_front.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_fat_armor_flakvest_front.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_marine | `1.7/Models/person_fat_armor_marine.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_fat_armor_marine.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_plate | `1.7/Models/person_fat_armor_plate.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_fat_armor_plate.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, armor_recon | `1.7/Models/person_fat_armor_recon.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_fat_armor_recon.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, bandolier | `1.7/Models/person_fat_bandolier.obj` | Z04 / `Models/person_fat_bandolier.obj` | Clay delivery with retained geometry |
| People.Work: fat body fit, bandolier_overvest | `1.7/Models/person_fat_bandolier_overvest.obj` | Z04 / `Models/person_fat_bandolier_overvest.obj` | Clay delivery with retained geometry |
| People.Work: fat body fit, coat_long | `1.7/Models/person_fat_coat_long.obj` | Z02 / `Models/person_fat_coat_long.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, coat_short | `1.7/Models/person_fat_coat_short.obj` | Z02 / `Models/person_fat_coat_short.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, duster | `1.7/Models/person_fat_duster.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_fat_duster.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, jacket | `1.7/Models/person_fat_jacket.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_fat_jacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, labcoat | `1.7/Models/person_fat_labcoat.obj` | Z03 / `Models/person_fat_labcoat.obj` | Clay delivery with retained geometry |
| People.Work: fat body fit, locust | `1.7/Models/person_fat_locust.obj` | Z18 / `Models/person_fat_locust.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, lower | `1.7/Models/person_fat_lower.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_fat_lower.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, mechlord | `1.7/Models/person_fat_mechlord.obj` | Z04 / `Models/person_fat_mechlord.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, parka | `1.7/Models/person_fat_parka.obj` | Z04 / `Models/person_fat_parka.obj` | Clay delivery, prior approved export reused |
| People.Work: fat body fit, robe | `1.7/Models/person_fat_robe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_fat_robe.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, robe_psy | `1.7/Models/person_fat_robe_psy.obj` | Z18 / `Models/person_fat_robe_psy.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, robe_royal | `1.7/Models/person_fat_robe_royal.obj` | Z18 / `Models/person_fat_robe_royal.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, upper | `1.7/Models/person_fat_upper.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_fat_upper.obj` | Clay delivery, selected runtime geometry |
| People.Work: fat body fit, vacsuit | `1.7/Models/person_fat_vacsuit.obj` | Z05 / `Models/person_fat_vacsuit.obj` | Clay delivery with retained geometry |
| People.Work: female body fit, armor_cataphract | `1.7/Models/person_female_armor_cataphract.obj` | Z18 / `Models/person_female_armor_cataphract.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_flakjacket | `1.7/Models/person_female_armor_flakjacket.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_female_armor_flakjacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_flakpants | `1.7/Models/person_female_armor_flakpants.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_female_armor_flakpants.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_flakvest | `1.7/Models/person_female_armor_flakvest.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_female_armor_flakvest.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_flakvest_front | `1.7/Models/person_female_armor_flakvest_front.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_female_armor_flakvest_front.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_marine | `1.7/Models/person_female_armor_marine.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_female_armor_marine.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_plate | `1.7/Models/person_female_armor_plate.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_female_armor_plate.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, armor_recon | `1.7/Models/person_female_armor_recon.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_female_armor_recon.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, bandolier | `1.7/Models/person_female_bandolier.obj` | Z04 / `Models/person_female_bandolier.obj` | Clay delivery with retained geometry |
| People.Work: female body fit, bandolier_overvest | `1.7/Models/person_female_bandolier_overvest.obj` | Z04 / `Models/person_female_bandolier_overvest.obj` | Clay delivery with retained geometry |
| People.Work: female body fit, coat_long | `1.7/Models/person_female_coat_long.obj` | Z02 / `Models/person_female_coat_long.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, coat_short | `1.7/Models/person_female_coat_short.obj` | Z02 / `Models/person_female_coat_short.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, duster | `1.7/Models/person_female_duster.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_female_duster.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, jacket | `1.7/Models/person_female_jacket.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_female_jacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, labcoat | `1.7/Models/person_female_labcoat.obj` | Z03 / `Models/person_female_labcoat.obj` | Clay delivery with retained geometry |
| People.Work: female body fit, locust | `1.7/Models/person_female_locust.obj` | Z18 / `Models/person_female_locust.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, lower | `1.7/Models/person_female_lower.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_female_lower.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, mechlord | `1.7/Models/person_female_mechlord.obj` | Z04 / `Models/person_female_mechlord.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, parka | `1.7/Models/person_female_parka.obj` | Z04 / `Models/person_female_parka.obj` | Clay delivery, prior approved export reused |
| People.Work: female body fit, robe | `1.7/Models/person_female_robe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_female_robe.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, robe_psy | `1.7/Models/person_female_robe_psy.obj` | Z18 / `Models/person_female_robe_psy.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, robe_royal | `1.7/Models/person_female_robe_royal.obj` | Z18 / `Models/person_female_robe_royal.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, upper | `1.7/Models/person_female_upper.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_female_upper.obj` | Clay delivery, selected runtime geometry |
| People.Work: female body fit, vacsuit | `1.7/Models/person_female_vacsuit.obj` | Z05 / `Models/person_female_vacsuit.obj` | Clay delivery with retained geometry |
| HairParts.Hair: Afro | `1.7/Models/person_hair_afro.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_afro.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: Curly, Artist, Mary | `1.7/Models/person_hair_bob.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_bob.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: sidebuns, topbun_tail, FancyBun, Anne, Elisabeth | `1.7/Models/person_hair_bun.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_bun.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: shoulder, Long, LongOily, LongMess, Princess, Wizard, LongDutch, Bushy, BushyStyled, Seer, Urist, BeardCurly, TriBraid, Fork, BeardFurskinMegabraid, BeardFurskinSenile | `1.7/Models/person_hair_long.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_long.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: bob, Bob, Messy, Parted, SideCut | `1.7/Models/person_hair_medium.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_medium.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: topknot, crest, Mohawk, ShavedMohawk | `1.7/Models/person_hair_mohawk.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_mohawk.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: twintails, BriefPonytail | `1.7/Models/person_hair_ponytail.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_ponytail.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: balding, William, Recruit, ShortCut, Topdog, Ghoul_Furious, Ghoul_Hungry, Ghoul_Ravenous, Ghoul_Twisted, Ghoul_Vengeful, Ghoul_Warped, Ghoul_Wicked | `1.7/Models/person_hair_short.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_short.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: Spikes, SideSpikes, LargeTuft | `1.7/Models/person_hair_spiky.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_spiky.obj` | Clay delivery, selected runtime geometry |
| HairDef:Bowlcut | `1.7/Models/person_hair_style_bowlcut.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_bowlcut.obj` | Clay delivery, selected runtime geometry |
| HairDef:BraidedKnot | `1.7/Models/person_hair_style_braidedknot.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_braidedknot.obj` | Clay delivery, selected runtime geometry |
| HairDef:Bravo | `1.7/Models/person_hair_style_bravo.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_bravo.obj` | Clay delivery, selected runtime geometry |
| HairDef:Burgundy | `1.7/Models/person_hair_style_burgundy.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_burgundy.obj` | Clay delivery, selected runtime geometry |
| HairDef:Cleopatra | `1.7/Models/person_hair_style_cleopatra.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_cleopatra.obj` | Clay delivery, selected runtime geometry |
| HairDef:Cute | `1.7/Models/person_hair_style_cute.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_cute.obj` | Clay delivery, selected runtime geometry |
| HairDef:Decent | `1.7/Models/person_hair_style_decent.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_decent.obj` | Clay delivery, selected runtime geometry |
| HairDef:Firestarter | `1.7/Models/person_hair_style_firestarter.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_firestarter.obj` | Clay delivery, selected runtime geometry |
| HairDef:Flowy | `1.7/Models/person_hair_style_flowy.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_flowy.obj` | Clay delivery, selected runtime geometry |
| HairDef:Fringe | `1.7/Models/person_hair_style_fringe.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_fringe.obj` | Clay delivery, selected runtime geometry |
| HairDef:Frozen | `1.7/Models/person_hair_style_frozen.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_frozen.obj` | Clay delivery, selected runtime geometry |
| HairDef:GreasySwoop | `1.7/Models/person_hair_style_greasyswoop.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_greasyswoop.obj` | Clay delivery, selected runtime geometry |
| HairDef:Junkie | `1.7/Models/person_hair_style_junkie.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_junkie.obj` | Clay delivery, selected runtime geometry |
| HairDef:Keeper | `1.7/Models/person_hair_style_keeper.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_keeper.obj` | Clay delivery, selected runtime geometry |
| HairDef:Lackland | `1.7/Models/person_hair_style_lackland.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_lackland.obj` | Clay delivery, selected runtime geometry |
| HairDef:Mop | `1.7/Models/person_hair_style_mop.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_mop.obj` | Clay delivery, selected runtime geometry |
| HairDef:Ponytails | `1.7/Models/person_hair_style_ponytails.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_ponytails.obj` | Clay delivery, selected runtime geometry |
| HairDef:Primal | `1.7/Models/person_hair_style_primal.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_primal.obj` | Clay delivery, selected runtime geometry |
| HairDef:Randy | `1.7/Models/person_hair_style_randy.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_randy.obj` | Clay delivery, selected runtime geometry |
| HairDef:Revolt | `1.7/Models/person_hair_style_revolt.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_revolt.obj` | Clay delivery, selected runtime geometry |
| HairDef:Rockstar | `1.7/Models/person_hair_style_rockstar.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_rockstar.obj` | Clay delivery, selected runtime geometry |
| HairDef:Rookie | `1.7/Models/person_hair_style_rookie.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_rookie.obj` | Clay delivery, selected runtime geometry |
| HairDef:Scatman | `1.7/Models/person_hair_style_scatman.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_scatman.obj` | Clay delivery, selected runtime geometry |
| HairDef:Scrapper | `1.7/Models/person_hair_style_scrapper.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_scrapper.obj` | Clay delivery, selected runtime geometry |
| HairDef:Senorita | `1.7/Models/person_hair_style_senorita.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_senorita.obj` | Clay delivery, selected runtime geometry |
| HairDef:ShaveTopBraid | `1.7/Models/person_hair_style_shavetopbraid.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_shavetopbraid.obj` | Clay delivery, selected runtime geometry |
| HairDef:Snazzy | `1.7/Models/person_hair_style_snazzy.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_snazzy.obj` | Clay delivery, selected runtime geometry |
| HairDef:Troubadour | `1.7/Models/person_hair_style_troubadour.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_troubadour.obj` | Clay delivery, selected runtime geometry |
| HairDef:Warden | `1.7/Models/person_hair_style_warden.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_warden.obj` | Clay delivery, selected runtime geometry |
| HairDef:Wavy | `1.7/Models/person_hair_style_wavy.obj` | Z09 / `IsoRim_Clay_Hairstyles_091-120/hair/person_hair_style_wavy.obj` | Clay delivery, selected runtime geometry |
| HairParts.Hair: Pigtails | `1.7/Models/person_hair_twintails.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/hair/person_hair_twintails.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_airwire_headset.obj` | Z04 / `Models/person_hat_airwire_headset.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_array_headset.obj` | Z04 / `Models/person_hat_array_headset.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_beret.obj` | Z18 / `Models/person_hat_beret.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_bowler_hat.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_bowler_hat.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_brim.obj` | Z02 / `Models/person_hat_brim.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_cap.obj` | Z02 / `Models/person_hat_cap.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_cataphract_helm.obj` | Z18 / `Models/person_hat_cataphract_helm.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_cloth_hood.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_cloth_hood.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_cloth_mask.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_cloth_mask.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_combat.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_combat.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_coronet.obj` | Z18 / `Models/person_hat_coronet.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_cowboy_hat.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/headgear/person_hat_cowboy_hat.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_crown.obj` | Z18 / `Models/person_hat_crown.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_crown_stellic.obj` | Z18 / `Models/person_hat_crown_stellic.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_cultist_mask.obj` | Z03 / `Models/person_hat_cultist_mask.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_eltex_skullcap.obj` | Z18 / `Models/person_hat_eltex_skullcap.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_fullhelm.obj` | Z02 / `Models/person_hat_fullhelm.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_gas_mask.obj` | Z04 / `Models/person_hat_gas_mask.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_gunlink.obj` | Z18 / `Models/person_hat_gunlink.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_helmet.obj` | Z02 / `Models/person_hat_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_hood.obj` | Z02 / `Models/person_hat_hood.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_integrator_headset.obj` | Z04 / `Models/person_hat_integrator_headset.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_kid_helmet.obj` | Z04 / `Models/person_hat_kid_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_ladies_hat.obj` | Z18 / `Models/person_hat_ladies_hat.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_marine_helm.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_marine_helm.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_mechcommander_helmet.obj` | Z04 / `Models/person_hat_mechcommander_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_mechlord_helmet.obj` | Z04 / `Models/person_hat_mechlord_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_psychic_foil_helmet.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_psychic_foil_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_psyfocus_helmet.obj` | Z18 / `Models/person_hat_psyfocus_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_recon_helm.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_recon_helm.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_simple_helmet.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/headgear/person_hat_simple_helmet.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_tall.obj` | Z02 / `Models/person_hat_tall.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_top_hat.obj` | Z18 / `Models/person_hat_top_hat.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_tribal_headdress.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hat_tribal_headdress.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_tuque.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/headgear/person_hat_tuque.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_vacsuit_helmet.obj` | Z05 / `Models/person_hat_vacsuit_helmet.obj` | Clay delivery with retained geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_war_mask.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/headgear/person_hat_war_mask.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_war_veil.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/headgear/person_hat_war_veil.obj` | Clay delivery, selected runtime geometry |
| People.OwnHats / OwnFaceWear / HatKind | `1.7/Models/person_hat_whisperer_hood.obj` | Z03 / `Models/person_hat_whisperer_hood.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/person_head.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_head.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_cataphract | `1.7/Models/person_hulk_armor_cataphract.obj` | Z18 / `Models/person_hulk_armor_cataphract.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_flakjacket | `1.7/Models/person_hulk_armor_flakjacket.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hulk_armor_flakjacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_flakpants | `1.7/Models/person_hulk_armor_flakpants.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hulk_armor_flakpants.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_flakvest | `1.7/Models/person_hulk_armor_flakvest.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_hulk_armor_flakvest.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_flakvest_front | `1.7/Models/person_hulk_armor_flakvest_front.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_hulk_armor_flakvest_front.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_marine | `1.7/Models/person_hulk_armor_marine.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hulk_armor_marine.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_plate | `1.7/Models/person_hulk_armor_plate.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hulk_armor_plate.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, armor_recon | `1.7/Models/person_hulk_armor_recon.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hulk_armor_recon.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, bandolier | `1.7/Models/person_hulk_bandolier.obj` | Z04 / `Models/person_hulk_bandolier.obj` | Clay delivery with retained geometry |
| People.Work: hulk body fit, bandolier_overvest | `1.7/Models/person_hulk_bandolier_overvest.obj` | Z04 / `Models/person_hulk_bandolier_overvest.obj` | Clay delivery with retained geometry |
| People.Work: hulk body fit, coat_long | `1.7/Models/person_hulk_coat_long.obj` | Z02 / `Models/person_hulk_coat_long.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, coat_short | `1.7/Models/person_hulk_coat_short.obj` | Z02 / `Models/person_hulk_coat_short.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, duster | `1.7/Models/person_hulk_duster.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_hulk_duster.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, jacket | `1.7/Models/person_hulk_jacket.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_hulk_jacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, labcoat | `1.7/Models/person_hulk_labcoat.obj` | Z03 / `Models/person_hulk_labcoat.obj` | Clay delivery with retained geometry |
| People.Work: hulk body fit, locust | `1.7/Models/person_hulk_locust.obj` | Z18 / `Models/person_hulk_locust.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, lower | `1.7/Models/person_hulk_lower.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_hulk_lower.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, mechlord | `1.7/Models/person_hulk_mechlord.obj` | Z04 / `Models/person_hulk_mechlord.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, parka | `1.7/Models/person_hulk_parka.obj` | Z04 / `Models/person_hulk_parka.obj` | Clay delivery, prior approved export reused |
| People.Work: hulk body fit, robe | `1.7/Models/person_hulk_robe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_hulk_robe.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, robe_psy | `1.7/Models/person_hulk_robe_psy.obj` | Z18 / `Models/person_hulk_robe_psy.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, robe_royal | `1.7/Models/person_hulk_robe_royal.obj` | Z18 / `Models/person_hulk_robe_royal.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, upper | `1.7/Models/person_hulk_upper.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_hulk_upper.obj` | Clay delivery, selected runtime geometry |
| People.Work: hulk body fit, vacsuit | `1.7/Models/person_hulk_vacsuit.obj` | Z05 / `Models/person_hulk_vacsuit.obj` | Clay delivery with retained geometry |
| People.Work: male body fit, armor_cataphract | `1.7/Models/person_male_armor_cataphract.obj` | Z18 / `Models/person_male_armor_cataphract.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_flakjacket | `1.7/Models/person_male_armor_flakjacket.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_male_armor_flakjacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_flakpants | `1.7/Models/person_male_armor_flakpants.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_male_armor_flakpants.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_flakvest | `1.7/Models/person_male_armor_flakvest.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_male_armor_flakvest.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_flakvest_front | `1.7/Models/person_male_armor_flakvest_front.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_male_armor_flakvest_front.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_marine | `1.7/Models/person_male_armor_marine.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_male_armor_marine.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_plate | `1.7/Models/person_male_armor_plate.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_male_armor_plate.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, armor_recon | `1.7/Models/person_male_armor_recon.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_male_armor_recon.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, bandolier | `1.7/Models/person_male_bandolier.obj` | Z04 / `Models/person_male_bandolier.obj` | Clay delivery with retained geometry |
| People.Work: male body fit, bandolier_overvest | `1.7/Models/person_male_bandolier_overvest.obj` | Z04 / `Models/person_male_bandolier_overvest.obj` | Clay delivery with retained geometry |
| People.Work: male body fit, coat_long | `1.7/Models/person_male_coat_long.obj` | Z02 / `Models/person_male_coat_long.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, coat_short | `1.7/Models/person_male_coat_short.obj` | Z02 / `Models/person_male_coat_short.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, duster | `1.7/Models/person_male_duster.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_male_duster.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, jacket | `1.7/Models/person_male_jacket.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_male_jacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, labcoat | `1.7/Models/person_male_labcoat.obj` | Z03 / `Models/person_male_labcoat.obj` | Clay delivery with retained geometry |
| People.Work: male body fit, locust | `1.7/Models/person_male_locust.obj` | Z18 / `Models/person_male_locust.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, lower | `1.7/Models/person_male_lower.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_male_lower.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, mechlord | `1.7/Models/person_male_mechlord.obj` | Z04 / `Models/person_male_mechlord.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, parka | `1.7/Models/person_male_parka.obj` | Z04 / `Models/person_male_parka.obj` | Clay delivery, prior approved export reused |
| People.Work: male body fit, robe | `1.7/Models/person_male_robe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_male_robe.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, robe_psy | `1.7/Models/person_male_robe_psy.obj` | Z18 / `Models/person_male_robe_psy.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, robe_royal | `1.7/Models/person_male_robe_royal.obj` | Z18 / `Models/person_male_robe_royal.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, upper | `1.7/Models/person_male_upper.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_male_upper.obj` | Clay delivery, selected runtime geometry |
| People.Work: male body fit, vacsuit | `1.7/Models/person_male_vacsuit.obj` | Z05 / `Models/person_male_vacsuit.obj` | Clay delivery with retained geometry |
| Apparel_PackBandwidth:model | `1.7/Models/person_pack_bandwidth.obj` | Z04 / `Models/person_pack_bandwidth.obj` | Clay delivery with retained geometry |
| Apparel_PackBroadshield:model | `1.7/Models/person_pack_broadshield.obj` | Z18 / `Models/person_pack_broadshield.obj` | Clay delivery with retained geometry |
| Apparel_CerebrexNode:model | `1.7/Models/person_pack_cerebrex.obj` | Z05 / `Models/person_pack_cerebrex.obj` | Clay delivery with retained geometry |
| Apparel_PackControl:model | `1.7/Models/person_pack_control.obj` | Z04 / `Models/person_pack_control.obj` | Clay delivery with retained geometry |
| Apparel_DeadlifePack:model | `1.7/Models/person_pack_deadlife.obj` | Z03 / `Models/person_pack_deadlife.obj` | Clay delivery with retained geometry |
| Apparel_DisruptorFlarePack:model | `1.7/Models/person_pack_disruptor.obj` | Z03 / `Models/person_pack_disruptor.obj` | Clay delivery with retained geometry |
| Apparel_FirefoampopPack:model | `1.7/Models/person_pack_firefoam.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_pack_firefoam.obj` | Clay delivery, selected runtime geometry |
| Apparel_PackHunter:model | `1.7/Models/person_pack_hunter.obj` | Z05 / `Models/person_pack_hunter.obj` | Clay delivery with retained geometry |
| Apparel_PackJump:model | `1.7/Models/person_pack_jump.obj` | Z18 / `Models/person_pack_jump.obj` | Clay delivery with retained geometry |
| Apparel_PackTox:model | `1.7/Models/person_pack_tox.obj` | Z04 / `Models/person_pack_tox.obj` | Clay delivery with retained geometry |
| Apparel_PackTurret:model | `1.7/Models/person_pack_turret.obj` | Z03 / `Models/person_pack_turret.obj` | Clay delivery with retained geometry |
| People.Work: thin body fit, armor_cataphract | `1.7/Models/person_thin_armor_cataphract.obj` | Z18 / `Models/person_thin_armor_cataphract.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_flakjacket | `1.7/Models/person_thin_armor_flakjacket.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_thin_armor_flakjacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_flakpants | `1.7/Models/person_thin_armor_flakpants.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_thin_armor_flakpants.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_flakvest | `1.7/Models/person_thin_armor_flakvest.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_thin_armor_flakvest.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_flakvest_front | `1.7/Models/person_thin_armor_flakvest_front.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_thin_armor_flakvest_front.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_marine | `1.7/Models/person_thin_armor_marine.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_thin_armor_marine.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_plate | `1.7/Models/person_thin_armor_plate.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_thin_armor_plate.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, armor_recon | `1.7/Models/person_thin_armor_recon.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_thin_armor_recon.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, bandolier | `1.7/Models/person_thin_bandolier.obj` | Z04 / `Models/person_thin_bandolier.obj` | Clay delivery with retained geometry |
| People.Work: thin body fit, bandolier_overvest | `1.7/Models/person_thin_bandolier_overvest.obj` | Z04 / `Models/person_thin_bandolier_overvest.obj` | Clay delivery with retained geometry |
| People.Work: thin body fit, coat_long | `1.7/Models/person_thin_coat_long.obj` | Z02 / `Models/person_thin_coat_long.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, coat_short | `1.7/Models/person_thin_coat_short.obj` | Z02 / `Models/person_thin_coat_short.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, duster | `1.7/Models/person_thin_duster.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_thin_duster.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, jacket | `1.7/Models/person_thin_jacket.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_thin_jacket.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, labcoat | `1.7/Models/person_thin_labcoat.obj` | Z03 / `Models/person_thin_labcoat.obj` | Clay delivery with retained geometry |
| People.Work: thin body fit, locust | `1.7/Models/person_thin_locust.obj` | Z18 / `Models/person_thin_locust.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, lower | `1.7/Models/person_thin_lower.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_thin_lower.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, mechlord | `1.7/Models/person_thin_mechlord.obj` | Z04 / `Models/person_thin_mechlord.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, parka | `1.7/Models/person_thin_parka.obj` | Z04 / `Models/person_thin_parka.obj` | Clay delivery, prior approved export reused |
| People.Work: thin body fit, robe | `1.7/Models/person_thin_robe.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_thin_robe.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, robe_psy | `1.7/Models/person_thin_robe_psy.obj` | Z18 / `Models/person_thin_robe_psy.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, robe_royal | `1.7/Models/person_thin_robe_royal.obj` | Z18 / `Models/person_thin_robe_royal.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, upper | `1.7/Models/person_thin_upper.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_thin_upper.obj` | Clay delivery, selected runtime geometry |
| People.Work: thin body fit, vacsuit | `1.7/Models/person_thin_vacsuit.obj` | Z05 / `Models/person_thin_vacsuit.obj` | Clay delivery with retained geometry |
| OrbitalTargeterBombardment:model | `1.7/Models/person_tool_bombardment.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_tool_bombardment.obj` | Clay delivery, selected runtime geometry |
| Apparel_PsychicInsanityLance:model | `1.7/Models/person_tool_insanity.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_tool_insanity.obj` | Clay delivery, selected runtime geometry |
| OrbitalTargeterMechCluster:model | `1.7/Models/person_tool_mechcluster.obj` | Z18 / `Models/person_tool_mechcluster.obj` | Clay delivery with retained geometry |
| OrbitalTargeterPowerBeam:model | `1.7/Models/person_tool_powerbeam.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_tool_powerbeam.obj` | Clay delivery, selected runtime geometry |
| Apparel_PsychicShockLance:model | `1.7/Models/person_tool_shock.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_tool_shock.obj` | Clay delivery, selected runtime geometry |
| TornadoGenerator:model | `1.7/Models/person_tool_tornado.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/person_tool_tornado.obj` | Clay delivery, selected runtime geometry |
| Piano:model; Royalty:Piano | `1.7/Models/piano.obj` | Z18 / `Models/piano.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman_head.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman_head.obj` | Clay delivery with retained geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman_leg_left_front.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman_leg_left_front.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman_leg_left_rear.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman_leg_left_rear.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman_leg_right_front.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman_leg_right_front.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman_leg_right_rear.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman_leg_right_rear.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Pikeman; Mech_Pikeman:model | `1.7/Models/pikeman_weapon.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/pikeman_weapon.obj` | Clay delivery with retained geometry |
| Core:Pila; Pila:model | `1.7/Models/pila.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/pila.obj` | Clay delivery, selected runtime geometry |
| Odyssey:PilotConsole; PilotConsole:model | `1.7/Models/pilot_console.obj` | Z05 / `Models/pilot_console.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_Autopistol; Gun_Autopistol:model | `1.7/Models/pistol.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/pistol.obj` | Clay delivery, selected runtime geometry |
| Core:PlantPot; PlantPot:model | `1.7/Models/plant_pot.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/plant_pot.obj` | Clay delivery, selected runtime geometry |
| Core:PodLauncher; PodLauncher:model | `1.7/Models/pod_launcher.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/pod_launcher.obj` | Clay delivery, selected runtime geometry |
| Core:PokerTable; PokerTable:model | `1.7/Models/poker_table.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/poker_table.obj` | Clay delivery, selected runtime geometry |
| Biotech:PollutionPump; PollutionPump:model | `1.7/Models/pollution_pump.obj` | Z04 / `Models/pollution_pump.obj` | Clay delivery with retained geometry |
| Core:Plant_Potato; Plant_Potato:li | `1.7/Models/potato_1.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/potato_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Potato; Plant_Potato:li | `1.7/Models/potato_2.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/potato_2.obj` | Clay delivery, selected runtime geometry |
| Royalty:UnstablePowerCell; UnstablePowerCell:model | `1.7/Models/power_cell.obj` | Z18 / `Models/power_cell.obj` | Clay delivery with retained geometry |
| Core:PowerSwitch; PowerSwitch:model | `1.7/Models/power_switch.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/power_switch.obj` | Clay delivery, selected runtime geometry |
| ActivatorProximity:model; Royalty:ActivatorProximity | `1.7/Models/proximity.obj` | Z18 / `Models/proximity.obj` | Clay delivery with retained geometry |
| Anomaly:ProximityDetector; ProximityDetector:model | `1.7/Models/proximity_detector.obj` | Z03 / `Models/proximity_detector.obj` | Clay delivery, selected runtime geometry |
| PsychicDroner:model; Royalty:PsychicDroner | `1.7/Models/psychic_droner.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/psychic_droner.obj` | Clay delivery, selected runtime geometry |
| PsychicSuppressor:model; Royalty:PsychicSuppressor | `1.7/Models/psychic_suppressor.obj` | Z18 / `Models/psychic_suppressor.obj` | Clay delivery, selected runtime geometry |
| Biotech:PsychofluidPump; PsychofluidPump:model | `1.7/Models/psychofluid_pump.obj` | Z04 / `Models/psychofluid_pump.obj` | Clay delivery with retained geometry |
| Core:Plant_Psychoid; Plant_Psychoid:model | `1.7/Models/psychoid.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/psychoid.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Revenant; Revenant:model | `1.7/Models/revenant_arm_-1.obj` | Z03 / `Models/revenant_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Revenant; Revenant:model | `1.7/Models/revenant_arm_1.obj` | Z03 / `Models/revenant_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Revenant; Revenant:model | `1.7/Models/revenant_body.obj` | Z03 / `Models/revenant_body.obj` | Clay delivery with retained geometry |
| Anomaly:Revenant; Revenant:model | `1.7/Models/revenant_head.obj` | Z03 / `Models/revenant_head.obj` | Clay delivery with retained geometry |
| Anomaly:Revenant; Revenant:model | `1.7/Models/revenant_leg_-1.obj` | Z03 / `Models/revenant_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Revenant; Revenant:model | `1.7/Models/revenant_leg_1.obj` | Z03 / `Models/revenant_leg_1.obj` | Clay delivery with retained geometry |
| Core:Gun_Revolver; Gun_Revolver:model | `1.7/Models/revolver.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/revolver.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Rice; Plant_Rice:li | `1.7/Models/rice_1.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/rice_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_Rice; Plant_Rice:li | `1.7/Models/rice_2.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/organics/rice_2.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_DoomsdayRocket; Core:Gun_TripleRocket; Gun_DoomsdayRocket:model; Gun_TripleRocket:model | `1.7/Models/rocket_launcher.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/rocket_launcher.obj` | Clay delivery, selected runtime geometry |
| Core:Turret_RocketswarmLauncher; Turret_RocketswarmLauncher:model | `1.7/Models/rocketswarm_base.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/rocketswarm_base.obj` | Clay delivery with retained geometry |
| Core:Turret_RocketswarmLauncher; Turret_RocketswarmLauncher:topModel | `1.7/Models/rocketswarm_top_empty.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/rocketswarm_top_empty.obj` | Clay delivery with retained geometry |
| Core:Turret_RocketswarmLauncher; Turret_RocketswarmLauncher:topModelLoaded | `1.7/Models/rocketswarm_top_full.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/rocketswarm_top_full.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/roof_rim.obj` | Z05 / `Models/roof_rim.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/roof_rim90.obj` | Z05 / `Models/roof_rim90.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/roof_rim_diagonal.obj` | Z05 / `Models/roof_rim_diagonal.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/roof_square.obj` | Z05 / `Models/roof_square.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/roof_triangle.obj` | Z05 / `Models/roof_triangle.obj` | Clay delivery with retained geometry |
| Core:RoyalBed; RoyalBed:model | `1.7/Models/royal_bed.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/royal_bed.obj` | Clay delivery with retained geometry |
| Core:AncientPipelineSection; Core:AncientPipes; Odyssey:AncientPipe; R3D_Match_209_ruin_pipe:model | `1.7/Models/ruin_pipe.obj` | Z05 / `Models/ruin_pipe.obj` | Clay delivery with retained geometry |
| Core:Sandbags; Sandbags:li | `1.7/Models/sandbags_1.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/sandbags_1.obj` | Clay delivery, selected runtime geometry |
| Core:Sandbags; Sandbags:li | `1.7/Models/sandbags_2.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/sandbags_2.obj` | Clay delivery, selected runtime geometry |
| Core:Sandbags; Sandbags:li | `1.7/Models/sandbags_3.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/sandbags_3.obj` | Clay delivery, selected runtime geometry |
| Core:Sarcophagus; Sarcophagus:model | `1.7/Models/sarcophagus.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/sarcophagus.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/scholar_female_face.obj` | Z03 / `Models/scholar_female_face.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/scholar_male_face.obj` | Z03 / `Models/scholar_male_face.obj` | Clay delivery with retained geometry |
| Biotech:SchoolDesk; SchoolDesk:model | `1.7/Models/school_desk.obj` | Z04 / `Models/school_desk.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher.obj` | Z04 / `Models/scorcher.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher_front_left.obj` | Z04 / `Models/scorcher_front_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher_front_right.obj` | Z04 / `Models/scorcher_front_right.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher_head.obj` | Z04 / `Models/scorcher_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher_rear_left.obj` | Z04 / `Models/scorcher_rear_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher_rear_right.obj` | Z04 / `Models/scorcher_rear_right.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Scorcher; Mech_Scorcher:model | `1.7/Models/scorcher_weapon.obj` | Z04 / `Models/scorcher_weapon.obj` | Clay delivery with retained geometry |
| Core:SculptureGrand; SculptureGrand:li | `1.7/Models/sculpture_grand_0.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_grand_0.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureGrand; SculptureGrand:li | `1.7/Models/sculpture_grand_1.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_grand_1.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureGrand; SculptureGrand:li | `1.7/Models/sculpture_grand_2.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_grand_2.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureLarge; SculptureLarge:li | `1.7/Models/sculpture_large_0.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_large_0.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureLarge; SculptureLarge:li | `1.7/Models/sculpture_large_1.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_large_1.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureLarge; SculptureLarge:li | `1.7/Models/sculpture_large_2.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_large_2.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureSmall; SculptureSmall:li | `1.7/Models/sculpture_small_0.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_small_0.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureSmall; SculptureSmall:li | `1.7/Models/sculpture_small_1.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_small_1.obj` | Clay delivery, selected runtime geometry |
| Core:SculptureSmall; SculptureSmall:li | `1.7/Models/sculpture_small_2.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sculpture_small_2.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther_arm_left.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther_arm_left.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther_arm_right.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther_arm_right.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther_blade_left.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther_blade_left.obj` | Clay delivery with retained geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther_blade_right.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther_blade_right.obj` | Clay delivery with retained geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther_head.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther_head.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Scyther; Mech_Scyther:model | `1.7/Models/scyther_wheel.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/scyther_wheel.obj` | Clay delivery with retained geometry |
| Anomaly:SerumCentrifuge; SerumCentrifuge:model | `1.7/Models/serum_lab.obj` | Z03 / `Models/serum_lab.obj` | Clay delivery, selected runtime geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/shambler_fat_body_overlay.obj` | Z03 / `Models/shambler_fat_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/shambler_female_body_overlay.obj` | Z03 / `Models/shambler_female_body_overlay.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/shambler_head_scars.obj` | Z03 / `Models/shambler_head_scars.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/shambler_hulk_body_overlay.obj` | Z03 / `Models/shambler_hulk_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/shambler_male_body_overlay.obj` | Z03 / `Models/shambler_male_body_overlay.obj` | Clay delivery with retained geometry |
| People.Anomaly: head/body/implant or ghoul hair part | `1.7/Models/shambler_thin_body_overlay.obj` | Z03 / `Models/shambler_thin_body_overlay.obj` | Clay delivery with retained geometry |
| Core:Shelf; Shelf:model | `1.7/Models/shelf.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/shelf.obj` | Clay delivery, selected runtime geometry |
| Core:ShelfSmall; ShelfSmall:model | `1.7/Models/shelf_small.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/shelf_small.obj` | Clay delivery, selected runtime geometry |
| Core:Shell_AntigrainWarhead; Core:Shell_HighExplosive; Shell_AntigrainWarhead:model; Shell_HighExplosive:model | `1.7/Models/shells_he.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/shells_he.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Shell_Deadlife; Biotech:Shell_Toxic; Core:Shell_EMP; Core:Shell_Firefoam; Core:Shell_Incendiary; Core:Shell_Smoke; MortarShells:model | `1.7/Models/shells_other.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/shells_other.obj` | Clay delivery, selected runtime geometry |
| Core:Ship_Beam; Ship_Beam:model | `1.7/Models/ship_beam.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_beam.obj` | Clay delivery, selected runtime geometry |
| Core:ShipChunk; ShipChunk:li | `1.7/Models/ship_chunk_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_chunk_1.obj` | Clay delivery, selected runtime geometry |
| Core:ShipChunk; ShipChunk:li | `1.7/Models/ship_chunk_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_chunk_2.obj` | Clay delivery, selected runtime geometry |
| Core:Ship_ComputerCore; Ship_ComputerCore:model | `1.7/Models/ship_computer_core.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_computer_core.obj` | Clay delivery, selected runtime geometry |
| Core:Ship_CryptosleepCasket; Ship_CryptosleepCasket:model | `1.7/Models/ship_cryptosleep_casket.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_cryptosleep_casket.obj` | Clay delivery, selected runtime geometry |
| Core:Ship_Engine; Ship_Engine:model | `1.7/Models/ship_engine.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_engine.obj` | Clay delivery, selected runtime geometry |
| Core:Ship_Reactor; Ship_Reactor:model | `1.7/Models/ship_reactor.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_reactor.obj` | Clay delivery, selected runtime geometry |
| Core:Ship_SensorCluster; Ship_SensorCluster:model | `1.7/Models/ship_sensor.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/ship_sensor.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_PumpShotgun; Gun_PumpShotgun:model | `1.7/Models/shotgun.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/shotgun.obj` | Clay delivery, selected runtime geometry |
| Core:ShuttleCrashed; ShuttleCrashed:model | `1.7/Models/shuttle_crashed.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/shuttle_crashed.obj` | Clay delivery, selected runtime geometry |
| Core:Shuttle; Shuttle:model | `1.7/Models/shuttle_imperial.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/shuttle_imperial.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_a_arm_-1.obj` | Z03 / `Models/sightstealer_a_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_a_arm_1.obj` | Z03 / `Models/sightstealer_a_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_a_body.obj` | Z03 / `Models/sightstealer_a_body.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_a_head.obj` | Z03 / `Models/sightstealer_a_head.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_a_leg_-1.obj` | Z03 / `Models/sightstealer_a_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_a_leg_1.obj` | Z03 / `Models/sightstealer_a_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_b_arm_-1.obj` | Z03 / `Models/sightstealer_b_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_b_arm_1.obj` | Z03 / `Models/sightstealer_b_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_b_body.obj` | Z03 / `Models/sightstealer_b_body.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_b_head.obj` | Z03 / `Models/sightstealer_b_head.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_b_leg_-1.obj` | Z03 / `Models/sightstealer_b_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_b_leg_1.obj` | Z03 / `Models/sightstealer_b_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_c_arm_-1.obj` | Z03 / `Models/sightstealer_c_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_c_arm_1.obj` | Z03 / `Models/sightstealer_c_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_c_body.obj` | Z03 / `Models/sightstealer_c_body.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_c_head.obj` | Z03 / `Models/sightstealer_c_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_c_leg_-1.obj` | Z03 / `Models/sightstealer_c_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_c_leg_1.obj` | Z03 / `Models/sightstealer_c_leg_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_d_arm_-1.obj` | Z03 / `Models/sightstealer_d_arm_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_d_arm_1.obj` | Z03 / `Models/sightstealer_d_arm_1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_d_body.obj` | Z03 / `Models/sightstealer_d_body.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_d_head.obj` | Z03 / `Models/sightstealer_d_head.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_d_leg_-1.obj` | Z03 / `Models/sightstealer_d_leg_-1.obj` | Clay delivery with retained geometry |
| Anomaly:Sightstealer; Sightstealer:model | `1.7/Models/sightstealer_d_leg_1.obj` | Z03 / `Models/sightstealer_d_leg_1.obj` | Clay delivery with retained geometry |
| Odyssey:SignalJammer; SignalJammer:model | `1.7/Models/signal_jammer.obj` | Z05 / `Models/signal_jammer.obj` | Clay delivery, selected runtime geometry |
| ChunkMechanoidSlag:li; ChunkSlagSteel:li; Core:ChunkMechanoidSlag; Core:ChunkSlagSteel | `1.7/Models/slag_1.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/slag_1.obj` | Clay delivery, selected runtime geometry |
| ChunkMechanoidSlag:li; ChunkSlagSteel:li; Core:ChunkMechanoidSlag; Core:ChunkSlagSteel | `1.7/Models/slag_2.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/slag_2.obj` | Clay delivery, selected runtime geometry |
| Anomaly:SleepSuppressor; SleepSuppressor:model | `1.7/Models/sleep_suppressor.obj` | Z03 / `Models/sleep_suppressor.obj` | Clay delivery, selected runtime geometry |
| Odyssey:SmallThruster; SmallThruster:model | `1.7/Models/small_thruster.obj` | Z05 / `Models/small_thruster.obj` | Clay delivery, selected runtime geometry |
| Core:Gun_HeavySMG; Gun_HeavySMG:model | `1.7/Models/smg.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/smg.obj` | Clay delivery, selected runtime geometry |
| Royalty:SmokeSpewer; SmokeSpewer:model | `1.7/Models/smoke_spewer.obj` | Z18 / `Models/smoke_spewer.obj` | Clay delivery with retained geometry |
| Core:Gun_SniperRifle; Gun_SniperRifle:model | `1.7/Models/sniper_rifle.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sniper_rifle.obj` | Clay delivery, selected runtime geometry |
| Core:SolarGenerator; SolarGenerator:model | `1.7/Models/solar_generator.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/solar_generator.obj` | Clay delivery, selected runtime geometry |
| Core:MeleeWeapon_Spear; MeleeWeapon_Spear:model | `1.7/Models/spear.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/spear.obj` | Clay delivery, selected runtime geometry |
| Core:TrapSpike; TrapSpike:model | `1.7/Models/spike_trap.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/spike_trap.obj` | Clay delivery, selected runtime geometry |
| Biotech:StandardRecharger; StandardRecharger:model | `1.7/Models/standard_recharger.obj` | Z04 / `Models/standard_recharger.obj` | Clay delivery with retained geometry |
| Core:StandingLamp; Darklight:model; StandingLamp:model; StandingLamp_Blue:model; StandingLamp_Green:model; StandingLamp_Red:model | `1.7/Models/standing_lamp.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/standing_lamp.obj` | Clay delivery, selected runtime geometry |
| Core:SteleGrand; SteleGrand:model | `1.7/Models/stele_grand.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/stele_grand.obj` | Clay delivery, selected runtime geometry |
| Core:SteleLarge; SteleLarge:model | `1.7/Models/stele_large.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/stele_large.obj` | Clay delivery, selected runtime geometry |
| Core:Stool; Stool:model | `1.7/Models/stool.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/stool.obj` | Clay delivery, selected runtime geometry |
| Biotech:SubcoreEncoder; SubcoreEncoder:model | `1.7/Models/subcore_encoder.obj` | Z04 / `Models/subcore_encoder.obj` | Clay delivery with retained geometry |
| Biotech:SubcoreRipscanner; SubcoreRipscanner:model | `1.7/Models/subcore_ripscanner.obj` | Z04 / `Models/subcore_ripscanner.obj` | Clay delivery with retained geometry |
| Biotech:SubcoreSoftscanner; SubcoreSoftscanner:model | `1.7/Models/subcore_softscanner.obj` | Z04 / `Models/subcore_softscanner.obj` | Clay delivery with retained geometry |
| Royalty:SunBlocker; SunBlocker:model | `1.7/Models/sun_blocker.obj` | Z18 / `Models/sun_blocker.obj` | Clay delivery, selected runtime geometry |
| Core:SunLamp; SunLamp:model | `1.7/Models/sun_lamp.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/sun_lamp.obj` | Clay delivery, selected runtime geometry |
| Core:Table1x2c; Table1x2c:model | `1.7/Models/table_1x2.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/table_1x2.obj` | Clay delivery, selected runtime geometry |
| Core:Table2x2c; Table2x2c:model | `1.7/Models/table_2x2.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/table_2x2.obj` | Clay delivery, selected runtime geometry |
| Core:Table2x4c; Table2x4c:model | `1.7/Models/table_2x4.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/table_2x4.obj` | Clay delivery, selected runtime geometry |
| Core:Table3x3c; Table3x3c:model | `1.7/Models/table_3x3.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/table_3x3.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TallGrass; Plant_TallGrass:li | `1.7/Models/tall_grass_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tall_grass_1.obj` | Clay delivery with retained geometry |
| Core:Plant_TallGrass; Plant_TallGrass:li | `1.7/Models/tall_grass_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tall_grass_2.obj` | Clay delivery with retained geometry |
| Core:Plant_TallGrass; Plant_TallGrass:li | `1.7/Models/tall_grass_3.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tall_grass_3.obj` | Clay delivery with retained geometry |
| AncientTankTrap:li; Core:AncientTankTrap | `1.7/Models/tank_trap_a.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/tank_trap_a.obj` | Clay delivery, selected runtime geometry |
| AncientTankTrap:li; Core:AncientTankTrap | `1.7/Models/tank_trap_b.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/tank_trap_b.obj` | Clay delivery, selected runtime geometry |
| AncientTankTrap:li; Core:AncientTankTrap | `1.7/Models/tank_trap_c.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/tank_trap_c.obj` | Clay delivery, selected runtime geometry |
| Core:Telescope; Telescope:model | `1.7/Models/telescope.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/telescope.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_head.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_head.obj` | Clay delivery, selected runtime geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_leg_left_1.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_leg_left_1.obj` | Clay delivery with retained geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_leg_left_2.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_leg_left_2.obj` | Clay delivery with retained geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_leg_left_3.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_leg_left_3.obj` | Clay delivery with retained geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_leg_right_1.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_leg_right_1.obj` | Clay delivery with retained geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_leg_right_2.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_leg_right_2.obj` | Clay delivery with retained geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_leg_right_3.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_leg_right_3.obj` | Clay delivery with retained geometry |
| Core:Mech_Termite; Mech_Termite:model | `1.7/Models/termite_weapon.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/termite_weapon.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tesseron; Mech_Tesseron:model | `1.7/Models/tesseron.obj` | Z04 / `Models/tesseron.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tesseron; Mech_Tesseron:model | `1.7/Models/tesseron_leg_left.obj` | Z04 / `Models/tesseron_leg_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tesseron; Mech_Tesseron:model | `1.7/Models/tesseron_leg_right.obj` | Z04 / `Models/tesseron_leg_right.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tesseron; Mech_Tesseron:model | `1.7/Models/tesseron_upper.obj` | Z04 / `Models/tesseron_upper.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tesseron; Mech_Tesseron:model | `1.7/Models/tesseron_weapon.obj` | Z04 / `Models/tesseron_weapon.obj` | Clay delivery with retained geometry |
| Royalty:Throne; Throne:model | `1.7/Models/throne.obj` | Z18 / `Models/throne.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/timeless_face.obj` | Z03 / `Models/timeless_face.obj` | Clay delivery with retained geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_axe.obj` | Z02 / `Models/tool_axe.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_broom.obj` | Z02 / `Models/tool_broom.obj` | Clay delivery, selected runtime geometry |
| Core:ToolCabinet; ToolCabinet:model | `1.7/Models/tool_cabinet.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/tool_cabinet.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_hammer.obj` | Z02 / `Models/tool_hammer.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_hoe.obj` | Z02 / `Models/tool_hoe.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_knife.obj` | Z02 / `Models/tool_knife.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_pickaxe.obj` | Z02 / `Models/tool_pickaxe.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/tool_scythe.obj` | Z02 / `Models/tool_scythe.obj` | Clay delivery, selected runtime geometry |
| Core:TorchLamp; TorchLamp:model | `1.7/Models/torch_lamp.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/utilities/torch_lamp.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_appendage_0.obj` | Z03 / `Models/toughspike_appendage_0.obj` | Clay delivery with retained geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_appendage_1.obj` | Z03 / `Models/toughspike_appendage_1.obj` | Clay delivery with retained geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_body.obj` | Z03 / `Models/toughspike_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_head.obj` | Z03 / `Models/toughspike_head.obj` | Clay delivery with retained geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_skirt_0.obj` | Z03 / `Models/toughspike_skirt_0.obj` | Clay delivery with retained geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_skirt_1.obj` | Z03 / `Models/toughspike_skirt_1.obj` | Clay delivery with retained geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_skirt_2.obj` | Z03 / `Models/toughspike_skirt_2.obj` | Clay delivery with retained geometry |
| Anomaly:Toughspike; Toughspike:model | `1.7/Models/toughspike_skirt_3.obj` | Z03 / `Models/toughspike_skirt_3.obj` | Clay delivery with retained geometry |
| Royalty:ToxicSpewer; ToxicSpewer:model | `1.7/Models/toxic_spewer.obj` | Z18 / `Models/toxic_spewer.obj` | Clay delivery, selected runtime geometry |
| Biotech:ToxifierGenerator; ToxifierGenerator:model | `1.7/Models/toxifier_generator.obj` | Z04 / `Models/toxifier_generator.obj` | Clay delivery with retained geometry |
| Biotech:ToyBox; ToyBox:model | `1.7/Models/toy_box.obj` | Z04 / `Models/toy_box.obj` | Clay delivery with retained geometry |
| Core:OrbitalTradeBeacon; OrbitalTradeBeacon:model | `1.7/Models/trade_beacon.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/trade_beacon.obj` | Clay delivery, selected runtime geometry |
| Core:TransportPod; TransportPod:model | `1.7/Models/transport_pod.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/transport_pod.obj` | Clay delivery, selected runtime geometry |
| Plant_TreeAnima:model; Royalty:Plant_TreeAnima | `1.7/Models/tree_anima.obj` | Z18 / `Models/tree_anima.obj` | Clay delivery with retained geometry |
| Core:Plant_TreeBamboo; Plant_TreeBamboo:li | `1.7/Models/tree_bamboo_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_bamboo_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeBamboo; Plant_TreeBamboo:li | `1.7/Models/tree_bamboo_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_bamboo_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeBirch; Plant_TreeBirch:li | `1.7/Models/tree_birch_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_birch_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeBirch; Plant_TreeBirch:li | `1.7/Models/tree_birch_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_birch_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_SaguaroCactus; Plant_SaguaroCactus:model | `1.7/Models/tree_cactus.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_cactus.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeCecropia; Plant_TreeCecropia:li | `1.7/Models/tree_cecropia_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_cecropia_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeCecropia; Plant_TreeCecropia:li | `1.7/Models/tree_cecropia_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_cecropia_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeCocoa; Plant_TreeCocoa:model | `1.7/Models/tree_cocoa.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_cocoa.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeCypress; Plant_TreeCypress:model | `1.7/Models/tree_cypress.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_cypress.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeDrago; Plant_TreeDrago:model | `1.7/Models/tree_drago.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_drago.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeMaple; Plant_TreeMaple:li | `1.7/Models/tree_maple_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_maple_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeMaple; Plant_TreeMaple:li | `1.7/Models/tree_maple_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_maple_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeOak; Plant_TreeOak:li | `1.7/Models/tree_oak_1.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/tree_oak_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreePalm; Plant_TreePalm:li | `1.7/Models/tree_palm_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_palm_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreePalm; Plant_TreePalm:li | `1.7/Models/tree_palm_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_palm_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreePine; Plant_TreePine:li | `1.7/Models/tree_pine_1.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/tree_pine_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreePine; Plant_TreePine:li | `1.7/Models/tree_pine_2.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/tree_pine_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreePoplar; Plant_TreePoplar:li | `1.7/Models/tree_poplar_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_poplar_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreePoplar; Plant_TreePoplar:li | `1.7/Models/tree_poplar_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_poplar_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeTeak; Plant_TreeTeak:li | `1.7/Models/tree_teak_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_teak_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeTeak; Plant_TreeTeak:li | `1.7/Models/tree_teak_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_teak_2.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeWillow; Plant_TreeWillow:li | `1.7/Models/tree_willow_1.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_willow_1.obj` | Clay delivery, selected runtime geometry |
| Core:Plant_TreeWillow; Plant_TreeWillow:li | `1.7/Models/tree_willow_2.obj` | Z14 / `IsoRim_Clay_Batch_03_Models/Models/tree_willow_2.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_appendage_0.obj` | Z03 / `Models/trispike_appendage_0.obj` | Clay delivery with retained geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_appendage_1.obj` | Z03 / `Models/trispike_appendage_1.obj` | Clay delivery with retained geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_appendage_2.obj` | Z03 / `Models/trispike_appendage_2.obj` | Clay delivery with retained geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_body.obj` | Z03 / `Models/trispike_body.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_head.obj` | Z03 / `Models/trispike_head.obj` | Clay delivery, selected runtime geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_skirt_0.obj` | Z03 / `Models/trispike_skirt_0.obj` | Clay delivery with retained geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_skirt_1.obj` | Z03 / `Models/trispike_skirt_1.obj` | Clay delivery with retained geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_skirt_2.obj` | Z03 / `Models/trispike_skirt_2.obj` | Clay delivery with retained geometry |
| Anomaly:Trispike; Trispike:model | `1.7/Models/trispike_skirt_3.obj` | Z03 / `Models/trispike_skirt_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler.obj` | Z04 / `Models/tunneler.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_arm_left.obj` | Z04 / `Models/tunneler_arm_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_arm_right.obj` | Z04 / `Models/tunneler_arm_right.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_claw_left_inner.obj` | Z04 / `Models/tunneler_claw_left_inner.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_claw_left_outer.obj` | Z04 / `Models/tunneler_claw_left_outer.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_claw_right_inner.obj` | Z04 / `Models/tunneler_claw_right_inner.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_claw_right_outer.obj` | Z04 / `Models/tunneler_claw_right_outer.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_head.obj` | Z04 / `Models/tunneler_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_track_left.obj` | Z04 / `Models/tunneler_track_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Tunneler; Mech_Tunneler:model | `1.7/Models/tunneler_track_right.obj` | Z04 / `Models/tunneler_track_right.obj` | Clay delivery with retained geometry |
| Core:Turret_Autocannon; Turret_Autocannon:model | `1.7/Models/turret_auto.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/turret_auto.obj` | Clay delivery, selected runtime geometry |
| Core:Turret_Autocannon; Turret_Autocannon:topModel | `1.7/Models/turret_gun_auto.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/turret_gun_auto.obj` | Clay delivery, selected runtime geometry |
| Core:Turret_MiniTurret; Turret_MiniTurret:topModel | `1.7/Models/turret_gun_mini.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/turret_gun_mini.obj` | Clay delivery, selected runtime geometry |
| Core:Turret_Mortar; Turret_Mortar:topModel | `1.7/Models/turret_gun_mortar.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/turret_gun_mortar.obj` | Clay delivery with retained geometry |
| Core:Turret_Sniper; Turret_Sniper:topModel | `1.7/Models/turret_gun_sniper.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/turret_gun_sniper.obj` | Clay delivery, selected runtime geometry |
| Core:Turret_MiniTurret; Turret_MiniTurret:model | `1.7/Models/turret_mini.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/turret_mini.obj` | Clay delivery, selected runtime geometry |
| Core:Turret_Mortar; Turret_Mortar:model | `1.7/Models/turret_mortar.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/turret_mortar.obj` | Clay delivery with retained geometry |
| Core:Turret_Sniper; Turret_Sniper:model | `1.7/Models/turret_sniper.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/turret_sniper.obj` | Clay delivery, selected runtime geometry |
| Core:FlatscreenTelevision; FlatscreenTelevision:model | `1.7/Models/tv_flat.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/furniture/tv_flat.obj` | Clay delivery, selected runtime geometry |
| Core:MegascreenTelevision; MegascreenTelevision:model | `1.7/Models/tv_mega.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/tv_mega.obj` | Clay delivery, selected runtime geometry |
| Core:TubeTelevision; TubeTelevision:model | `1.7/Models/tv_tube.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/tv_tube.obj` | Clay delivery, selected runtime geometry |
| Odyssey:VacBarrier; VacBarrier:model | `1.7/Models/vac_barrier.obj` | Z05 / `Models/vac_barrier.obj` | Clay delivery with retained geometry |
| Odyssey:VacBarrier; VacBarrier:doorTopModel | `1.7/Models/vac_barrier_top.obj` | Z05 / `Models/vac_barrier_top.obj` | Clay delivery with retained geometry |
| Core:Vent; Vent:model | `1.7/Models/vent.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/vent.obj` | Clay delivery, selected runtime geometry |
| ApparelArmor:model; Biotech:Apparel_MechlordSuit; Core:Apparel_ArmorRecon; Core:Apparel_FlakJacket; Core:Apparel_FlakPants; Core:Apparel_FlakVest; Core:Apparel_PlateArmor; Core:Apparel_PowerArmor; Odyssey:Apparel_Vacsuit; Odyssey:Apparel_VacsuitChildren; Royalty:Apparel_ArmorCataphract; Royalty:Apparel_ArmorCataphractPhoenix; Royalty:Apparel_ArmorCataphractPrestige; Royalty:Apparel_ArmorLocust; Royalty:Apparel_ArmorMarineGrenadier; Royalty:Apparel_ArmorMarinePrestige; Royalty:Apparel_ArmorReconPrestige | `1.7/Models/vest.obj` | Z02 / `Models/vest.obj` | Clay delivery, selected runtime geometry |
| Core:VitalsMonitor; VitalsMonitor:model | `1.7/Models/vitals_monitor.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/vitals_monitor.obj` | Clay delivery, selected runtime geometry |
| Core:WallLamp; WallLamp:model | `1.7/Models/wall_lamp.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/wall_lamp.obj` | Clay delivery, selected runtime geometry |
| Core:TorchWallLamp; TorchWallLamp:model | `1.7/Models/wall_torch.obj` | Z11 / `IsoRim_Clay_Objects_001-030/furniture/wall_torch.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_WarUrchin; Mech_WarUrchin:model | `1.7/Models/war_urchin.obj` | Z04 / `Models/war_urchin.obj` | Clay delivery with retained geometry |
| Biotech:Mech_WarUrchin; Mech_WarUrchin:model | `1.7/Models/war_urchin_head.obj` | Z04 / `Models/war_urchin_head.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_WarUrchin; Mech_WarUrchin:model | `1.7/Models/war_urchin_weapon.obj` | Z04 / `Models/war_urchin_weapon.obj` | Clay delivery with retained geometry |
| Biotech:Mech_WarUrchin; Mech_WarUrchin:model | `1.7/Models/war_urchin_wheel.obj` | Z04 / `Models/war_urchin_wheel.obj` | Clay delivery with retained geometry |
| MeleeWeapon_Warhammer:model; Royalty:MeleeWeapon_Warhammer | `1.7/Models/warhammer.obj` | Z18 / `Models/warhammer.obj` | Clay delivery, selected runtime geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen.obj` | Z04 / `Models/warqueen.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_hatch_left.obj` | Z04 / `Models/warqueen_hatch_left.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_hatch_right.obj` | Z04 / `Models/warqueen_hatch_right.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_head.obj` | Z04 / `Models/warqueen_head.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_left_1.obj` | Z04 / `Models/warqueen_leg_left_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_left_2.obj` | Z04 / `Models/warqueen_leg_left_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_left_3.obj` | Z04 / `Models/warqueen_leg_left_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_left_4.obj` | Z04 / `Models/warqueen_leg_left_4.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_left_5.obj` | Z04 / `Models/warqueen_leg_left_5.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_right_1.obj` | Z04 / `Models/warqueen_leg_right_1.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_right_2.obj` | Z04 / `Models/warqueen_leg_right_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_right_3.obj` | Z04 / `Models/warqueen_leg_right_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_right_4.obj` | Z04 / `Models/warqueen_leg_right_4.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_leg_right_5.obj` | Z04 / `Models/warqueen_leg_right_5.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_segment_2.obj` | Z04 / `Models/warqueen_segment_2.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_segment_3.obj` | Z04 / `Models/warqueen_segment_3.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_segment_4.obj` | Z04 / `Models/warqueen_segment_4.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_segment_5.obj` | Z04 / `Models/warqueen_segment_5.obj` | Clay delivery with retained geometry |
| Biotech:Mech_Warqueen; Mech_Warqueen:model | `1.7/Models/warqueen_turret_weapon.obj` | Z04 / `Models/warqueen_turret_weapon.obj` | Clay delivery with retained geometry |
| Biotech:WastepackAtomizer; WastepackAtomizer:model | `1.7/Models/wastepack_atomizer.obj` | Z04 / `Models/wastepack_atomizer.obj` | Clay delivery with retained geometry |
| Biotech:WastepackAtomizer; WastepackAtomizer:glassModel | `1.7/Models/wastepack_atomizer_window.obj` | Z04 / `Models/wastepack_atomizer_window.obj` | Clay delivery with retained geometry |
| Core:WatermillGenerator; WatermillGenerator:model | `1.7/Models/watermill.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/watermill.obj` | Clay delivery, selected runtime geometry |
| Royalty:WeatherController; WeatherController:model | `1.7/Models/weather_controller.obj` | Z18 / `Models/weather_controller.obj` | Clay delivery, selected runtime geometry |
| Source17 internal part (literal filename) | `1.7/Models/wind_rotor.obj` | Z02 / `Models/wind_rotor.obj` | Clay delivery, prior approved export reused |
| Core:WindTurbine; WindTurbine:model | `1.7/Models/wind_turbine.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/wind_turbine.obj` | Clay delivery, selected runtime geometry |
| Core:WoodFiredGenerator; WoodFiredGenerator:model | `1.7/Models/wood_fired_generator.obj` | Z15 / `IsoRim_Economical_60_Assets/NewModels/machines/wood_fired_generator.obj` | Clay delivery, selected runtime geometry |
| AncientRustedCarFrame:model; Core:AncientRustedCarFrame; R3D_Match_195_wreck_car_frame:model | `1.7/Models/wreck_car_frame.obj` | Z13 / `IsoRim_Clay_Batch_02_Models/Models/wreck_car_frame.obj` | Clay delivery, selected runtime geometry |

## Original 3D

These active authored paths have no selected Clay replacement. Shared fallbacks and recolours remain one row.

| Identity / part and shared aliases | Current path |
|---|---|
| Core:Plant_Agave; Plant_Agave:li | `Models/agave_1.obj` |
| Core:Plant_Agave; Plant_Agave:li | `Models/agave_2.obj` |
| Altar_Grand:model; Ideology:Altar_Grand | `Models/altar_grand.obj` |
| Altar_Large:model; Ideology:Altar_Large | `Models/altar_large.obj` |
| Altar_Medium:model; Ideology:Altar_Medium | `Models/altar_medium.obj` |
| Altar_Small:model; Ideology:Altar_Small | `Models/altar_small.obj` |
| AncientCommsConsole:model; Ideology:AncientCommsConsole | `Models/ancient_comms.obj` |
| AncientTerminal:model; AncientTerminal_Worshipful:model; Ideology:AncientTerminal; Ideology:AncientTerminal_Worshipful | `Models/ancient_terminal.obj` |
| Autobong:model; Ideology:Autobong | `Models/autobong.obj` |
| Core:Plant_Berry; Plant_Berry:li | `Models/berry_bush_1.obj` |
| Core:Plant_Berry; Plant_Berry:li | `Models/berry_bush_2.obj` |
| BiosculpterPod:model; Ideology:BiosculpterPod | `Models/biosculpter_pod.obj` |
| BiosculpterPod:glassModel; Ideology:BiosculpterPod | `Models/biosculpter_pod_glass.obj` |
| Ideology:Plant_TreeBonsai; Plant_TreeBonsai:li | `Models/bonsai_a.obj` |
| Ideology:Plant_TreeBonsai; Plant_TreeBonsai:li | `Models/bonsai_b.obj` |
| Ideology:Plant_TreeBonsai; Plant_TreeBonsai:li | `Models/bonsai_c.obj` |
| Ideology:PlantPot_Bonsai; PlantPot_Bonsai:model | `Models/bonsai_pot.obj` |
| Core:Plant_Brambles; Plant_Brambles:li | `Models/bramble_1.obj` |
| Core:Plant_Brambles; Plant_Brambles:li | `Models/bramble_2.obj` |
| Burnbong:model; Ideology:Burnbong | `Models/burnbong.obj` |
| Core:Plant_Bush; Plant_Bush:li | `Models/bush_1.obj` |
| Core:Plant_Bush; Plant_Bush:li | `Models/bush_2.obj` |
| Core:Plant_Bush; Plant_Bush:li | `Models/bush_3.obj` |
| CannibalPlatter:model; Ideology:CannibalPlatter | `Models/cannibal_platter.obj` |
| ChristmasTree:model; Ideology:ChristmasTree | `Models/christmas_tree.obj` |
| Core:AncientConcreteBarrier; R3D_Match_213_concrete_barrier:model | `Models/concrete_barrier.obj` |
| Core:Plant_Dandelion; Plant_Dandelion:li | `Models/dandelion_1.obj` |
| Core:Plant_Dandelion; Plant_Dandelion:li | `Models/dandelion_2.obj` |
| Core:Plant_Clivia; Core:Plant_Daylily; Plant_Clivia:model; Plant_Daylily:model | `Models/daylily.obj` |
| DryadCocoon:model; DryadHealingPod:model; Ideology:DryadCocoon; Ideology:DryadHealingPod | `Models/dryad_shell.obj` |
| AncientEnemyTerminal:model; Ideology:AncientEnemyTerminal | `Models/enemy_terminal.obj` |
| Biotech:RawToxipotato; PlantFoodRaw:model | `Models/food_potato.obj` |
| DarktorchFungus:model; Ideology:DarktorchFungus | `Models/fungus_darktorch.obj` |
| GaumakerCocoon:li; Ideology:GaumakerCocoon | `Models/gaumaker_a.obj` |
| GaumakerCocoon:li; Ideology:GaumakerCocoon | `Models/gaumaker_b.obj` |
| GaumakerCocoon:li; Ideology:GaumakerCocoon | `Models/gaumaker_c.obj` |
| Ideology:Plant_PodGauranlen; Plant_PodGauranlen:li | `Models/gauranlen_pod_a.obj` |
| Ideology:Plant_PodGauranlen; Plant_PodGauranlen:li | `Models/gauranlen_pod_b.obj` |
| Ideology:Plant_PodGauranlen; Plant_PodGauranlen:li | `Models/gauranlen_pod_c.obj` |
| GibbetCage:model; Ideology:GibbetCage | `Models/gibbet_cage.obj` |
| Core:Plant_Grass; Plant_Grass:li | `Models/grass_2.obj` |
| Core:Plant_Grass; Plant_Grass:li | `Models/grass_3.obj` |
| Core:Plant_Grass; Plant_Grass:li | `Models/grass_4.obj` |
| Core:Grave; Grave:li | `Models/grave_full.obj` |
| Core:Grave; Grave:li | `Models/grave_open.obj` |
| Core:Plant_Haygrass; Plant_Haygrass:li | `Models/haygrass_1.obj` |
| Core:Plant_Haygrass; Plant_Haygrass:li | `Models/haygrass_2.obj` |
| Core:Plant_Hops; Plant_Hops:model | `Models/hops.obj` |
| Ideology:IncenseShrine; IncenseShrine:model | `Models/incense_shrine.obj` |
| Textiles:model | `Models/item_cloth.obj` |
| R3D_Match_270_item_neurotrainer:model | `Models/item_neurotrainer.obj` |
| Core:BlocksMarble; Odyssey:BlocksVacstone; StoneBlocks:li | `Models/item_rm_blocks_a.obj` |
| Core:BlocksMarble; Odyssey:BlocksVacstone; StoneBlocks:li | `Models/item_rm_blocks_b.obj` |
| Core:BlocksMarble; Odyssey:BlocksVacstone; StoneBlocks:li | `Models/item_rm_blocks_c.obj` |
| Core:WoodLog; WoodLog:li | `Models/item_rm_logs_a.obj` |
| Core:WoodLog; WoodLog:li | `Models/item_rm_logs_b.obj` |
| Core:WoodLog; WoodLog:li | `Models/item_rm_logs_c.obj` |
| R3D_Match_271_item_techprint:model | `Models/item_techprint.obj` |
| Core:AncientRustedDropship; Odyssey:AncientRustedMicrowave; Odyssey:RubblePile; R3D_Match_215_junk_1:li | `Models/junk_1.obj` |
| Core:AncientRustedDropship; Odyssey:AncientRustedMicrowave; Odyssey:RubblePile; R3D_Match_215_junk_1:li | `Models/junk_2.obj` |
| Core:AncientRustedDropship; Odyssey:AncientRustedMicrowave; Odyssey:RubblePile; R3D_Match_215_junk_1:li | `Models/junk_3.obj` |
| Ideology:KneelPillow; KneelPillow:model | `Models/kneel_pillow.obj` |
| Ideology:KneelSheet; KneelSheet:model | `Models/kneel_sheet.obj` |
| Ideology:Lectern; Lectern:model | `Models/lectern.obj` |
| Ideology:LightBall; LightBall:model | `Models/lightball.obj` |
| Ideology:LightBall; LightBall:spinModel | `Models/lightball_rotor.obj` |
| Ideology:Loudspeaker; Loudspeaker:model | `Models/loudspeaker.obj` |
| Altar_Grand:model; Ideology:Altar_Grand | `Models/morbid_altar_grand.obj` |
| Altar_Large:model; Ideology:Altar_Large | `Models/morbid_altar_large.obj` |
| Altar_Medium:model; Ideology:Altar_Medium | `Models/morbid_altar_medium.obj` |
| Altar_Small:model; Ideology:Altar_Small | `Models/morbid_altar_small.obj` |
| Ideology:NeuralSupercharger; NeuralSupercharger:model | `Models/neural_supercharger.obj` |
| Source17 internal part (literal filename) | `Models/person_face.obj` |
| People.Work: fat body fit, bodystrap | `Models/person_fat_bodystrap.obj` |
| People.Work: fat body fit, bodystrap_overvest | `Models/person_fat_bodystrap_overvest.obj` |
| People.Work: fat body fit, burka_body | `Models/person_fat_burka_body.obj` |
| People.Work: fat body fit, cape | `Models/person_fat_cape.obj` |
| People.Work: female body fit, bodystrap | `Models/person_female_bodystrap.obj` |
| People.Work: female body fit, bodystrap_overvest | `Models/person_female_bodystrap_overvest.obj` |
| People.Work: female body fit, burka_body | `Models/person_female_burka_body.obj` |
| People.Work: female body fit, cape | `Models/person_female_cape.obj` |
| HairParts.Hair: Elder, Stephen | `Models/person_hair_balding.obj` |
| HairParts.Hair: Shaved, RoughShave, PatchyShave | `Models/person_hair_buzz.obj` |
| HairParts.Hair: Tuft, DoubleTuft | `Models/person_hair_crest.obj` |
| HairParts.Hair: Sticky, Locks, Savage | `Models/person_hair_shoulder.obj` |
| HairParts.Hair: Henry, Richard, Lisa, Jane | `Models/person_hair_sidebuns.obj` |
| HairParts.Hair: Victoria | `Models/person_hair_topbun_tail.obj` |
| HairParts.Hair:  | `Models/person_hair_topbun_tail_bun.obj` |
| HairParts.Hair: ScorpionTail | `Models/person_hair_topknot.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_authority_cap.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_blindfold.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_broadwrap.obj` |
| Source17 internal part (literal filename) | `Models/person_hat_burka.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_collar.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_flophat.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_headwrap.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_shadecone.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_slicecap.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_tailcap.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_torture_crown.obj` |
| People.OwnHats / OwnFaceWear / HatKind | `Models/person_hat_visage_mask.obj` |
| People.Work: hulk body fit, bodystrap | `Models/person_hulk_bodystrap.obj` |
| People.Work: hulk body fit, bodystrap_overvest | `Models/person_hulk_bodystrap_overvest.obj` |
| People.Work: hulk body fit, burka_body | `Models/person_hulk_burka_body.obj` |
| People.Work: hulk body fit, cape | `Models/person_hulk_cape.obj` |
| People.Work: male body fit, bodystrap | `Models/person_male_bodystrap.obj` |
| People.Work: male body fit, bodystrap_overvest | `Models/person_male_bodystrap_overvest.obj` |
| People.Work: male body fit, burka_body | `Models/person_male_burka_body.obj` |
| People.Work: male body fit, cape | `Models/person_male_cape.obj` |
| Source17 internal part (literal filename) | `Models/person_pads.obj` |
| People.Work: thin body fit, bodystrap | `Models/person_thin_bodystrap.obj` |
| People.Work: thin body fit, bodystrap_overvest | `Models/person_thin_bodystrap_overvest.obj` |
| People.Work: thin body fit, burka_body | `Models/person_thin_burka_body.obj` |
| People.Work: thin body fit, cape | `Models/person_thin_cape.obj` |
| Source17 internal part (literal filename) | `Models/person_visor.obj` |
| Ideology:Pew; Pew:model | `Models/pew.obj` |
| Core:AncientRazorWire; R3D_Match_214_razor_wire:model | `Models/razor_wire.obj` |
| Ideology:Reliquary; Reliquary:model | `Models/reliquary.obj` |
| Drum:model; Ideology:Drum | `Models/ritual_drum.obj` |
| Effigy:model; Ideology:Effigy | `Models/ritual_effigy.obj` |
| Ideology:Pyre; Pyre:model | `Models/ritual_pyre.obj` |
| Core:Plant_Rose; Plant_Rose:model | `Models/rose.obj` |
| Core:Plant_PincushionCactus; Plant_PincushionCactus:model | `Models/round_cactus.obj` |
| Core:AncientBarrel; R3D_Match_206_ruin_barrels:model | `Models/ruin_barrels.obj` |
| Core:AncientMegaCannonBarrel; R3D_Match_204_ruin_cannon:model | `Models/ruin_cannon.obj` |
| Core:AncientCrate; Odyssey:AncientCratePallet; R3D_Match_208_ruin_crate:model | `Models/ruin_crate.obj` |
| Core:AncientSpacerCrate; R3D_Match_207_ruin_crate_spacer:model | `Models/ruin_crate_spacer.obj` |
| Core:AncientLargeRustedEngineBlock; Core:AncientRustedEngineBlock; R3D_Match_210_ruin_engine:model | `Models/ruin_engine.obj` |
| Core:AncientLamp; Core:AncientLamppost; R3D_Match_212_ruin_lamppost:model | `Models/ruin_lamppost.obj` |
| Core:AncientEquipmentBlocks; Core:AncientGenerator; Core:AncientMachine; R3D_Match_211_ruin_machine:model | `Models/ruin_machine.obj` |
| Core:AncientMegaCannonTripod; R3D_Match_205_ruin_tripod:model | `Models/ruin_tripod.obj` |
| Ideology:SacrificialFlag; SacrificialFlag:model | `Models/sacrificial_flag.obj` |
| AncientSecurityCrate:model; Ideology:AncientSecurityCrate | `Models/security_crate.obj` |
| AncientSecurityCrate:emptyModel; Ideology:AncientSecurityCrate | `Models/security_crate_open.obj` |
| Ideology:Skullspike; Skullspike:model | `Models/skullspike.obj` |
| Ideology:SlabBed; SlabBed:model | `Models/slab_bed.obj` |
| Ideology:SlabDoubleBed; SlabDoubleBed:model | `Models/slab_double_bed.obj` |
| Ideology:SleepAccelerator; SleepAccelerator:model | `Models/sleep_accelerator.obj` |
| Core:Plant_Smokeleaf; Plant_Smokeleaf:model | `Models/smokeleaf.obj` |
| Core:Plant_Strawberry; Plant_Strawberry:li | `Models/strawberry_1.obj` |
| Core:Plant_Strawberry; Plant_Strawberry:li | `Models/strawberry_2.obj` |
| Ideology:StylingStation; StylingStation:model | `Models/styling_station.obj` |
| Altar_Grand:model; Ideology:Altar_Grand | `Models/totemic_altar_grand.obj` |
| Altar_Large:model; Ideology:Altar_Large | `Models/totemic_altar_large.obj` |
| Altar_Medium:model; Ideology:Altar_Medium | `Models/totemic_altar_medium.obj` |
| Altar_Small:model; Ideology:Altar_Small | `Models/totemic_altar_small.obj` |
| Ideology:Plant_TreeGauranlen; Plant_TreeGauranlen:model | `Models/tree_gauranlen.obj` |
| Core:Plant_TreeOak; Plant_TreeOak:li | `Models/tree_oak_2.obj` |
| Core:Plant_TreeOak; Plant_TreeOak:li | `Models/tree_oak_3.obj` |
| Core:AncientWarwalkerClaw; R3D_Match_202_warwalker_claw:model | `Models/warwalker_claw.obj` |
| Core:AncientMiniWarwalkerRemains; Core:AncientWarwalkerFoot; Core:AncientWarwalkerShell; R3D_Match_203_warwalker_foot:model | `Models/warwalker_foot.obj` |
| Core:AncientWarwalkerLeg; R3D_Match_201_warwalker_leg:model | `Models/warwalker_leg.obj` |
| Core:AncientWarwalkerTorso; R3D_Match_200_warwalker_torso:model | `Models/warwalker_torso.obj` |
| Core:AncientAPC; R3D_Match_198_wreck_apc:model | `Models/wreck_apc.obj` |
| Core:AncientRustedCar; R3D_Match_194_wreck_car:model | `Models/wreck_car.obj` |
| Core:AncientRustedJeep; R3D_Match_196_wreck_jeep:model | `Models/wreck_jeep.obj` |
| Core:AncientTank; R3D_Match_199_wreck_tank:model | `Models/wreck_tank.obj` |
| Core:AncientRustedTruck; Odyssey:AncientChemtruck; Odyssey:AncientIndustrialTruck; R3D_Match_197_wreck_truck:model | `Models/wreck_truck.obj` |

## No authored 3D yet

These rows retain the stated fallback. A procedural wall remains rendered 3D even though it has no authored OBJ.

| Pack | Identity | Current fallback |
|---|---|---|
| Core | AIPersonaCore | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | ActiveDropPod | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | ActiveDropPodAncient | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | ActiveDropPodMechanoid | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | AdrenalHeart | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Alligator | Game pawn sprite; no bound authored race model |
| Odyssey | AlphaThrumbo | Game pawn sprite; no bound authored race model |
| Odyssey | AlphaThrumboHorn | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Alphabeaver | Game pawn sprite; no bound authored race model |
| Core | Ambrosia | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | AncientAmmoStack | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientAntenna | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientArmchair | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientAutocannonTurret | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | AncientBandNode | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientBarrierLong | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientBasicRecharger | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientBathtub | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientBed | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientBox_ComponentIndustrial | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientBox_ComponentSpacer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientBox_SteelSlag | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientCardboardBox | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientChembarrel | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientChembarrelPallet | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientChemfuelCanister | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientColdStorage | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientCraneArm | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientCraneArmSmall | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientCraneBase | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientCraneColumn | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientDestroyedConsole | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientDestroyedConsoleLarge | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientDisplayBank | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientDoubleBed | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientDrillPlatform | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientDropshipEngine | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientEmergencyLight_Blue | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientEmergencyLight_Green | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientEmergencyLight_Red | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientExcavator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | AncientExostriderCannon | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | AncientExostriderHead | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | AncientExostriderLeg | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | AncientExostriderRemains | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientExplosivesCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientFilingCabinet | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientForklift | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientFortifiedWall | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | AncientFuelNode | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientGiantWheel | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientGravEngine | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientGravReactor | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientHeatVent | Game realtime sprite/effect; no authored body binding |
| Core | AncientHermeticCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientIndustrialShelf | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientJetEngine | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientKitchenSink | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientLargeContainer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientLargeCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientLargeMechGestator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientLockerBank | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientLockers | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientLongCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientMechGestator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientMechGestatorTank | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientMediumContainer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientMetalCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientMicrowave | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientMilitaryBarrier | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientMilitaryCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientMilitaryGeneratorSmall | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientMiningCharge | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientMortar | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientOpenContainer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientOperatingTable | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientOven | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientPallet_SteelSlag | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientPlantPot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientPodCar | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientPostbox | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientRefrigerator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSafe | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSealedContainer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSealedContainer_GravshipUpgrade | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSealedCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSealedCrate_Gravlite | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSealedCrate_Shells | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSecurityTerminal | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientSecurityTurret | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientShelf | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientShipBeacon | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientShoppingCart | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientShower | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSimpleResearchBench | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSingleBed | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSmallContainer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientSmallCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSmallShelf | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientSmokeVent | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientStandardRecharger | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientStorageCylinder | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientStove | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientSystemRack | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTV | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableBrewery | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableButcher | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableDrugLab | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableMachining | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableSmithingElectric | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableSmithingFueled | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableStoveElectric | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableStoveFueled | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableTailorElectric | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTableTailorHand | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTerraformer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientToilet | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientToxVent | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | AncientToxifierGenerator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTransportPod | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTunnelerClaw | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientTunnelerHusk | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientUplink | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientUraniumSlugTurret | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientVendingMachine | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientWarspiderRemains | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientWashingMachine | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AncientWheel | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientWindTurbineBody | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AncientWoodenCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | AnimalSleepingSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | AnimalistSlab_Broad | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | AnimalistSlab_Medium | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | AnticraftBeam | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | Apparel_Bandolier | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_BasicShirt | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_Beret | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | Apparel_BodyStrap | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | Apparel_Burka | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_CollarShirt | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_Coronet | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_Crown | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_CrownStellic | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_Duster | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_HatLadies | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_HatTop | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_HeavyShield | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_Jacket | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_KidPants | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_KidParka | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_KidRomper | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_KidShirt | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_KidTribal | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | Apparel_LabCoat | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_Pants | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_Parka | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_PsyfocusRobe | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_PsyfocusShirt | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Apparel_PsyfocusVest | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_Robe | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Apparel_Sash | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Apparel_TribalA | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | ArcheanSeed | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | ArchiteCapsule | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | ArchonexusCore | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | ArchotechTower | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Armadillo | Game pawn sprite; no bound authored race model |
| Core | Artillery_AutoMortar | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Artillery_Mortar | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | AtmosphericHeater | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | BabyFood | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | BabySleepingSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Badger | Game pawn sprite; no bound authored race model |
| Core | Barricade | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | BloodWarmer | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Bluebird | Game pawn sprite; no bound authored race model |
| Odyssey | BogHound | Game pawn sprite; no bound authored race model |
| Odyssey | Bow_Great_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | Brazier | Game realtime sprite/effect; no authored body binding |
| Royalty | BroadshieldCore | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Bullfrog | Game pawn sprite; no bound authored race model |
| Core | BurnedTree | Game plant sprite; no bound authored tree/stump model |
| Core | BurningPowerCell | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | BurrowWall | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | ButcherSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CaravanPackingSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Cat_Scimitar | Game pawn sprite; no bound authored race model |
| Core | CaveExit | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | CerebrexCore | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | CerebrexCore_Destroyed | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | CerebrexStabilizer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | ChoppedStump | Game plant sprite; no bound authored tree/stump model |
| Royalty | ChoppedStump_Anima | Game plant sprite; no bound authored tree/stump model |
| Odyssey | ChoppedStump_Archean | Game plant sprite; no bound authored tree/stump model |
| Ideology | ChoppedStump_Gauranlen | Game plant sprite; no bound authored tree/stump model |
| Anomaly | ChoppedStump_Harbinger | Game plant sprite; no bound authored tree/stump model |
| Biotech | ChoppedStump_Polluted | Game plant sprite; no bound authored tree/stump model |
| Biotech | ChoppedStump_Polux | Game plant sprite; no bound authored tree/stump model |
| Odyssey | CocoonLocust | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CocoonMegascarab | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CocoonMegaspider | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CocoonSpelopede | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CollapsedRocks | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | ColossusToad | Game pawn sprite; no bound authored race model |
| Biotech | ControlSublink | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | ControlSublinkHigh | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | CorrosiveHeart | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | CraftingSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CraterLarge | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CraterMedium | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | CraterSmall | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Crow | Game pawn sprite; no bound authored race model |
| Royalty | DarklightBrazier | Game realtime sprite/effect; no authored body binding |
| Ideology | Darktorch | Game realtime sprite/effect; no authored body binding |
| Core | DefoliatorShipPart | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | DirtCubeSculpture | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | DoubleSleepingSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Drone_Hunter | Game pawn sprite; no bound authored race model |
| Odyssey | Drone_Sentry | Game pawn sprite; no bound authored race model |
| Odyssey | Drone_Wasp | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Barkskin | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Basic | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Berrymaker | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Carrier | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Clawer | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Gaumaker | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Medicinemaker | Game pawn sprite; no bound authored race model |
| Ideology | Dryad_Woodmaker | Game pawn sprite; no bound authored race model |
| Core | Dye | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | EggSac | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | ElephantTusk | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | EndlessPit2x2c | Game realtime sprite/effect; no authored body binding |
| Anomaly | EndlessPit3x2c | Game realtime sprite/effect; no authored body binding |
| Anomaly | EndlessPit3x3c | Game realtime sprite/effect; no authored body binding |
| Odyssey | Fish_Bass | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Bluefish | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Catfish | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Cod | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Dogfish | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Flounder | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Frostfish | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Guppy | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Marlin | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Piranha | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Salmon | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Tilapia | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Toxfish | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Fish_Tuna | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Flamingo | Game pawn sprite; no bound authored race model |
| Anomaly | FleshSack | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | Fleshbulb | Game realtime sprite/effect; no authored body binding |
| Anomaly | Fleshmass | Linked sprite or specialised procedural layer; no authored model binding |
| Anomaly | FleshmassHeart | Game realtime sprite/effect; no authored body binding |
| Anomaly | FleshmassNeuralLump | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | FleshmassSpitter | Game realtime sprite/effect; no authored body binding |
| Anomaly | Fleshmass_Active | Linked sprite or specialised procedural layer; no authored model binding |
| Anomaly | FloorEtching | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | FloorEtchingRambling | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | FrenzyInducer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | FuelOptimizer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | GauranlenSeed | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | GaussCannon | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | Genepack | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | GeothermalVent | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | GhoulBarbs | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | GhoulPlating | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gloomlight | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | GlowPod | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | GoldenCube | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gorilla | Game pawn sprite; no bound authored race model |
| Ideology | GrandArchotechStructure | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Granite | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | GravAnchor | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | GravEngine | Game realtime sprite/effect; no authored body binding |
| Odyssey | Gravcore | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | GravcorePowerCell | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | GravshipHull | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | GravshipShieldGenerator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | GrayBox | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | GrayFleshSample | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | GrayStatueDeadlifeDust | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | GrayStatueTeleporter | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | GrayWall | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | Gun_AncientArmoredTurret | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_AssaultRifle_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gun_AutocannonTurret | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_BeamRepeater_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_BoltActionRifle_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_ChainShotgun_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Gun_ChargeBlasterTurret | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_ChargeLance_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_ChargeRifle_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_HeavySMG_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_HellcatRifle_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_LMG_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gun_MiniTurret | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gun_MiniTurretMech | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_Minigun_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_Revolver_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gun_RocketswarmLauncher | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Gun_SniperRifle_Unique | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | Gun_TacticalTurret | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gun_TurretFoam | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Gun_TurretSniper | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | HarbingerSeed | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | HemogenPack | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | HermitCrab | Game pawn sprite; no bound authored race model |
| Odyssey | Heron | Game pawn sprite; no bound authored race model |
| Core | HiddenConduit | Linked sprite or specialised procedural layer; no authored model binding |
| Odyssey | Hippo | Game pawn sprite; no bound authored race model |
| Odyssey | HiveQueen | Game pawn sprite; no bound authored race model |
| Anomaly | HoldingSpot | Game realtime sprite/effect; no authored body binding |
| Biotech | HumanEmbryo | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | HumanOvum | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | HunterDroneTrap | Game realtime sprite/effect; no authored body binding |
| Ideology | Ideogram | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | InfiniteChemreactor | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | InsectLairEntrance | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | LabyrinthMatter | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Larva | Game pawn sprite; no bound authored race model |
| Odyssey | LavaSnail | Game pawn sprite; no bound authored race model |
| Odyssey | LifeSupportUnit | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Limestone | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | Locust | Game pawn sprite; no bound authored race model |
| Odyssey | Macaw | Game pawn sprite; no bound authored race model |
| Ideology | MajorArchotechStructure | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | MajorArchotechStructureStudiable | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | MalfunctioningTransportPod | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Map | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Marble | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MarriageSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Mastodon | Game pawn sprite; no bound authored race model |
| Odyssey | MastodonTusk | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | MechFormfeeder | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | MechPylon | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | MechRelay | Game realtime sprite/effect; no authored body binding |
| Odyssey | MechRelay_Crashed | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | MechStabilizer | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Mech_Cyclops | Game pawn sprite; no bound authored race model |
| Odyssey | MechanoidDropPod | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | MechanoidProcessor | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | MechanoidTransponder | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | MechhiveAssembler | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | Mechlink | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | MeditationSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Megavole | Game pawn sprite; no bound authored race model |
| Royalty | MeleeWeapon_MonoSwordBladelink | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | MeleeWeapon_PlasmaSwordBladelink | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | MeleeWeapon_ZeusHammerBladelink | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | MetalbloodHeart | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | MineableComponentsIndustrial | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MineableGold | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MineableJade | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | MineableObsidian | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MineablePlasteel | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MineableSilver | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MineableSteel | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | MineableUranium | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | MinifiedGravEngine | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | MinifiedThing | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | MinifiedThingRealtimeDraw | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | MinifiedTree | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Mink | Game pawn sprite; no bound authored race model |
| Odyssey | MonitorLizard | Game pawn sprite; no bound authored race model |
| Anomaly | MonolithFragment | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Moose | Game pawn sprite; no bound authored race model |
| Ideology | MorbidSlab_Broad | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | MorbidSlab_Medium | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Muskox | Game pawn sprite; no bound authored race model |
| Biotech | NanostructuringChip | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | NerveBundle | Game realtime sprite/effect; no authored body binding |
| Anomaly | Noctolith | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Novel | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | OrbitalAncientFortifiedWall | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | Otter | Game pawn sprite; no bound authored race model |
| Odyssey | Pallet_Medicine | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Pallet_SurvivalMeals | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Panda | Game pawn sprite; no bound authored race model |
| Core | PartySpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | PassengerShuttle | Game realtime sprite/effect; no authored body binding |
| Odyssey | Peacock | Game pawn sprite; no bound authored race model |
| Odyssey | Penguin | Game pawn sprite; no bound authored race model |
| Odyssey | PilotSubpersonaCore | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | PitBurrow | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | PitGate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | PitGateExit | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | Plant_PebbleCactus | Game plant sprite; no bound authored tree/stump model |
| Biotech | Plant_RatPalm | Game plant sprite; no bound authored tree/stump model |
| Core | Plant_Timbershroom | Game plant sprite; no bound authored tree/stump model |
| Odyssey | Plant_TreeArchean | Game plant sprite; no bound authored tree/stump model |
| Odyssey | Plant_TreeCocoa_Wild | Game plant sprite; no bound authored tree/stump model |
| Biotech | Plant_TreeGrayPine | Game plant sprite; no bound authored tree/stump model |
| Anomaly | Plant_TreeHarbinger | Game plant sprite; no bound authored tree/stump model |
| Biotech | Plant_TreePolux | Game plant sprite; no bound authored tree/stump model |
| Odyssey | Plant_TreeSnagroot | Game plant sprite; no bound authored tree/stump model |
| Biotech | Plant_Witchwood | Game plant sprite; no bound authored tree/stump model |
| Biotech | PoluxSeed | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Porcupine | Game pawn sprite; no bound authored race model |
| Core | PowerConduit | Linked sprite or specialised procedural layer; no authored model binding |
| Biotech | PowerfocusChip | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | PrairieDog | Game pawn sprite; no bound authored race model |
| Core | PsychicDronerShipPart | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | PsychicEmanator | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | PsychicRitualSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Quail | Game pawn sprite; no bound authored race model |
| Core | RaisedRocks | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | ReinforcedBarrel | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertArk | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertBox | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertCube | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertCup | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertFragment | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertPendant | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertSwordHandle | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RelicInertTablet | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | RemoteRepairer | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | RemoteShielder | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | RepairProbe | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | RevenantFleshChunk | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | RevenantSpine | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | RitualSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | RusticRug_Broad | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | RusticRug_Medium | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | SandCubeSculpture | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Sandstone | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Biotech | SanguphageMeetingTorch | Game realtime sprite/effect; no authored body binding |
| Core | Schematic | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | ScrapCubeSculpture | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | SculptureTerror | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | SeaLion | Game pawn sprite; no bound authored race model |
| Odyssey | SeaTurtle | Game pawn sprite; no bound authored race model |
| Odyssey | Seal | Game pawn sprite; no bound authored race model |
| Odyssey | SealedCrate | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | SecurityDoor | Game realtime sprite/effect; no authored body binding |
| Anomaly | Shard | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | ShardBeacon | Game realtime sprite/effect; no authored body binding |
| Anomaly | ShardInhibitor | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | ShipChunk_Mech | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | ShuttleCrashed_Exitable | Game realtime sprite/effect; no authored body binding |
| Biotech | ShuttleCrashed_Exitable_Mechanitor | Game realtime sprite/effect; no authored body binding |
| Odyssey | ShuttleEngine | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | SignalChip | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Skull | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Slate | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | SleepingSpot | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | SmashedStump | Game plant sprite; no bound authored tree/stump model |
| Royalty | SmashedStump_Anima | Game plant sprite; no bound authored tree/stump model |
| Odyssey | SmashedStump_Archean | Game plant sprite; no bound authored tree/stump model |
| Ideology | SmashedStump_Gauranlen | Game plant sprite; no bound authored tree/stump model |
| Anomaly | SmashedStump_Harbinger | Game plant sprite; no bound authored tree/stump model |
| Biotech | SmashedStump_Polluted | Game plant sprite; no bound authored tree/stump model |
| Biotech | SmashedStump_Polux | Game plant sprite; no bound authored tree/stump model |
| Core | SmoothedGranite | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | SmoothedLimestone | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | SmoothedMarble | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | SmoothedSandstone | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | SmoothedSlate | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | SmoothedSolidIce | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | SmoothedVacstone | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | Snowman | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | SolidIce | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Ideology | Spacedrone | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Sparrow | Game pawn sprite; no bound authored race model |
| Ideology | SpikecoreFloorStar_Broad | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | SpikecoreFloorStar_Medium | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Statue | Game realtime sprite/effect; no authored body binding |
| Odyssey | StoneCrab | Game pawn sprite; no bound authored race model |
| Anomaly | StoneCubeSculpture | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | SubcoreBasic | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | SubcoreHigh | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | SubcoreRegular | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | SurveyScanner | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Swan | Game pawn sprite; no bound authored race model |
| Core | TechprofSubpersonaCore | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | TextBook | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | ThrumboHorn | Automatic sprite extrusion or flat sprite, subject to item settings |
| Odyssey | Tiger | Game pawn sprite; no bound authored race model |
| Anomaly | Tome | Automatic sprite extrusion or flat sprite, subject to item settings |
| Ideology | TotemicSlab_Broad | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Ideology | TotemicSlab_Medium | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Biotech | Toxalope | Game pawn sprite; no bound authored race model |
| Odyssey | Turret_AncientArmoredTurret | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Turret_AutoChargeBlaster | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Turret_AutoInferno | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Turret_AutoMiniTurret | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | Turret_AutoMortar | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | Turret_TacticalTurret | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | TwistedArchotechSupport_Large | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | TwistedArchotechSupport_Small | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | TwistedArchotechSupport_Vertical | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Core | UnfinishedApparel | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedArmor | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedBelt | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedComponent | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedGun | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedHealthItemBionic | Automatic sprite extrusion or flat sprite, subject to item settings |
| Anomaly | UnfinishedHealthItemGhoul | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedHealthItemProsthetic | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedMask | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedMetallicTechArmor | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedMusicalInstrument | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedPack | Automatic sprite extrusion or flat sprite, subject to item settings |
| Royalty | UnfinishedPiano | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedSculpture | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | UnfinishedSubcore | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedTechArmor | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | UnfinishedWeapon | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | Urn | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Odyssey | Vacstone | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Core | VanometricPowerCell | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidMonolith | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidMonolithAttachmentBottomLeft | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidMonolithAttachmentBottomRight | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidMonolithAttachmentLeft | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidMonolithAttachmentRight | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidMonolithAttachmentTop | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidNode | Game realtime sprite/effect; no authored body binding |
| Anomaly | VoidSculpture | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidStructure | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | VoidmetalMassLarge | Game realtime sprite/effect; no authored body binding |
| Anomaly | VoidmetalMassMedium | Game realtime sprite/effect; no authored body binding |
| Anomaly | VoidmetalMassSmall | Game realtime sprite/effect; no authored body binding |
| Anomaly | VoidmetalWall | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | Vulture | Game pawn sprite; no bound authored race model |
| Core | Wall | Procedural block/wall or natural rock (WallBoxes); no authored OBJ required |
| Odyssey | Walrus | Game pawn sprite; no bound authored race model |
| Anomaly | WarpedObelisk_Abductor | Game realtime sprite/effect; no authored body binding |
| Anomaly | WarpedObelisk_Duplicator | Game realtime sprite/effect; no authored body binding |
| Anomaly | WarpedObelisk_Labyrinth | Automatic sprite extrusion when enabled; floor-level graphics remain flat |
| Anomaly | WarpedObelisk_Mutator | Game realtime sprite/effect; no authored body binding |
| Odyssey | WaspDroneTrap | Game realtime sprite/effect; no authored body binding |
| Biotech | WasteRat | Game pawn sprite; no bound authored race model |
| Biotech | Wastepack | Automatic sprite extrusion or flat sprite, subject to item settings |
| Core | WaterproofConduit | Linked sprite or specialised procedural layer; no authored model binding |
| Odyssey | Wolf_Great | Game pawn sprite; no bound authored race model |
| Odyssey | Wolverine | Game pawn sprite; no bound authored race model |
| Core | Wort | Automatic sprite extrusion or flat sprite, subject to item settings |
| Biotech | Xenogerm | Automatic sprite extrusion or flat sprite, subject to item settings |

## Procedural and shared exceptions

| Identity family | Active treatment |
|---|---|
| Walls, natural rock, ore, smooth rock | WallBoxes generates blocks, exposed surfaces and cutaway geometry. The delivered wall study is not bound. |
| Ground, floors, water, ordinary roofs | Runtime terrain/layer geometry. Preview soil/floor/water/roof studies do not replace those generators. |
| Gravship roofs | Authored Odyssey kit parts combine with procedural fill. They are distinct from ordinary roofs. |
| Worn clothing and body builds | People.Work uses five shared body fits, explicit coat/armour tables, hats and WornGear. On-skin recolours share upper/lower bodies. |
| Hair and beards | HairParts maps named styles and shared aliases. Bald, NoBeard and Stubble intentionally have no part. Thirty delivered styles now bind unique parts. |
| Animals and mechs | XML body/leg/mechParts routes retain separate pivots and attachments. A reusable leg counts once, regardless of instance count. |
| Humans, children, babies and Anomaly humanoids | People builds a shared figure with age scaling, body fits, head widths and conditional overlays. No new age mesh is invented. |
| Effects, projectiles, motes, filth and gas | Game sprites or specialised runtime effects. Flame/core/puff are authored shared parts where referenced. These are not extra ThingDef census rows. |
| Runtime-generated meat, leather, corpses and blueprints | Category/shared shapes or game sprites follow runtime generation. They are not additional authored sculpts or concrete XML census rows. |

## Delivered but unmapped

These deliveries remain in private staging. They do not count as active Clay.

| Delivery | ZIP / member | Reason |
|---|---|---|
| agave_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/agave_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| agave_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/agave_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| animal_cobra_tail.obj | Z08 / `IsoRim_Clay_Animals_161-200/animals_b/animal_cobra_tail.obj` | No existing model identity or explicit binding found |
| berry_bush_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/berry_bush_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| berry_bush_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/berry_bush_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| bramble_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/bramble_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| bramble_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/bramble_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| bush_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/bush_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| bush_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/bush_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| dandelion_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/dandelion_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| dandelion_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/dandelion_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| daylily_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/daylily_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| daylily_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/daylily_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| grave_marker.obj | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/grave_marker.obj` | new_standalone_marker_study_requires_placement_and_runtime_binding_not_full_grave_replacement |
| haygrass_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/haygrass_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| haygrass_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/haygrass_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| hops_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/hops_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| hops_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/hops_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| item_wood.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_wood.obj` | Legacy root model. Active 1.7 WoodLog uses item_rm_wood_a/b/c; no matching stage delivery. |
| rose_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/rose_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| rose_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/rose_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| round_cactus_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/round_cactus_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| round_cactus_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/round_cactus_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| smokeleaf_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/smokeleaf_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| smokeleaf_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/smokeleaf_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| strawberry_mature.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/strawberry_mature.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| strawberry_young.obj | Z12 / `IsoRim_Clay_Resources_and_Plants_031-090/plants/strawberry_young.obj` | requires_growth_stage_selection_integration;current_variants_are_not_young_mature |
| Procedural studies | Z01, Z06, Z15, Z17 / Studies, ProceduralStudies, ProceduralWallStudy | Wall/soil/floor/water/ordinary-roof references require generator integration. Source/comparison meshes and Blender geometry remain excluded. |

## Duplicate and collision decisions

Z01 and Z06 are byte-identical ZIPs. The import reads that delivery once. Economical60 supersedes the earlier first-five and 30-trial exports.
Efficient90 selects the later shared bodies and generic hairstyles. Batch200 adds distinct hairstyles, not replacements for every generic hairstyle.
Batches02/03 add complementary parts. Batch04 explicitly reuses earlier support parts. Batch05 parka rows explicitly reuse Efficient90 exports.
The following table records every repeated runtime basename. Earlier members remain provenance alternatives rather than silent overwrites.

| Identity | Selected source | Earlier sources | Decision |
|---|---|---|---|
| animal_horse.obj | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_horse.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/animal_horse.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| animal_horse_leg.obj | Z07 / `IsoRim_Clay_Animals_121-160/animals_a/animal_horse_leg.obj` | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/animal_horse_leg.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| animal_muffalo.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/animal_muffalo.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/animal_muffalo.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| animal_muffalo_leg.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/animal_muffalo_leg.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/animal_muffalo_leg.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| armchair.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/armchair.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/armchair.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| assault_rifle.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/assault_rifle.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/assault_rifle.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| battery.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/battery.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/battery.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| bed.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bed.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/bed.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| bench_electric_smithy.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bench_electric_smithy.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/bench_electric_smithy.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| bench_research.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bench_research.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/bench_research.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| bench_stove.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/bench_stove.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/bench_stove.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| campfire.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/campfire.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/campfire.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| dining_chair.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/dining_chair.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/dining_chair.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| door_leaf.obj | Z02 / `Models/door_leaf.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/door_leaf.obj` (identical OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/door_leaf.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| door_lintel.obj | Z02 / `Models/door_lintel.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/door_lintel.obj` (identical OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/door_lintel.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| fx_flame.obj | Z02 / `Models/fx_flame.obj` | Z01 / `IsoRim_Clay_30_Trial/Effects/fx_flame.obj` (identical OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Effects/fx_flame.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| fx_flame_core.obj | Z02 / `Models/fx_flame_core.obj` | Z01 / `IsoRim_Clay_30_Trial/Effects/fx_flame_core.obj` (identical OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Effects/fx_flame_core.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| fx_puff.obj | Z02 / `Models/fx_puff.obj` | Z01 / `IsoRim_Clay_30_Trial/Effects/fx_puff.obj` (identical OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Effects/fx_puff.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| gate_leaf.obj | Z02 / `Models/gate_leaf.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/gate_leaf.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| gate_posts.obj | Z02 / `Models/gate_posts.obj` | Z11 / `IsoRim_Clay_Objects_001-030/props/Models/gate_posts.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| grass_1.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/grass_1.obj` | Z17 / `Models/grass_1.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| hospital_bed.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/hospital_bed.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/hospital_bed.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| hydroponics.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/hydroponics.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/hydroponics.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| item_medicine_industrial_a.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_medicine_industrial_a.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/item_medicine_industrial_a.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| item_medicine_industrial_b.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_medicine_industrial_b.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/item_medicine_industrial_b.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| item_medicine_industrial_c.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_medicine_industrial_c.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/item_medicine_industrial_c.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| item_wood.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/item_wood.obj` | Z17 / `Models/item_wood.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| paste_dispenser.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/paste_dispenser.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/paste_dispenser.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| person_belt_shield.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_belt_shield.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/person_belt_shield.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| person_fat_duster.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_fat_duster.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/person_fat_duster.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_fat_parka.obj | Z04 / `Models/person_fat_parka.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_fat_parka.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| person_female_duster.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_female_duster.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/person_female_duster.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_female_parka.obj | Z04 / `Models/person_female_parka.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_female_parka.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| person_head.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_head.obj` | Z17 / `Models/person_head.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_hulk_duster.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_hulk_duster.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/person_hulk_duster.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_hulk_parka.obj | Z04 / `Models/person_hulk_parka.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_hulk_parka.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| person_male_duster.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_male_duster.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/person_male_duster.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_male_lower.obj | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_male_lower.obj` | Z17 / `Models/person_male_lower.obj` (different OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_male_lower.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_male_parka.obj | Z04 / `Models/person_male_parka.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_male_parka.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| person_male_upper.obj | Z16 / `IsoRim_Efficient_90_Assets/Models/shared_apparel/person_male_upper.obj` | Z17 / `Models/person_male_upper.obj` (different OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_male_upper.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_thin_duster.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/person_thin_duster.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/person_thin_duster.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| person_thin_parka.obj | Z04 / `Models/person_thin_parka.obj` | Z16 / `IsoRim_Efficient_90_Assets/Models/outerwear/person_thin_parka.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| potato_1.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/potato_1.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/potato_1.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| potato_2.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/potato_2.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/potato_2.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| sandbags_1.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/sandbags_1.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/sandbags_1.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| sandbags_2.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/sandbags_2.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/sandbags_2.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| sandbags_3.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/sandbags_3.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/sandbags_3.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| shelf.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/shelf.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/shelf.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| solar_generator.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/solar_generator.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/solar_generator.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| standing_lamp.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/standing_lamp.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/standing_lamp.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| table_2x2.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/table_2x2.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/table_2x2.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| tree_oak_1.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/tree_oak_1.obj` | Z17 / `Models/tree_oak_1.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| tree_pine_1.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/tree_pine_1.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/tree_pine_1.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| tree_pine_2.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/tree_pine_2.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/tree_pine_2.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |
| turret_gun_mini.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/turret_gun_mini.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/turret_gun_mini.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| turret_mini.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/turret_mini.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/turret_mini.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| wind_rotor.obj | Z02 / `Models/wind_rotor.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/wind_rotor.obj` (different OBJ); Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/wind_rotor.obj` (identical OBJ) | Explicit later selected runtime export / documented reuse |
| wind_turbine.obj | Z15 / `IsoRim_Economical_60_Assets/OptimizedPrevious/Models/wind_turbine.obj` | Z01 / `IsoRim_Clay_30_Trial/Models/wind_turbine.obj` (different OBJ) | Explicit later selected runtime export / documented reuse |

Four stone families deliver identical OBJ/MTL payloads for each stack stage. Granite is the shared active path for all four explicit bindings.
The import keeps semantic identities in the XML. Nine redundant stone copies do not create extra model identities.
MTL basenames never decide selection. Each distinct rewritten MTL content has a clay_<SHA-256-prefix>.mtl filename beside the OBJs.
Every material name, Kd value, transparency/emission record and dynamic {stuff} token stays intact. Texture keys use IsoRim/Clay/Swatches.
All required supplied swatches match their source bytes. The 1.7 texture namespace prevents changes to frozen root content.
No unresolved geometry-selection collision remains. Inherited contacts and missing runtime integration remain limitations, not hidden fixes.

### Material basename collisions

Both differing libraries remain available under distinct content names. Each OBJ references its selected adjacent library.

| Original basename | Source ZIP/member(s) | Source SHA-256 | Active library |
|---|---|---|---|
| isorim.mtl | `IsoRim_Clay_Batch200_Animals_121-160/IsoRim_Clay_Animals_121-160/animals_a/isorim.mtl`; `IsoRim_Clay_Batch200_Animals_161-200/IsoRim_Clay_Animals_161-200/animals_b/isorim.mtl`; `IsoRim_Economical_60_Assets/IsoRim_Economical_60_Assets/NewModels/animals/isorim.mtl`; `IsoRim_Economical_60_Assets/IsoRim_Economical_60_Assets/OptimizedPrevious/Models/isorim.mtl`; `IsoRim_Economical_60_Assets/IsoRim_Economical_60_Assets/NewModels/machines/isorim.mtl`; `IsoRim_Efficient_90_Assets/IsoRim_Efficient_90_Assets/Models/utilities/isorim.mtl`; `IsoRim_Clay_Batch200_Objects_001-030/IsoRim_Clay_Objects_001-030/props/Models/isorim.mtl`; `IsoRim_Economical_60_Assets/IsoRim_Economical_60_Assets/NewModels/organics/isorim.mtl`; `IsoRim_Batch_04_Final_Support_Models/Models/isorim.mtl`; `IsoRim_Clay_Batch200_Resources_and_Plants_031-090/IsoRim_Clay_Resources_and_Plants_031-090/resources/Models/isorim.mtl`; `IsoRim_Efficient_90_Assets/IsoRim_Efficient_90_Assets/Models/hair/isorim.mtl`; `IsoRim_Efficient_90_Assets/IsoRim_Efficient_90_Assets/Models/headgear/isorim.mtl` | `b464c37fea31d042720734b56b8d61d96897253cb0d9976be9c1c6e60e46cfb8` | `1.7/Models/clay_a722e6d79e79d3a847e8.mtl` |
| isorim.mtl | `IsoRim_Clay_Batch200_Hairstyles_091-120/IsoRim_Clay_Hairstyles_091-120/hair/isorim.mtl` | `cf6962e425177a86cec03aba39bc8d2715b5ff0c91ce40867b7d05a42aa78d92` | `1.7/Models/clay_cf6962e425177a86cec0.mtl` |
| isorim_source_materials.mtl | `IsoRim_Economical_60_Assets/IsoRim_Economical_60_Assets/OptimizedPrevious/Models/isorim_source_materials.mtl` | `22fed876f75f88e2ef1e5db6c6275136aafd984d42775ce453e46cb8ec78ce85` | `1.7/Models/clay_feaab62f41af8a3a0ba6.mtl` |
| isorim_source_materials.mtl | `IsoRim_Economical_60_Assets/IsoRim_Economical_60_Assets/NewModels/furniture/isorim_source_materials.mtl` | `77fa8f22394ad7d3bba124572188446fc8a84c6018716438192af4557984e4d0` | `1.7/Models/clay_2b4cebdd378e564a4239.mtl` |

## Missing deliveries and next modelling gaps

No listed ZIP is missing. The manifest-only ZIP contributes provenance rather than geometry.
The listed ZIP set contains no dedicated Ideology final delivery. Its remaining active original paths stay in the Original 3D table.
Batch200 plant young/mature stages need growth-stage renderer selection. Numbered random variants cannot substitute for maturity states.
The grave marker needs a standalone placement route. The cobra tail needs a segmented body and articulation to avoid a duplicate tail.
The WoodLog stages have no selected matching Clay delivery. The older item_wood export belongs to a legacy inactive identity.
The horse uses the delivered chibi hip coordinates and legLength=0.65. Standing, gait, sleeping and downed runtime poses remain untested.
Further work should start with these missing bindings and the named No authored 3D identities. Recolours do not require duplicate modelling.
Archive inspection found no separate licence or attribution payload. Existing product LICENSE, ATTRIBUTION.md and THIRD_PARTY remain the attribution record.

## Verification and reproducibility

Selection follows each archive manifest and handoff, then actual product XML and hardcoded routes. Completion checklists do not establish bindings.
Private task scripts under artifacts/clay-asset-import record the selection, import, static census and diagnostics. They are ignored task evidence.
The source census reads every XML under installed Data/{Core,Royalty,Ideology,Biotech,Anomaly,Odyssey}/Defs. It resolves ParentName inheritance.
It applies explicit thing, exact category, name-match and SmallPlants routes in product order. It includes People and HairParts shared routes.
The installed XML census excludes runtime-generated implied definitions. Its result is not a live DefDatabase completeness claim.
The final handoff reports the outcomes of the checks below. No live game/rendering check ran for this import.

- `python artifacts/clay-asset-import/check_import.py`: payload preservation, explicit paths and frozen-version scope.
- `python artifacts/clay-asset-import/check_bindings.py`: thirty hairstyle mappings and delivered horse pivots.
- `python tools/validate_mod.py`: prepared indices, finite authored normals, adjacent materials and model references.
- `python artifacts/clay-asset-import/code_checks.py`: 1.7 Release compile against installed game references.
- `python artifacts/clay-asset-import/code_checks.py --format`: scoped whitespace/style write and verify checks.

Each complete gate uses a deadline below 300 seconds. Private scripts remain available in the claimed slot for the Chief.

## Check results for the imported assets

The initial checks below ran at IsoRim@1080268d03050320d631382de828b51fb6572120. The first build checked source in isolated output.
That import omitted the matching packaged DLL. The package correction below replaces the tracked assembly with that branch source build.

| Check command | Outcome |
|---|---|
| `python artifacts/clay-asset-import/check_import.py` | Pass. 1,238 unique OBJ payloads preserve all non-mtllib records. All 1,744 changed paths stay inside the grant. Frozen root/1.5/1.6 remain untouched. |
| `python artifacts/clay-asset-import/check_bindings.py` | Pass. Static checks cover 30 named hairstyle bindings and the delivered horse hips/leg length. The initial check failed on Scatman mapping to balding. |
| `python artifacts/clay-asset-import/validate.py` | Pass, 52.55 seconds. Runs the existing tools/validate_mod.py unchanged, with verbose stdout compacted. It checks 1,238 OBJ files and four definition inputs. |
| `python artifacts/clay-asset-import/code_checks.py --format` | Pass, 17.39 seconds total. Whitespace and style write/verify checks include only Source17/HairParts.cs. |
| `python artifacts/clay-asset-import/code_checks.py` | Pass, 3.08 seconds. Installed-reference 1.7 Release compile. One NU1900 warning reports unavailable NuGet vulnerability-feed data. Zero compiler errors. |
| `git -c core.whitespace=cr-at-eol diff --check` | Pass. The existing XML CRLF line endings remain intact. |

Input accounting: 1,340 runtime candidate records resolve to 1,274 delivered identities. The selection supersedes 66 earlier candidate records.
The import activates 1,247 delivery identities through 1,238 unique OBJ paths. Fifteen selected identities reuse approved earlier exports.
Nine identical stone-stage copies share active paths. Twenty-seven delivered identities remain unmapped: 24 plant stages, the grave marker, cobra tail and legacy wood.
The byte-identical duplicate 30-trial ZIP contributes no second payload. Source/comparison geometry, rejected alternatives and procedural studies remain outside active Models.
No listed ZIP is missing. No unknown selected runtime name remains. No geometry-selection or material-name conflict remains unresolved.
No live-game, animation, collision, shader or rendered-scene check ran. Static census limitations and inherited contacts remain disclosed above.

## Packaged 1.7 assembly correction

The isolated installed-reference Release build checked source at IsoRim@8d60f7a7b6041fdf4c06de8d015b99b89dc5b6cb. The package-fix commit changes no C# source or asset inputs.
The packaged `1.7/Assemblies/IsoRim.dll` now matches `.testruns/clay-asset-import/build/bin/IsoRim.dll` byte for byte.
Both files have SHA-256 `eba30bc8d3377d10fb26d381c94393c3b41062b5d75a57e20db751a14fb7a032` and size 749,568 bytes.
All 30 delivered hairstyle literals occur in the built assembly. The frozen root and 1.5/1.6 inputs remain unchanged.

| Check command | Outcome |
|---|---|
| `python artifacts/clay-asset-import/code_checks.py` | Pass, 0.83 seconds. Incremental installed-reference 1.7 Release build from unchanged branch source. One inherited NU1900 feed warning, zero errors. |
| `python artifacts/clay-asset-import/package_fix.py` | Pass. Exact artifact copy/equality, 30 hairstyle literals, unchanged source/assets and frozen-version scope. |
| `git diff 8d60f7a7 --name-only` | Only 1.7/Assemblies/IsoRim.dll and this report change for the packaging correction. |

The earlier 52.55-second intake result still covers identical source and asset inputs. This package-only correction does not repeat intake or run the live game.
Asset totals, provenance, unmapped deliveries and census limits remain unchanged. The branch stays local and slot 2 stays claimed.

# Asset status for IsoRim 1.7

**Active OBJ paths/parts:** 1,320 Clay, 76 original. Shared aliases count once.
**No bound authored model:** 538 concrete installed XML ThingDefs across Core and all five DLCs.

## Active coverage

- All `1.7/Models/*.obj` files are active Clay overrides. Definition-owned models use explicit paths.
- People, hair and other internal parts prefer `1.7/Models/<name>.obj`, then fall back to `Models/<name>.obj`.
- The remaining active originals appear below. Root, 1.5 and 1.6 inputs remain frozen.
- Shared bodies cover human ages, builds and worn recolours. Bald, NoBeard and Stubble intentionally have no separate part.
- Walls, rock, terrain, water and ordinary roofs use procedural geometry. Gravship roofs also use authored kit parts.
- Item, food and stone models depend on graphics settings. Sprite extrusion does not count as an authored model.

## Remaining original models (76 OBJ paths)

Names below expand under `Models/` with `.obj`. Braces list exact suffix alternatives.

daylily, hops, rose, smokeleaf, agave_{1, 2}, berry_{bush_1, bush_2}, bramble_{1, 2}, bush_{1, 2, 3},
concrete_barrier, dandelion_{1, 2}, food_potato, grass_{2, 3, 4}, grave_{full, open}, haygrass_{1, 2},
item_{cloth, neurotrainer, rm_blocks_a, rm_blocks_b, rm_blocks_c, rm_logs_a, rm_logs_b, rm_logs_c, techprint},
junk_{1, 2, 3}, person_{face, fat_cape, female_cape, hair_balding, hair_buzz, hair_crest, hair_shoulder,
hair_sidebuns, hair_topbun_tail, hair_topbun_tail_bun, hair_topknot, hat_burka, hulk_cape, male_cape, pads,
thin_cape, visor}, razor_wire, round_cactus, ruin_{barrels, cannon, crate, crate_spacer, engine, lamppost,
machine, tripod}, strawberry_{1, 2}, tree_{oak_2, oak_3}, warwalker_{claw, foot, leg, torso}, wreck_{apc, car,
jeep, tank, truck}

## No bound authored model (538 XML identities)

Names below are exact ThingDefs. Braces list exact suffix alternatives.
Apparel names describe ground items here. Shared worn models can still exist.

Fallback keys: **item** = extrusion or sprite by settings, **building** = extrusion or flat floor graphic,
**pawn/plant** = game sprite, **effect** = realtime sprite/effect, **linked** = linked sprite or procedural layer,
**wall** = procedural block/rock. Procedural 3D does not count as an authored mesh.

### Core (141)

**building (66):**
Gloomlight, Snowman, Urn, Ancient{BasicRecharger, Bed, DisplayBank, DropshipEngine, FuelNode, GiantWheel,
HermeticCrate, JetEngine, KitchenSink, LargeContainer, LargeCrate, LargeMechGestator, LockerBank, LongCrate,
MechGestator, MechGestatorTank, Microwave, MilitaryCrate, OperatingTable, Oven, PodCar, Postbox, Refrigerator,
SecurityTurret, ShipBeacon, ShoppingCart, SmallCrate, StandardRecharger, StorageCylinder, Stove, SystemRack,
Toilet, VendingMachine, WarspiderRemains, WashingMachine, Wheel}, AnimalSleepingSpot, BurningPowerCell,
ButcherSpot, CaravanPackingSpot, CaveExit, Cocoon{Megascarab, Megaspider, Spelopede}, CollapsedRocks,
CraftingSpot, Crater{Large, Medium, Small}, DefoliatorShipPart, DoubleSleepingSpot, GlowPod,
InfiniteChemreactor, MarriageSpot, PartySpot, Psychic{DronerShipPart, Emanator}, SleepingSpot,
Turret_{AutoChargeBlaster, AutoInferno, AutoMiniTurret, AutoMortar}, VanometricPowerCell

**item (47):**
AIPersonaCore, Ambrosia, Dye, Novel, Schematic, Skull, Wort, Active{DropPod, DropPodMechanoid},
Apparel_{BasicShirt, CollarShirt, Duster, Jacket, Pants, Parka, Robe, TribalA}, Artillery_{AutoMortar, Mortar},
ElephantTusk, Gun_{AutocannonTurret, MiniTurret, MiniTurretMech, RocketswarmLauncher, TurretFoam, TurretSniper},
Minified{Thing, ThingRealtimeDraw, Tree}, ReinforcedBarrel, TechprofSubpersonaCore, TextBook, ThrumboHorn,
Unfinished{Apparel, Armor, Belt, Component, Gun, HealthItemBionic, HealthItemProsthetic, Mask,
MetallicTechArmor, MusicalInstrument, Pack, Sculpture, TechArmor, Weapon}

**linked (3):**
HiddenConduit, PowerConduit, WaterproofConduit

**pawn (1):**
Alphabeaver

**plant (4):**
BurnedTree, ChoppedStump, Plant_Timbershroom, SmashedStump

**wall (20):**
Barricade, Granite, Limestone, Marble, Sandstone, Slate, Wall, Mineable{ComponentsIndustrial, Gold, Jade,
Plasteel, Silver, Steel, Uranium}, RaisedRocks, Smoothed{Granite, Limestone, Marble, Sandstone, Slate}

### Royalty (19)

**building (1):**
MeditationSpot

**effect (2):**
Brazier, DarklightBrazier

**item (14):**
Apparel_{Beret, Coronet, Crown, CrownStellic, HatLadies, HatTop, PsyfocusRobe, PsyfocusShirt, PsyfocusVest},
BroadshieldCore, Melee{Weapon_MonoSwordBladelink, Weapon_PlasmaSwordBladelink, Weapon_ZeusHammerBladelink},
UnfinishedPiano

**plant (2):**
ChoppedStump_Anima, SmashedStump_Anima

### Ideology (41)

**building (19):**
Ideogram, Spacedrone, Animalist{Slab_Broad, Slab_Medium}, ArchonexusCore, ArchotechTower,
GrandArchotechStructure, Major{ArchotechStructure, ArchotechStructureStudiable}, Morbid{Slab_Broad,
Slab_Medium}, RitualSpot, Rustic{Rug_Broad, Rug_Medium}, SculptureTerror, Spikecore{FloorStar_Broad,
FloorStar_Medium}, Totemic{Slab_Broad, Slab_Medium}

**effect (1):**
Darktorch

**item (11):**
Apparel_{BodyStrap, Burka}, GauranlenSeed, Relic{InertArk, InertBox, InertCube, InertCup, InertFragment,
InertPendant, InertSwordHandle, InertTablet}

**pawn (8):**
Dryad_{Barkskin, Basic, Berrymaker, Carrier, Clawer, Gaumaker, Medicinemaker, Woodmaker}

**plant (2):**
ChoppedStump_Gauranlen, SmashedStump_Gauranlen

### Biotech (54)

**building (7):**
Ancient{BandNode, ExostriderCannon, ExostriderHead, ExostriderLeg, ExostriderRemains, ToxifierGenerator},
BabySleepingSpot

**effect (3):**
SanguphageMeetingTorch, Shuttle{Crashed_Exitable, Crashed_Exitable_Mechanitor}

**item (33):**
Genepack, Mechlink, Wastepack, Xenogerm, Apparel_{Bandolier, HeavyShield, KidPants, KidParka, KidRomper,
KidShirt, KidTribal, Sash}, ArchiteCapsule, BabyFood, Control{Sublink, SublinkHigh}, Gun_ChargeBlasterTurret,
HemogenPack, Human{Embryo, Ovum}, MechFormfeeder, MechanoidTransponder, NanostructuringChip, PoluxSeed,
PowerfocusChip, Remote{Repairer, Shielder}, RepairProbe, SignalChip, Subcore{Basic, High, Regular},
UnfinishedSubcore

**pawn (2):**
Toxalope, WasteRat

**plant (9):**
Chopped{Stump_Polluted, Stump_Polux}, Plant_{PebbleCactus, RatPalm, TreeGrayPine, TreePolux, Witchwood},
Smashed{Stump_Polluted, Stump_Polux}

### Anomaly (73)

**building (31):**
Noctolith, AtmosphericHeater, DirtCubeSculpture, FleshSack, Floor{Etching, EtchingRambling}, FrenzyInducer,
Gray{Box, StatueDeadlifeDust, StatueTeleporter}, Pit{Burrow, Gate, GateExit}, PsychicRitualSpot,
SandCubeSculpture, ScrapCubeSculpture, ShardInhibitor, StoneCubeSculpture, Turret_TacticalTurret,
Twisted{ArchotechSupport_Large, ArchotechSupport_Small, ArchotechSupport_Vertical}, Void{Monolith,
MonolithAttachmentBottomLeft, MonolithAttachmentBottomRight, MonolithAttachmentLeft, MonolithAttachmentRight,
MonolithAttachmentTop, Sculpture, Structure}, WarpedObelisk_Labyrinth

**effect (17):**
Fleshbulb, Endless{Pit2x2c, Pit3x2c, Pit3x3c}, Fleshmass{Heart, Spitter}, HoldingSpot, NerveBundle,
SecurityDoor, ShardBeacon, VoidNode, Voidmetal{MassLarge, MassMedium, MassSmall}, Warped{Obelisk_Abductor,
Obelisk_Duplicator, Obelisk_Mutator}

**item (18):**
Shard, Tome, AdrenalHeart, Apparel_LabCoat, CorrosiveHeart, FleshmassNeuralLump, Ghoul{Barbs, Plating},
GoldenCube, GrayFleshSample, Gun_TacticalTurret, HarbingerSeed, LabyrinthMatter, MetalbloodHeart,
MonolithFragment, Revenant{FleshChunk, Spine}, UnfinishedHealthItemGhoul

**linked (2):**
Fleshmass, Fleshmass_Active

**plant (3):**
ChoppedStump_Harbinger, Plant_TreeHarbinger, SmashedStump_Harbinger

**wall (2):**
GrayWall, VoidmetalWall

### Odyssey (210)

**building (104):**
Ancient{AmmoStack, Antenna, Armchair, AutocannonTurret, BarrierLong, Bathtub, Box_ComponentIndustrial,
Box_ComponentSpacer, Box_SteelSlag, CardboardBox, Chembarrel, ChembarrelPallet, ChemfuelCanister, ColdStorage,
CraneArm, CraneArmSmall, CraneBase, CraneColumn, DestroyedConsole, DestroyedConsoleLarge, DoubleBed,
DrillPlatform, EmergencyLight_Blue, EmergencyLight_Green, EmergencyLight_Red, Excavator, ExplosivesCrate,
FilingCabinet, Forklift, GravEngine, GravReactor, IndustrialShelf, Lockers, MediumContainer, MetalCrate,
MilitaryBarrier, MilitaryGeneratorSmall, MiningCharge, Mortar, OpenContainer, Pallet_SteelSlag, PlantPot, Safe,
SealedContainer, SealedContainer_GravshipUpgrade, SealedCrate, SealedCrate_Gravlite, SealedCrate_Shells,
SecurityTerminal, Shelf, Shower, SimpleResearchBench, SingleBed, SmallContainer, SmallShelf, SmokeVent, TV,
TableBrewery, TableButcher, TableDrugLab, TableMachining, TableSmithingElectric, TableSmithingFueled,
TableStoveElectric, TableStoveFueled, TableTailorElectric, TableTailorHand, Terraformer, ToxVent, TransportPod,
TunnelerClaw, TunnelerHusk, Uplink, UraniumSlugTurret, WindTurbineBody, WoodenCrate}, AnticraftBeam,
Cerebrex{Core, Core_Destroyed, Stabilizer}, CocoonLocust, EggSac, FuelOptimizer, GaussCannon, GeothermalVent,
GravAnchor, GravcorePowerCell, GravshipShieldGenerator, InsectLairEntrance, LifeSupportUnit,
MalfunctioningTransportPod, Mech{Pylon, Relay_Crashed, Stabilizer}, Mechanoid{DropPod, Processor},
MechhiveAssembler, Pallet_{Medicine, SurvivalMeals}, PilotSubpersonaCore, SealedCrate, ShipChunk_Mech,
SurveyScanner, Turret_AncientArmoredTurret

**effect (7):**
Statue, AncientHeatVent, GravEngine, HunterDroneTrap, MechRelay, PassengerShuttle, WaspDroneTrap

**item (37):**
Gravcore, Map, ActiveDropPodAncient, AlphaThrumboHorn, ArcheanSeed, BloodWarmer, Bow_Great_Unique, Fish_{Bass,
Bluefish, Catfish, Cod, Dogfish, Flounder, Frostfish, Guppy, Marlin, Piranha, Salmon, Tilapia, Toxfish, Tuna},
Gun_{AncientArmoredTurret, AssaultRifle_Unique, BeamRepeater_Unique, BoltActionRifle_Unique,
ChainShotgun_Unique, ChargeLance_Unique, ChargeRifle_Unique, HeavySMG_Unique, HellcatRifle_Unique, LMG_Unique,
Minigun_Unique, Revolver_Unique, SniperRifle_Unique}, MastodonTusk, MinifiedGravEngine, ShuttleEngine

**pawn (48):**
Alligator, Armadillo, Badger, Bluebird, Bullfrog, Crow, Flamingo, Gorilla, Heron, Hippo, Larva, Locust, Macaw,
Mastodon, Megavole, Mink, Moose, Muskox, Otter, Panda, Peacock, Penguin, Porcupine, Quail, Seal, Sparrow, Swan,
Tiger, Vulture, Walrus, Wolverine, AlphaThrumbo, BogHound, Cat_Scimitar, ColossusToad, Drone_{Hunter, Sentry,
Wasp}, HermitCrab, HiveQueen, LavaSnail, Mech_Cyclops, MonitorLizard, PrairieDog, Sea{Lion, Turtle}, StoneCrab,
Wolf_Great

**plant (5):**
ChoppedStump_Archean, Plant_{TreeArchean, TreeCocoa_Wild, TreeSnagroot}, SmashedStump_Archean

**wall (9):**
Vacstone, AncientFortifiedWall, BurrowWall, GravshipHull, MineableObsidian, OrbitalAncientFortifiedWall,
Smoothed{SolidIce, Vacstone}, SolidIce

## Delivered but unmapped (27 named meshes)

- Growth-stage selection needed: `{agave,berry_bush,bramble,bush,dandelion,daylily,haygrass,hops,rose,round_cactus,smokeleaf,strawberry}_{young,mature}.obj` (24).
- `grave_marker.obj`: needs standalone placement and binding.
- `animal_cobra_tail.obj`: needs segmented-body articulation.
- `item_wood.obj`: legacy identity. Active WoodLog uses `item_rm_logs_{a,b,c}.obj` and lacks matching Clay stages.
- Procedural wall/soil/floor/water/ordinary-roof studies need generator integration. They are not active Clay.

## Verification limits

Static bindings, materials, dependencies and mesh data pass intake checks. The prepared 1.7 importer uses authored normals.
The XML census resolves inheritance but excludes game patches and runtime-generated definitions. No live-game visual, animation, collision or shader check ran.

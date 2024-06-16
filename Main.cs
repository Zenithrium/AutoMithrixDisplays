using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using HarmonyLib;
using System.Linq;
using UnityEngine.AddressableAssets;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace AutoMithrixDisplays {
    [BepInPlugin(ModGuid, ModName, ModVer)]
    //[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]

    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api.content_management", BepInDependency.DependencyFlags.HardDependency)]

    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class AutoMithrixDisplays : BaseUnityPlugin{
        public const string ModGuid = "com.Zenithrium.AutoMithrixDisplays";
        public const string ModName = "AutoMithrixDisplays";
        public const string ModVer = "1.0.0";

        //Provides a direct access to this plugin's logger for use in any of your other classes.
        public static BepInEx.Logging.ManualLogSource ModLogger;

        public static ConfigEntry<ulong> seed;
        public static ConfigEntry<string> idsToIgnore;

        public static AssetBundle MainAssets;

        public GameObject t1Infect;
        public GameObject t2Infect;
        public GameObject t3Infect;
        public GameObject bsInfect;
        public GameObject luInfect;

        public GameObject v1Infect;
        public GameObject v2Infect;
        public GameObject v3Infect;
        public GameObject v4Infect;

        public GameObject syblInfect;
        public GameObject sacrInfect;
        public GameObject vdluInfect;

        ItemTier syblTier;
        ItemTier sacrTier;
        ItemTier vdluTier;

        private void Awake(){
            //minCooldown = Config.Bind<float>("General", "Minimum Cooldown", 0f, "Adjust the minimum cooldown between skill activations. You probably want this to be 0, but you can make it any other number if you're in to that. For reference, the base game value is .5 seconds.");
            seed = Config.Bind<ulong>("General", "Placement Seed", 2147483520, "Adjust what seed the random number generator uses for item placement. (Ex: 2147483520, 7, 3691215, etc)");
            idsToIgnore = Config.Bind<string>("General", "Ignored Content Packs", "", "Adjust which mods do not get item displays added for them, in the event they already have them, separated by commas. (Ex. \"Aetherium, Starstorm\" or \"com.ContactLight.LostInTransit, com.Hex3.Hex3Mod\"");

            ModLogger = Logger;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AutoMithrixDisplays.automithrixdisplaysassets")){
                MainAssets = AssetBundle.LoadFromStream(stream);
            }
            Swapallshaders(MainAssets);

            On.RoR2.ItemCatalog.Init += ItemCatalog_Init;
        }

        private void ItemCatalog_Init(On.RoR2.ItemCatalog.orig_Init orig){
            orig();

            t1Infect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/ItemInfection, White.prefab").WaitForCompletion();
            t2Infect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/ItemInfection, Green.prefab").WaitForCompletion();
            t3Infect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/ItemInfection, Red.prefab").WaitForCompletion();
            luInfect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/ItemInfection, Blue.prefab").WaitForCompletion();

            v1Infect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("void1Infection.prefab");
            var v1dis = v1Infect.AddComponent<ItemDisplay>();
            v1dis.rendererInfos = new CharacterModel.RendererInfo[1];
            v1dis.rendererInfos[0].renderer = v1Infect.GetComponent<MeshRenderer>();
            v1dis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("voidInfectionT1.mat");

            v2Infect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("void2Infection.prefab");
            var v2dis = v2Infect.AddComponent<ItemDisplay>();
            v2dis.rendererInfos = new CharacterModel.RendererInfo[1];
            v2dis.rendererInfos[0].renderer = v2Infect.GetComponent<MeshRenderer>();
            v2dis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("voidInfectionT2.mat");

            v3Infect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("void3Infection.prefab");
            var v3dis = v3Infect.AddComponent<ItemDisplay>();
            v3dis.rendererInfos = new CharacterModel.RendererInfo[1];
            v3dis.rendererInfos[0].renderer = v3Infect.GetComponent<MeshRenderer>();
            v3dis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("voidInfectionT3.mat");

            v4Infect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("void3Infection.prefab");
            var v4dis = v4Infect.AddComponent<ItemDisplay>();
            v4dis.rendererInfos = new CharacterModel.RendererInfo[1];
            v4dis.rendererInfos[0].renderer = v4Infect.GetComponent<MeshRenderer>();
            v4dis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("voidInfectionT4.mat");

            bsInfect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("t4Infection.prefab");
            var bsdis = bsInfect.AddComponent<ItemDisplay>();
            bsdis.rendererInfos = new CharacterModel.RendererInfo[1];
            bsdis.rendererInfos[0].renderer = bsInfect.GetComponent<MeshRenderer>();
            bsdis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("InfectionT4.mat");


            syblInfect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("syblInfect.prefab");
            var sybdis = syblInfect.AddComponent<ItemDisplay>();
            sybdis.rendererInfos = new CharacterModel.RendererInfo[1];
            sybdis.rendererInfos[0].renderer = syblInfect.GetComponent<MeshRenderer>();
            sybdis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("syblInfectMat.mat");

            sacrInfect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("sacrInfect.prefab");
            var sacdis = sacrInfect.AddComponent<ItemDisplay>();
            sacdis.rendererInfos = new CharacterModel.RendererInfo[1];
            sacdis.rendererInfos[0].renderer = sacrInfect.GetComponent<MeshRenderer>();
            sacdis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("sacriInfectMat.mat");

            vdluInfect = AutoMithrixDisplays.MainAssets.LoadAsset<GameObject>("vdluInfect.prefab");
            var vludis = vdluInfect.AddComponent<ItemDisplay>();
            vludis.rendererInfos = new CharacterModel.RendererInfo[1];
            vludis.rendererInfos[0].renderer = vdluInfect.GetComponent<MeshRenderer>();
            vludis.rendererInfos[0].defaultMaterial = AutoMithrixDisplays.MainAssets.LoadAsset<Material>("vdluInfectMat.mat");

            string[] bones = { "chest", "Stomach", "Head", "UpperArmL", "LowerArmL", "UpperArmR", "LowerArmR", "ThighL", "ThighR", "CalfL", "CalfR", "chest", "Stomach", "chest" };
            //                    0         1         2         3            4             5           6          7          8        9        10       11        12        13
            Xoroshiro128Plus mithRand = new Xoroshiro128Plus(seed.Value);

            var j = ItemTierCatalog.allItemTierDefs;
            foreach(var j2 in j)
            {
                Debug.Log("j2: " + j2.name + " | " + j2.tier + " | " + j2.ToString()); 
            }
            //Debug.Log("");

            syblTier = ItemTierCatalog.FindTierDef("Sibylline").tier;
            sacrTier = ItemTierCatalog.FindTierDef("Sacrificial").tier;
            vdluTier = ItemTierCatalog.FindTierDef("VoidLunarTierDef").tier;

            var idrs = Addressables.LoadAssetAsync<ItemDisplayRuleSet>("RoR2/Base/Brother/idrsBrother.asset").WaitForCompletion();
            ItemDisplayRuleSet.KeyAssetRuleGroup[] itemRuleGroups = idrs.keyAssetRuleGroups;

            var contentPacks = ContentManager.allLoadedContentPacks;

            IList<string> ignoreList = idsToIgnore.Value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pack in contentPacks){
                var matches = ignoreList.Where(item => pack.identifier.Contains(item)).ToList();
                Debug.Log("List of Finds: " + matches.Count + " | " + matches.ToString());
                if (matches.Count > 0){
                    Debug.Log("Skipping " + pack.identifier);
                    continue;
                }

                if (pack.identifier == "RoR2.BaseContent"){
                    if (pack.itemDefs.Length != 0){
                        Debug.Log("Adding " + pack.identifier + " boss items");
                        itemRuleGroups = AddAllDisplays(pack.itemDefs, itemRuleGroups, mithRand, bones, ItemTier.Boss);
                    }
                }else{
                    if (pack.itemDefs.Length != 0){
                        Debug.Log("Adding " + pack.identifier + "'s items");
                        itemRuleGroups = AddAllDisplays(pack.itemDefs, itemRuleGroups, mithRand, bones, ItemTier.NoTier);
                    }
                }
            }
            idrs.keyAssetRuleGroups = itemRuleGroups;
        }

        (Vector3, Vector3) GeneratePositionFromRand(Xoroshiro128Plus rand, int randVal)
        {
            //string[] bones = { "chest", "Stomach", "Head", "UpperArmL", "LowerArmL", "UpperArmR", "LowerArmR", "ThighL", "ThighR", "CalfL", "CalfR", "chest", "Stomach" };
            //                      0         1         2         3           4              5           6          7          8        9        10       11        12

            Vector3 pos;
            Vector3 root = Vector3.zero;
            Vector3 modifier = Vector3.zero;
            if (randVal == 0 || randVal == 11)
            { //chest   -----------------VVV was .1f and a lot of other .085fs were 
                pos = new Vector3(rand.nextBool ? rand.RangeFloat(-0.125f, -0.045f) : rand.RangeFloat(0.045f, 0.125f), rand.RangeFloat(-.2f, 0.325f), rand.nextBool ? rand.RangeFloat(-0.125f, -0.085f) : rand.RangeFloat(0.085f, 0.125f));
                root = new Vector3(-0.00957f, 0.20225f, -0.023202f);

                modifier = new Vector3(0, 80, 330);

            }
            else if (randVal == 1 || randVal == 12)
            { //stomach

                pos = new Vector3(rand.nextBool ? rand.RangeFloat(-0.1275f, -0.065f) : rand.RangeFloat(0.065f, 0.1275f), rand.RangeFloat(0.125f, 0.325f), rand.nextBool ? rand.RangeFloat(-0.125f, -0.085f) : rand.RangeFloat(0.085f, 0.125f));

                root = new Vector3(-0.03386f, 0.24174f, 0.06423f);
                modifier = new Vector3(0, 90, 0);

            }
            else if (randVal == 2)
            { //head                                           
                pos = new Vector3(rand.RangeFloat(-0.075f, 0.075f), rand.RangeFloat(-.03f, .0675f), rand.RangeFloat(-0.075f, .075f));

            }
            else if (randVal >= 3 && randVal <= 6)
            { // arms
                pos = new Vector3(rand.RangeFloat(-0.0065f, 0.0065f), rand.RangeFloat(.075f, .3f), rand.RangeFloat(0.005f, 0.01f));

            }
            else if (randVal == 7 || randVal == 8)
            { //thigh
                pos = new Vector3(rand.RangeFloat(-0.021f, 0.029f), rand.RangeFloat(.0375f, .325f), 0);

            }
            else if (randVal == 9 || randVal == 10)
            { //calves
                var bean = (rand.RangeFloat(-0.025f, 0.045f), rand.RangeFloat(0, .3675f), rand.RangeFloat(.02f, 0.04f));
                Debug.Log("bean: " + bean.Item1 + " | " + bean.Item2 + " | " + bean.Item3);
                pos = new Vector3(bean.Item1, bean.Item2, bean.Item3);
                root = new Vector3(.01f, pos.y, 0.03f);

            }
            else
            { //specifically boob region
                pos = new Vector3(rand.nextBool ? rand.RangeFloat(-0.115f, -0.065f) : rand.RangeFloat(0.065f, 0.115f), rand.RangeFloat(0.12f, 0.325f), rand.nextBool ? rand.RangeFloat(-.05f, -0.035f) : rand.RangeFloat(-.005f, .01f));
                root = new Vector3(-0.00957f, 0.20225f, -0.025f);

                modifier = new Vector3(341.1971f, 90, 27f);

            }
            root.y = pos.y;
            Vector3 between = pos - root;

            Vector3 rot = Quaternion.LookRotation(between).eulerAngles;
            rot += modifier;

            return (pos, rot);
        }

        ItemDisplayRuleSet.KeyAssetRuleGroup[] AddAllDisplays(ReadOnlyNamedAssetCollection<ItemDef> items, ItemDisplayRuleSet.KeyAssetRuleGroup[] itemRuleGroups, Xoroshiro128Plus mithRand, string[] bones, ItemTier filter){
            foreach (var item in items){

                if ((filter == ItemTier.NoTier && item.tier != ItemTier.NoTier) || (item.tier == filter && filter != ItemTier.NoTier)){
                    Debug.Log("item: " + item.nameToken + "| " + item.tier);
                    ItemDisplayRuleSet.KeyAssetRuleGroup a;
                    a.keyAsset = item;

                    var rand = mithRand.RangeInt(0, bones.Length);
                    var pos = GeneratePositionFromRand(mithRand, rand);
                    GameObject selection;
                    switch (item.tier){
                        case ItemTier.Tier1:
                            selection = t1Infect;
                            break;
                        case ItemTier.Tier2:
                            selection = t2Infect;
                            break;
                        case ItemTier.Tier3:
                            selection = t3Infect;
                            break;
                        case ItemTier.Boss:
                            selection = bsInfect;
                            break;
                        case ItemTier.Lunar:
                            selection = luInfect;
                            break;
                        case ItemTier.VoidTier1:
                            selection = t1Infect;
                            break;
                        case ItemTier.VoidTier2:
                            selection = t2Infect;
                            break;
                        case ItemTier.VoidTier3:
                            selection = t3Infect;
                            break;
                        case ItemTier.VoidBoss:
                            selection = bsInfect;
                            break;
                        default:
                            if (item.tier == syblTier){
                                selection = syblInfect;
                            }else if (item.tier == sacrTier){
                                selection = sacrInfect;
                            }else if(item.tier == vdluTier){
                                selection = vdluInfect;
                            }else{
                                selection = t1Infect;
                            }
                            break;
                    }

                    a.displayRuleGroup = new DisplayRuleGroup();
                    a.displayRuleGroup.AddDisplayRule(new RoR2.ItemDisplayRule{
                        ruleType = ItemDisplayRuleType.ParentedPrefab,
                        followerPrefab = selection,
                        childName = bones[rand],
                        localPos = pos.Item1,
                        localAngles = pos.Item2,
                        localScale = new Vector3(.11f, .11f, .11f)
                    });
                    itemRuleGroups = itemRuleGroups.AddItem(a).ToArray();
                }
            }
            return itemRuleGroups;
        }

        public void Swapallshaders(AssetBundle bundle){
            Material[] allMaterials = bundle.LoadAllAssets<Material>();
            foreach (Material mat in allMaterials){
                switch (mat.shader.name){
                    case "Stubbed Hopoo Games/Deferred/Standard":
                        mat.shader = Resources.Load<Shader>("shaders/deferred/hgstandard");
                        break;

                    case "Stubbed Hopoo Games/Deferred/Snow Topped":
                        mat.shader = Resources.Load<Shader>("shaders/deferred/hgsnowtopped");
                        break;

                    case "Stubbed Hopoo Games/FX/Cloud Remap":
                        mat.shader = Resources.Load<Shader>("shaders/fx/hgcloudremap");
                        break;

                    case "Stubbed Hopoo Games/FX/Cloud Intersection Remap":
                        mat.shader = Resources.Load<Shader>("shaders/fx/hgintersectioncloudremap");
                        break;

                    case "Stubbed Hopoo Games/FX/Opaque Cloud Remap":
                        mat.shader = Resources.Load<Shader>("shaders/fx/hgopaquecloudremap");
                        break;

                    case "Stubbed Hopoo Games/FX/Distortion":
                        mat.shader = Resources.Load<Shader>("shaders/fx/hgdistortion");
                        break;

                    case "Stubbed Hopoo Games/FX/Solid Parallax":
                        mat.shader = Resources.Load<Shader>("shaders/fx/hgsolidparallax");
                        break;

                    case "Stubbed Hopoo Games/Environment/Distant Water":
                        mat.shader = Resources.Load<Shader>("shaders/environment/hgdistantwater");
                        break;

                    case "StubbedRoR2/Base/Shaders/HGCloudRemap":
                        mat.shader = Resources.Load<Shader>("shaders/fx/hgcloudremap");
                        break;

                    default:
                        break;
                }

            }
        }

    }
}

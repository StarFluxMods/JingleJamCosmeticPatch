using System.Collections.Generic;
using KitchenLib;
using KitchenLib.Logging;
using KitchenMods;
using System.Reflection;
using HarmonyLib;
using Kitchen;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenJingleJamCosmeticPatch
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.jjcosmeticpatch";
        public const string MOD_NAME = "JingleJam Cosmetic Patch";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.8";

        public static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Logger = InitLogger();
        }
    }
    
    [HarmonyPatch(typeof(PlayerCosmeticSubview), "SetHatVisibility")]
    public class PlayerCosmeticSubview_Patch
    {
        private static FieldInfo _HideHeadBones = ReflectionUtils.GetField<PlayerCosmeticSubview>("HideHeadBones");
        static void Prefix(PlayerCosmeticSubview __instance)
        {
            List<Transform> HideHeadBones = (List<Transform>)_HideHeadBones.GetValue(__instance);
            if (HideHeadBones.Count == 0)
            {
                GameObject HeadBone = GameObjectUtils.GetChildObject(__instance.gameObject.transform.parent.gameObject, "MorphmanPlus/Armature/LowerSpine/Spine/UpperSpine/Neck/Head");
                HideHeadBones.Add(HeadBone.transform);
                _HideHeadBones.SetValue(__instance, HideHeadBones);
            }
        }
    }
}


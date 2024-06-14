﻿using HarmonyLib;
using NeosModLoader;
using FrooxEngine;

namespace MaybeJump
{
    public class MaybeJump : NeosMod
    {
        public static ModConfiguration Config;

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableLeft = new ModConfigurationKey<bool>("EnableLeft", "Enable left jump.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableRight = new ModConfigurationKey<bool>("EnableRight", "Enable right jump.", () => true);

        public override string Author => "Banane9";
        public override string Link => "https://github.com/Ryn-Fox/ResoniteMaybeJump/";
        public override string Name => "MaybeJump";
        public override string Version => "1.1.0";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony($"{Author}.{Name}");
            Config = GetConfiguration();
            Config.Save(true);
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(DualControllerBindingGenerator))]
        private class NoLeftJumpPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("BindJump")]
            private static bool BindJumpPrefix(InputGroup group, IDualBindingController left, IDualBindingController right, ref AnyInput __result)
            {
                AnyInput anyInput = new AnyInput();

                if (Config.GetValue(EnableLeft))
                    left?.BindNodeActions(group, anyInput, "Jump");

                if (Config.GetValue(EnableRight))
                    right?.BindNodeActions(group, anyInput, "Jump");

                __result = anyInput;
                return false;
            }
        }
    }
}

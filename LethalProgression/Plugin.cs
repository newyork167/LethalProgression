﻿using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using System.Reflection;
using System.IO;
using UnityEngine.SceneManagement;
using BepInEx.Bootstrap;
using LethalProgression.GUI;
using LethalProgression.Skills;
using LethalProgression.Patches;
using LethalProgression.Config;

namespace LethalProgression
{
    [BepInPlugin("Stoneman.LethalProgression", "Lethal Progression", "1.2.0")]
    internal class LethalPlugin : BaseUnityPlugin
    {
        private const string modGUID = "Stoneman.LethalProgression";
        private const string modName = "Lethal Progression";
        private const string modVersion = "1.2.0";
        private const string modAuthor = "Stoneman";

        // Make a public AssetBundle
        public static AssetBundle skillBundle;

        internal static ManualLogSource Log;
        internal static bool ReservedSlots;

        public static LethalPlugin Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            var harmony = new Harmony(modGUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            skillBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "skillmenu"));

            Log = Logger;

            Log.LogInfo("Lethal Progression loaded.");

            foreach (var plugin in Chainloader.PluginInfos)
            {
                if (plugin.Value.Metadata.GUID.IndexOf("ReservedItem") >= 0)
                {
                    ReservedSlots = true;
                }
            }

            // Network patcher!
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            SceneManager.sceneLoaded += LethalProgression.XPHandler.ClientConnectInitializer;

            SkillConfig.InitConfig();
        }

        public void BindConfig<T>(ref ConfigEntry<T> config, string section, string key, T defaultValue, string description = "")
        {
            config = Config.Bind(section,
                key,
                defaultValue,
                description
            );
        }
    }
}

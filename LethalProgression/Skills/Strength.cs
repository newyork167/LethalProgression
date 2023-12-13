using System;
using System.Collections.Generic;
using System.Text;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace LethalProgression.Skills
{
    internal class Strength
    {
        public static void StrengthUpdate(int updatedValue, int newStrength)
        {
            Skill skill = LP_NetworkManager.xpInstance.skillList.skills[UpgradeType.Strength];
            PlayerControllerB localPlayer = GameNetworkManager.Instance.localPlayerController;
            
            // 1 level adds 2% more to 5. So that is equals to 5 * 1.02.
            float addedStrength = (updatedValue * skill.GetMultiplier() / 100f) * 11f;
            localPlayer.sprintTime += addedStrength;
            LethalPlugin.Log.LogInfo($"{updatedValue} change, {newStrength} new strength points, Adding {addedStrength} resulting in {localPlayer.sprintTime} strength");
            
            GameObject ship = GameObject.Find("/Environment/HangarShip");
            Console.WriteLine($"Ship position: {ship.transform.position}");
            foreach (var component in ship.gameObject.GetComponents(typeof(Component)))
            {
                Console.WriteLine($"Component: {component}");
            }

            var skillsObject = GameObject.Find("Systems/UI/Canvas/QuickMenu/MainButtons/Skills");
            if (skillsObject)
            {
                var position = skillsObject.transform.position;
                LethalPlugin.Log.LogInfo($"skillsObject.transform = {position.ToString()}");
                LethalPlugin.Log.LogInfo("Dropping transform.y by -.01");
                position = new Vector3(position.x, position.y - .01f, position.z);
                skillsObject.transform.position = position;
            }
        }
    }
}

using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LethalProgression.Patches
{
    [HarmonyPatch]
    internal class HangarShipDoorPatch
    {
        private static bool fired = false;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HangarShipDoor), "Update")]
        private static void Update(HangarShipDoor __instance)
        {
            // LethalFreedom: Allow players to open doors in space, sucks out all players and furniture
            var startOfRound = GameObject.FindObjectOfType<StartOfRound>();
            
            __instance.doorPowerDisplay.text = string.Format("FF%");
            switch (__instance.doorPower)
            {
                case float n when n < 0.25f:
                    __instance.doorPowerDisplay.color = Color.red;
                    break;
                case float n when n < 0.5f:
                    __instance.doorPowerDisplay.color = Color.yellow;
                    break;
                case float n when n < 0.75f:
                    __instance.doorPowerDisplay.color = Color.green;
                    break;
                default:
                    __instance.doorPowerDisplay.color = Color.green;
                    break;
            }
            
            __instance.SetDoorButtonsEnabled(true);
            __instance.triggerScript.interactable = true;
            __instance.SetDoorOpen();

            if (!fired && __instance.overheated && startOfRound.inShipPhase)
            {
                fired = true;
                LethalPlugin.Log.LogInfo("Firing players out of ship");
                
                startOfRound.suckingPlayersOutOfShip = true;
                startOfRound.suckingFurnitureOutOfShip = true;
                startOfRound.firingPlayersCutsceneRunning = true;
                
                PlaceableShipObject[] array = GameObject.FindObjectsOfType<PlaceableShipObject>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].parentObject == null)
                    {
                        Debug.Log("Error! No parentObject for placeable object: " + startOfRound.unlockablesList.unlockables[array[i].unlockableID].unlockableName);
                    }
                    array[i].parentObject.StartSuckingOutOfShip();
                }
                // StartOfRound.Instance.FirePlayersAfterDeadlineClientRpc(null, true);
            }
        }
    }
}

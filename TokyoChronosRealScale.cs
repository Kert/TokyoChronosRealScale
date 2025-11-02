using System;
using System.Reflection;
using UnityEngine;
using BepInEx;

namespace TokyoChronosRealScale
{
    [BepInPlugin("com.kert.tokyochronosrealscale", "TokyoChronosRealScale", "1.0.0")]
    public partial class TokyoChronosRealScale : BaseUnityPlugin
    {
        const float WORLD_SCALE = 3.0f; // Real life floor is synced perfectly with VR when used with this scale
        // used to get data from private fields with reflection
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            try
            {
                // hooks
                On.TkChronos.PlayAreaManager.InitMover += PlayAreaManager_InitMover;
                On.TkChronos.PlayAreaManager.AttachTrackingObjects += PlayAreaManager_AttachTrackingObjects;                
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to initialize");
                Logger.LogError(e);
                throw;
            }
        }

        void PlayAreaManager_InitMover(On.TkChronos.PlayAreaManager.orig_InitMover orig, TkChronos.PlayAreaManager self)
        {
            self.VRAreaTransform.localScale = new Vector3(WORLD_SCALE, WORLD_SCALE, WORLD_SCALE);
            Logger.LogInfo("Real world scale set");
            orig(self);
        }

        void PlayAreaManager_AttachTrackingObjects(On.TkChronos.PlayAreaManager.orig_AttachTrackingObjects orig, TkChronos.PlayAreaManager self)
        {
            TkChronos.TrackingObjectComponents component = (TkChronos.TrackingObjectComponents)typeof(TkChronos.PlayAreaManager).GetField("userTrackComponent", bindingFlags).GetValue(self);
            float s = 1.0f / WORLD_SCALE;
            component.rightHandComponent.transform.localScale = new Vector3(s, s, s);
            Logger.LogInfo("Laser pointer scale set");
            orig(self);
        }
    }
}

using System;
using System.Reflection;
using UnityEngine;
using BepInEx;

namespace TokyoChronosRealScale
{
    [BepInPlugin("com.kert.tokyochronosrealscale", "TokyoChronosRealScale", "1.0.1")]
    public partial class TokyoChronosRealScale : BaseUnityPlugin
    {
        const float WORLD_SCALE = 3.0f; // Real life floor is synced perfectly with VR when used with this scale
        const float CLOSEUP_SCENE_SCALE = WORLD_SCALE / 2f;
        const float BODY_SCALE = WORLD_SCALE / 1.2f;
        Transform world = null;
        Transform body = null;
        // used to fix laser pointer size
        Transform rightController = null;
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
                On.TkChronos.PlayAreaManager.SetLayerMode += PlayAreaManager_SetLayerMode;
                On.TkChronos.PlayerAvater.Initialize += PlayerAvater_Initialize;
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
            world = self.VRAreaTransform;
            orig(self);
        }

        void PlayAreaManager_AttachTrackingObjects(On.TkChronos.PlayAreaManager.orig_AttachTrackingObjects orig, TkChronos.PlayAreaManager self)
        {
            TkChronos.TrackingObjectComponents component = (TkChronos.TrackingObjectComponents)typeof(TkChronos.PlayAreaManager).GetField("userTrackComponent", bindingFlags).GetValue(self);
            rightController = component.rightHandComponent.transform;
            orig(self);
        }

        void PlayAreaManager_SetLayerMode(On.TkChronos.PlayAreaManager.orig_SetLayerMode orig, TkChronos.PlayAreaManager self, TkChronos.PlayAreaManager.LayerModes mode)
        {
            if(mode == TkChronos.PlayAreaManager.LayerModes.BokeSphere)
            {
                SetScale(CLOSEUP_SCENE_SCALE);
                body.localScale = new Vector3(BODY_SCALE / 2f, BODY_SCALE / 2f, BODY_SCALE / 2f);
            }
            else
            {
                SetScale(WORLD_SCALE);
                if(body)
                    body.localScale = new Vector3(BODY_SCALE, BODY_SCALE, BODY_SCALE);
            }
            orig(self, mode);
        }

        void PlayerAvater_Initialize(On.TkChronos.PlayerAvater.orig_Initialize orig, TkChronos.PlayerAvater self)
        {
            body = self.transform;
            orig(self);
        }

        void SetScale(float scale)
        {
            Logger.LogInfo("Game Scale set to " + scale.ToString());
            world.localScale = new Vector3(scale, scale, scale);
            float s = 1.0f / scale;
            rightController.localScale = new Vector3(s, s, s);
        }
    }
}

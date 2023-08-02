using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CameraBoard
{
    [BepInPlugin("pl2w.cameraboard", "CameraBoard", "1.0.0")]
    public class ModLoader : BaseUnityPlugin
    {
        public static ModLoader instance;
        GameObject ui;
        Harmony harmony;
        void OnEnable()
        {
            instance = this;

            harmony = new Harmony("pl2w.cameraboard");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if(ui) ui.SetActive(true);
        }

        void OnDisable()
        {
            harmony.UnpatchSelf();
            ui.SetActive(false);
        }

        public void LoadMod()
        {
            AssetBundle watchBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("CameraBoard.Resources.playerui"));
            ui = Instantiate(watchBundle.LoadAsset<GameObject>("PlayerUI"));
            ui.transform.parent = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/Main Camera").transform;
            ui.transform.localPosition = new Vector3(0.1f, 0f, 0.6436f);
            ui.transform.localRotation = Quaternion.identity;
            ui.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            ui.transform.GetChild(0).transform.localPosition = Vector3.zero;
            ui.transform.GetChild(0).transform.localRotation = Quaternion.identity;

            watchBundle.Unload(false);

            ui.AddComponent<CameraBoard>();
        }
    }
}

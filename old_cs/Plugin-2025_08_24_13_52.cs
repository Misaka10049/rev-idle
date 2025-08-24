using System;
// using AsmResolver.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.UnityEngine;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
// using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

using KeyCode = BepInEx.Unity.IL2CPP.UnityEngine.KeyCode;// UnityEngine.KeyCode
namespace Trainer_for_Revolution_Idle
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance { get; private set; }

        public Manager(IntPtr ptr) : base(ptr)
        {
            Instance = this;
        }

        internal static GameObject Create(string name)
        {
            var gameObject = new GameObject(name);
            DontDestroyOnLoad(gameObject);

            var component = new Manager(gameObject.AddComponent(Il2CppType.Of<Manager>()).Pointer);

            return gameObject;
        }

        private void Update()
        {
            if (Input.GetKeyInt(KeyCode.Tab)) {
                Plugin.Log.LogMessage("Hello World!");
            }
        }
    }
    [BepInPlugin("trainer.ri", "Trainer", "0.1.0")]
    [BepInProcess("Revolution Idle.exe")]
    public class Plugin : BasePlugin
    {
        // public Action<Scene, LoadSceneMode> LoadAction;
        internal static new ManualLogSource Log;
        public static ConfigEntry<int> ConfigboostProgress;
        public override void Load()// 模块从这里开始运行
        {
            Log = base.Log;
            ClassInjector.RegisterTypeInIl2Cpp<Manager>();// 在 IL2CPP 中注册新的类
            var harmony = new Harmony("trainer.ri");
            // var originalHandle = AccessTools.Method(typeof(CanvasScaler), "Handle");
            // var postHandle = AccessTools.Method(typeof(BootstrapPatch), "NewHandle");
            // harmony.Patch(originalHandle, postfix: new HarmonyMethod(postHandle));
            harmony.PatchAll();
            GetConfig();
            // CreateUI();
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} was loaded!");
        }
        private static void CreateUI()
        {
            // GameObject pingmian = GameObject.CreatePrimitive(PrimitiveType.Quad);
            GameObject pingmian = new GameObject("ByMod");
            pingmian.transform.position = new Vector3(0, 0, 0);
            // pingmian.AddComponent<Component>();
            pingmian.SetActive(true);
            // pingmian.AddComponent(Il2CppType.Of<Manager>());
        }
        public void GetConfig()
        {
            ConfigboostProgress = Config.Bind("Revolution", "boostProgress", -1, "每帧使每圈多完成 10^boostProgress 次, -1 为关闭, 最大为 300");
            if (ConfigboostProgress.Value > 300) ConfigboostProgress.Value = 300;
            Patches.boostProgress = ConfigboostProgress.Value;
        }
        public override bool Unload()
        {
            Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} was unloaded!");
            return false;
        }
        public static long Date() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    [HarmonyPatch]
    class Patches
    {
        [HarmonyPostfix, HarmonyPatch(typeof(CanvasScaler), "Handle")]
        static void Bootstrap()
        {
            if(Manager.Instance==null)
                Manager.Create("Manager");
        }
        public static int boostProgress;
        [HarmonyPostfix, HarmonyPatch(typeof(Revolution), "updateThis")]
        static void Postfix1(ref Revolution __instance)
        {
            if (boostProgress > 0)
                __instance.progress += Math.Pow(10, boostProgress);
        }
        [HarmonyPostfix, HarmonyPatch(typeof(GameController), "Update")]
        static void Postfix2(/*ref GameController __instance*/)
        {
            if (GameController.inventory.soul < 50000)
                GameController.inventory.soul = 50000;

            if (GameController.Infinity.infs < BigDouble.Pow10(500))
                GameController.Infinity.infs = BigDouble.Pow10(500);

            if (GameController.Eternity.eters < BigDouble.Pow10(500))
                GameController.Eternity.eters = BigDouble.Pow10(500);


            if (GameController.Eternity.DP < BigDouble.Pow10(750000))
                GameController.Eternity.DP = BigDouble.Pow10(750000);

            if (GameController.data.minerals.magnets < 10000)
                GameController.data.minerals.magnets = 1e4;

            GameController.Attacks.gold += GameController.Attacks.goldOnUnity;
            GameController.Unity.astrodust = BigDouble.Pow10(500);
            GameController.data.timeFlux = 1e10;
            GameController.data.minerals.mergeLevel = 1024;
            GameController.data.minerals.refinePoints += GameController.data.minerals.refinePointsNext;
            GameController.data.minerals.AutoMerge();
        }
    }
}

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
    // public class Manager : MonoBehaviour
    // {
    //     public static Manager Instance { get; private set; }

    //     public Manager(IntPtr ptr) : base(ptr)
    //     {
    //         Instance = this;
    //     }

    //     internal static GameObject Create(string name)
    //     {
    //         var gameObject = new GameObject(name);
    //         DontDestroyOnLoad(gameObject);

    //         var component = new Manager(gameObject.AddComponent(Il2CppType.Of<Manager>()).Pointer);

    //         return gameObject;
    //     }

    //     private void Update()
    //     {
    //     }
    // }
    [BepInPlugin("trainer.ri", "Trainer", "0.1.0")]
    [BepInProcess("Revolution Idle.exe")]
    public class Plugin : BasePlugin
    {
        internal static new ManualLogSource Log;
        public static ConfigEntry<int> ConfigBoostProgress, ConfigSoul, ConfigTimeFlux, ConfigAstrodust;
        public static ConfigEntry<bool> ConfigGoldOnUnity, ConfigPolishPointsNext, ConfigRefinePointsNext, ConfigAutoMerge;
        public override void Load()// 模块从这里开始运行
        {
            Log = base.Log;
            // ClassInjector.RegisterTypeInIl2Cpp<Manager>();// 在 IL2CPP 中注册新的类
            var harmony = new Harmony("trainer.ri");
            harmony.PatchAll();
            GetConfig();
            Log.LogMessage($"Plugin {MyPluginInfo.PLUGIN_GUID} was loaded!");
        }
        public void GetConfig()
        {
            ConfigBoostProgress = Config.Bind("Revolution", "boostProgress", -1, "每帧使每圈多完成 10^boostProgress 次, -1 为关闭, 最大为 300");
            if (ConfigBoostProgress.Value < -1) ConfigBoostProgress.Value = -1;
            if (ConfigBoostProgress.Value > 300) ConfigBoostProgress.Value = 300;
            Patches.boostProgress = ConfigBoostProgress.Value;

            ConfigSoul = Config.Bind("Inventory", "soul", 0, "将灵魂数量锁定为 soul, 0 为不启用, 最大为 2147483647");
            if (ConfigSoul.Value < 0) ConfigSoul.Value = 0;
            else if (ConfigSoul.Value > 2147483647) ConfigSoul.Value = 2147483647;
            Patches.soul = ConfigSoul.Value;

            ConfigTimeFlux = Config.Bind("Time", "timeFlux", 0, "将时间流量锁定为 timeFlux 秒, 0 为不启用, 最大为 2147483647");
            if (ConfigTimeFlux.Value < 0) ConfigTimeFlux.Value = 0;
            else if (ConfigTimeFlux.Value > 2147483647) ConfigTimeFlux.Value = 2147483647;
            Patches.timeFlux = ConfigTimeFlux.Value;

            ConfigAstrodust = Config.Bind("Unity", "astrodust", 0, "将星尘数量锁定为 10^astrodust, 0 为不启用, 最大为 300");
            if (ConfigAstrodust.Value > 300) ConfigAstrodust.Value = 300;
            Patches.astrodust = ConfigAstrodust.Value;

            ConfigGoldOnUnity = Config.Bind("Unity", "goldOnUnity", false, "每帧获得金子，数量等同于统一时能获得的金子, false 为不启用, true 为启用");
            Patches.goldOnUnity = ConfigGoldOnUnity.Value;

            ConfigPolishPointsNext = Config.Bind("Unity.Mine", "polishPointsNext", false, "每帧获得抛光点数，数量等同于抛光时能获得的抛光点数, false 为不启用, true 为启用");
            Patches.polishPointsNext = ConfigPolishPointsNext.Value;

            ConfigRefinePointsNext = Config.Bind("Unity.Mine", "refinePointsNext", false, "每帧获得精炼点数，数量等同于精炼时能获得的精炼点数, false 为不启用, true 为启用");
            Patches.refinePointsNext = ConfigRefinePointsNext.Value;

            ConfigAutoMerge = Config.Bind("Unity.Mine", "AutoMerge", false, "自动合成矿物, false 为不启用, true 为启用");
            Patches.AutoMerge = ConfigAutoMerge.Value;
        }
        public static long Date() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    [HarmonyPatch]
    class Patches
    {
        // [HarmonyPostfix, HarmonyPatch(typeof(CanvasScaler), "Handle")]
        // static void Bootstrap()
        // {
        //     if(Manager.Instance==null)
        //         Manager.Create("Manager");
        // }
        public static int boostProgress, soul, timeFlux, astrodust;
        public static bool goldOnUnity, polishPointsNext, refinePointsNext, AutoMerge;
        [HarmonyPostfix, HarmonyPatch(typeof(Revolution), "updateThis")]
        static void Postfix1(ref Revolution __instance)
        {
            if (boostProgress > 0)
                __instance.progress += Math.Pow(10, boostProgress);
        }
        [HarmonyPostfix, HarmonyPatch(typeof(GameController), "Update")]
        static void Postfix2(/*ref GameController __instance*/)
        {
            if (soul != 0)
                GameController.inventory.soul = soul;

            if (timeFlux != 0)
                GameController.data.timeFlux = timeFlux;

            if (astrodust != 0)
                GameController.Unity.astrodust = BigDouble.Pow10(astrodust);

            if (goldOnUnity)
                GameController.Attacks.gold += GameController.Attacks.goldOnUnity;

            if (polishPointsNext)
                GameController.data.minerals.polishPoints += GameController.data.minerals.polishPointsNext;

            if (refinePointsNext)
                GameController.data.minerals.refinePoints += GameController.data.minerals.refinePointsNext;

            if (AutoMerge)
                GameController.data.minerals.AutoMerge();

            // GameController.data.minerals.polishPointsNext
            // if (GameController.Infinity.infs < BigDouble.Pow10(500))
            //     GameController.Infinity.infs = BigDouble.Pow10(500);

            // if (GameController.Eternity.eters < BigDouble.Pow10(500))
            //     GameController.Eternity.eters = BigDouble.Pow10(500);


            // if (GameController.Eternity.DP < BigDouble.Pow10(750000))
            //     GameController.Eternity.DP = BigDouble.Pow10(750000);

            // if (GameController.data.minerals.magnets < 10000)
            //     GameController.data.minerals.magnets = 1e4;

            // GameController.Attacks.gold += GameController.Attacks.goldOnUnity;
            // GameController.Unity.astrodust = BigDouble.Pow10(500);
            // GameController.data.timeFlux = 1e10;
            // GameController.data.minerals.mergeLevel = 1024;
            // GameController.data.minerals.refinePoints += GameController.data.minerals.refinePointsNext;
            // GameController.data.minerals.AutoMerge();
        }
    }
}
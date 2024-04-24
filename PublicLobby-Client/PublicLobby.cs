using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace PublicLobby_Client;

[BepInAutoPlugin(PluginId)]
[BepInProcess("Among Us.exe")]
public sealed partial class PublicLobby : BasePlugin
{
    public const string PluginId = "lobby.mengchu.net";

    public static PublicLobby? Instance;

    public static ManualLogSource? log => Instance?.Log;
    
    public override void Load()
    {
        Instance = this;
        
        var assembly = typeof(PublicLobby).Assembly;
        Harmony.CreateAndPatchAll(assembly, Id);
        log?.LogInfo($"Plugin {PluginId} loaded");
    }
}
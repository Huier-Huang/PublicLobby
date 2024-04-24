using AmongUs.Data.Legacy;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using PublicLobby_Server.Api.Clients;
using PublicLobby_Server.Api.Servers;

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

        var Client = new Client();
        Client.Init();
        Client.InitMod(GetMods());
    }

    private static IEnumerable<Mod> GetMods()
    {
        return IL2CPPChainloader.Instance.Plugins.Values.Select(Info => new Mod
            { Id = Info.Metadata.GUID, Version = Info.Metadata.Version.ToString(), Name = Info.Metadata.Name });
    }
}

public static class Patches
{

    private static bool inited;
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPostfix]
    public static void OnMainMenu()
    {
        if (inited)
            return;
        Client.Local?.InitInfo(EOSManager.Instance.ProductId, EOSManager.Instance.FriendCode, LegacySaveManager.PlayerName);
        inited = true;
    }
}
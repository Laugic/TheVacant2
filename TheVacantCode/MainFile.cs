using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace TheVacant.TheVacantCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "TheVacant"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Editor.AssetBundleTools;

public static class MacBuildHelp
{
    [MenuItem("Build/iOS内网_重新编译", false, 101)]
    public static void BuildPlayeriOSNei_Rebuild()
    {
        string old = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, old + ",BUILD_TYPE_NEI");
        BuildPipeline.BuildPlayer(GetBuildScenesAndEnsuerFolder(), "../build/IPA_inner", BuildTarget.iOS, BuildOptions.None);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, old);
        Debug.Log("打包结束！");
    }

    private static string[] GetBuildScenesAndEnsuerFolder()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        if (!Directory.Exists("../build"))
        {
            System.IO.Directory.CreateDirectory("../build");
        }
        return names.ToArray();
    }

    [MenuItem("Build/Build IOS AssetBundle",false, 102)]
    public static void BuildIOSAssetBundle()
    {
        AssetBundleBuilderController controller = new AssetBundleBuilderController();
        if (!controller.Load())
        {
            throw new GameFrameworkException("Load configuration failure.");
        }
        else
        {
            Debug.Log("Load configuration success.");
        }

        if (controller.RefreshBuildEventHandler())
        {
            Debug.Log("Set build event success.");
        }
        else
        {
            Debug.LogWarning("Set build event failure.");
        }

        // 生成Directory
        string currentPath = Directory.GetCurrentDirectory();
        string workPath = currentPath + "/AssetBundle";
        if (!Directory.Exists(workPath))
        {
            Directory.CreateDirectory(workPath);
        }

        controller.OutputDirectory = workPath; 

        if (!controller.IsValidOutputDirectory)
        {
            throw new GameFrameworkException(string.Format("Output directory '{0}' is invalid.", controller.OutputDirectory));
        }

        if (!controller.BuildAssetBundles())
        {
            throw new GameFrameworkException("Build AssetBundles failure.");
        }
        else
        {
            Debug.Log("Build AssetBundles success.");
            controller.Save();
        }
    }
}

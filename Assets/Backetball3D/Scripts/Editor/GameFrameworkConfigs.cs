using GameFramework;
using UnityEngine;
using UnityGameFramework.Editor;
using UnityGameFramework.Editor.AssetBundleTools;

namespace BasketballRash.Editor
{
    public static class GameFrameworkConfigs
    {
        [BuildSettingsConfigPath]
        public static string BuildSettingsConfig = Utility.Path.GetCombinePath(Application.dataPath, "Backetball3D/Configs/BuildSettings.xml");

        [AssetBundleBuilderConfigPath]
        public static string AssetBundleBuilderConfig = Utility.Path.GetCombinePath(Application.dataPath, "Backetball3D/Configs/AssetBundleBuilder.xml");

        [AssetBundleEditorConfigPath]
        public static string AssetBundleEditorConfig = Utility.Path.GetCombinePath(Application.dataPath, "Backetball3D/Configs/AssetBundleEditor.xml");

        [AssetBundleCollectionConfigPath]
        public static string AssetBundleCollectionConfig = Utility.Path.GetCombinePath(Application.dataPath, "Backetball3D/Configs/AssetBundleCollection.xml");
    }
}

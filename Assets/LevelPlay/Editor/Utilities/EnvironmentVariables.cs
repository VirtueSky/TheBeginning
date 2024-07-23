#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Unity.Services.LevelPlay.Editor
{
    public static class EnvironmentVariables
    {
        const string k_LevelPlayPackageName = "com.unity.services.levelplay";
        const string k_AndroidLibPath = "Runtime/Plugins/Android/IronSource.androidlib/AndroidManifest.xml";
        static readonly string k_DotUnityPackageManifestPath = Path.Combine("LevelPlay", k_AndroidLibPath);
        static readonly string k_UpmManifestPath = Path.Combine("Packages/com.unity.services.levelplay", k_AndroidLibPath);

        internal static string androidManifestPath { get; private set; }

        [InitializeOnLoadMethod]
        static void BuildManifestPath()
        {
            LevelPlayPackmanQuerier.instance.CheckIfPackageIsInstalledWithUpm(k_LevelPlayPackageName, isInstalled =>
            {
                androidManifestPath = isInstalled ? Path.GetFullPath(k_UpmManifestPath) : Path.Combine(Application.dataPath, k_DotUnityPackageManifestPath);
            });
        }
    }
}
#endif

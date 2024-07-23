using System.IO;
using UnityEditor;

namespace Unity.Services.LevelPlay.Editor.Analytics
{
    class EditorSessionTracker
    {
#if !AdsMediation_BuilderProject
        const string k_NewSessionKey = "NewSession";
        const string k_FootprintFilePath = "Assets/LevelPlay/Editor/LevelPlayFootprint.txt";
        const string k_LevelPlayPackageName = "com.unity.services.levelplay";

        [InitializeOnLoadMethod]
        static void NewSession()
        {
            if (!SessionState.GetBool(k_NewSessionKey, false))
            {
                SessionState.SetBool(k_NewSessionKey, true);
                LevelPlayPackmanQuerier.instance.CheckIfPackageIsInstalledWithUpm(k_LevelPlayPackageName, SendNewSessionEvent);
            }

            SendInstallEventIfNeeded();
        }

        static void SendNewSessionEvent(bool levelPlayIsUpm)
        {
            LevelPlayEditorAnalytics.Instance.SendNewSession(levelPlayIsUpm
                ? LevelPlayEditorAnalytics.LevelPlayComponent.UpmPackage
                : LevelPlayEditorAnalytics.LevelPlayComponent.UnityPackage);
        }

        static void SendInstallEventIfNeeded()
        {
            if (File.Exists(k_FootprintFilePath))
            {
                LevelPlayEditorAnalytics.Instance.SendInstallPackage(LevelPlayEditorAnalytics.LevelPlayComponent.UnityPackage);
                File.Delete(k_FootprintFilePath);
            }
        }

#endif
    }
}

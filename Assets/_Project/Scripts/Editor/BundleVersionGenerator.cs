#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VirtueSky.Misc;

public class BundleVersionGenerator : Editor
{
    [MenuItem("TheBeginning_2/Generate Bundle Version", priority = 2001)]
    public static void GenerateBundleVersion()
    {
        // Lấy thời gian hiện tại
        System.DateTime now = System.DateTime.Now;

        // Format: YYMMDDHH (năm 2 chữ số, tháng 2 chữ số, ngày 2 chữ số, giờ 2 chữ số)
        string bundleVersion = now.ToString("yyMMddHH");

#if UNITY_ANDROID
        PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersion);
#elif UNITY_IOS
        PlayerSettings.iOS.buildNumber = bundleVersion;
#endif
        // PlayerSettings.Android.keystorePass = "abc";
        // PlayerSettings.Android.keyaliasPass = "abc";

        // Lưu thay đổi
        AssetDatabase.SaveAssets();

        Debug.Log($"Bundle version updated to: {bundleVersion}".SetColor(Color.green));

        // Hiển thị dialog thông báo
        EditorUtility.DisplayDialog("Bundle Version Generated",
            $"Bundle version has been set to: {bundleVersion}",
            "OK");
    }
}
#endif
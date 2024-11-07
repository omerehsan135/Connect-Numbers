using UnityEditor;
using UnityEngine;

namespace Ilumisoft.Hex.Editor.GameTemplate
{
    public static class MenuItems
    {
        [MenuItem("Game Template/Hex/Welcome")]
        static void OpenPackageUtility()
        {
            PackageUtilityWindow.Init();
        }

        [MenuItem("Game Template/Hex/Rate")]
        static void Rate()
        {
            var bundleInfo = ScriptableObjectUtility.Find<PackageInfo>();

            if (bundleInfo != null)
            {
                Application.OpenURL(bundleInfo.PackageURL);
            }
        }
    }
}
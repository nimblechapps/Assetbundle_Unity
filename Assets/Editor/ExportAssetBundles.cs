using UnityEngine;
using UnityEditor;

public class ExportAssetBundles
{

	[MenuItem ("Assets/Build AssetBundle")]
	static void ExportResource ()
	{
		// Bring up save panel
		string basename = Selection.activeObject ? Selection.activeObject.name : "New Resource";
		string path = EditorUtility.SaveFilePanel ("Save Resources", "", basename, "");

		if (path.Length != 0)
		{
			// Build the resource file from the active selection.
			Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);

			#if UNITY_ANDROID
			// for Android
			BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path + ".android.unity3d", BuildAssetBundleOptions.CollectDependencies |
			BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
			
			#elif UNITY_IPHONE
			// for iPhone
			BuildPipeline.BuildAssetBundle (Selection.activeObject, selection, path + ".iphone.unity3d", BuildAssetBundleOptions.CollectDependencies |
			BuildAssetBundleOptions.CompleteAssets, BuildTarget.iOS);
			#endif

			Selection.objects = selection;
		}
	}

	[MenuItem ("Assets/Build SceneBundle")]
	static void ExportScene ()
	{
		if (!System.IO.Directory.Exists (Application.dataPath + "/AssetBundle"))
		{
			System.IO.Directory.CreateDirectory (Application.dataPath + "/AssetBundle");
		}

		#if UNITY_ANDROID
		// for Android
		string[] levels = new string[] { "Assets/AssetScene.unity" };
		BuildPipeline.BuildStreamedSceneAssetBundle (levels, Application.dataPath + "/AssetBundle/AssetScene.unity3d", BuildTarget.Android);

		#elif UNITY_IPHONE
		// for iPhone
		string[] levels1 = new string[] { "Assets/AssetScene.unity" };
		BuildPipeline.BuildStreamedSceneAssetBundle (levels, Application.dataPath + "/AssetBundle/AssetScene.unity3d", BuildTarget.iOS);
		#endif
	}
}

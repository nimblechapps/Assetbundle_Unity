using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class AssetBundleSample : MonoBehaviour
{
	public string loadUrl;
	public Text Statustext;
	bool isLoading;
	GameObject AssetBundleObj;
	public Button LoadSceneButton;

	// Use this for initialization
	void Start ()
	{
		isLoading = false;
		LoadSceneButton.gameObject.SetActive (false);
		StartCoroutine (LoadSceneBundle ());
	}
  
	// Update is called once per frame
	void Update ()
	{
		// progress
		if (isLoading)
		{
			int percent = (int)(www.progress * 100);
			Statustext.text = percent.ToString () + "%";
		}
	}

	private WWW www;

	private IEnumerator load (string soundAsset)
	{
		// wait for the caching system to be ready
		while (!Caching.ready)
			yield return null;

		// load AssetBundle file from Cache if it exists with the same version or download and store it in the cache
		www = WWW.LoadFromCacheOrDownload (loadUrl, 1);
		yield return www;

		Debug.Log ("Loaded ");
		Statustext.text = "Loaded";
		if (www.error != null)
			throw new Exception ("WWW download had an error: " + www.error);
    
		AssetBundle assetBundle = www.assetBundle;
		//Instantiate (assetBundle.mainAsset); // Instantiate(assetBundle.Load("AssetName"));

		AssetBundleObj = Instantiate (assetBundle.mainAsset) as GameObject;
		AssetBundleObj.transform.Find (soundAsset).GetComponent<AudioSource> ().Play ();
		// Unload the AssetBundles compressed contents to conserve memory
		assetBundle.Unload (false);
	}

	public void LoadSoundFiles (string soundAsset)
	{
		isLoading = true;
		// Clear Cache
		Caching.CleanCache ();

		if (AssetBundleObj)
		{
			Destroy (AssetBundleObj);
		}
		StartCoroutine (load (soundAsset));
		
	}

	public void LoadFromLocal ()
	{
		string path = "";

		path = "file://" + Application.persistentDataPath + "/AssetScene.unity3d";
		Debug.Log (path);

		StartCoroutine (LoadFromMemoryAsync (path));
	}

	IEnumerator LoadSceneBundle ()
	{
		Statustext.text = "Downloading scene ...";
		string SceneURL = "https://www.dropbox.com/s/fo4v777xvgrakrh/AssetScene.unity3d?dl=1";

		www = new WWW (SceneURL);
		yield return www;

		Debug.Log ("Loaded ");
		if (www.error != null)
			throw new Exception ("WWW download had an error: " + www.error);

		File.WriteAllBytes (Application.persistentDataPath + "/AssetScene.unity3d", www.bytes);
		yield return new WaitForEndOfFrame ();

		Statustext.text = "";

		LoadSceneButton.gameObject.SetActive (true);
	}

	IEnumerator LoadFromMemoryAsync (string path)
	{
		var LoadedAssetScene = AssetBundle.LoadFromFile (Application.persistentDataPath + "/AssetScene.unity3d");
		if (LoadedAssetScene == null)
		{
			Debug.Log ("Failed to load AssetBundle!");
			yield return new WaitForEndOfFrame ();
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene ("AssetScene");
		LoadedAssetScene.Unload (false);
	}
}


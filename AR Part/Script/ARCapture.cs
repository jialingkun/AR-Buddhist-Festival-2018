using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ARCapture : MonoBehaviour {
	private GameObject buttonPanel;
	private Text errorText;

	//flash
	private Image flashPanel;
	public float FlashIntensity = 0.7f;
	public float FlashDuration = 0.7f;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.AutoRotation;
		buttonPanel = GameObject.Find ("ButtonPanel");
		errorText = GameObject.Find ("Error").GetComponent<Text>();

		//flash
		flashPanel = GameObject.Find ("FlashPanel").GetComponent<Image>();
		flashPanel.CrossFadeAlpha (0.0f, 0.1f, false);

		//temporary, should be execute at start menu
		SaveLoad.loadAll();

		// Get a reference to the storage service, using the default Firebase App
		//Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

		// Create a storage reference from our storage service
		//Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://buddhist-festival-ar-2018.appspot.com");
	}

	public IEnumerator screenshot(){
		buttonPanel.SetActive (false);
		yield return new WaitForEndOfFrame();
		//take screen shot

		Texture2D screenTexture = new Texture2D(Screen.width, Screen.height-(int)(Screen.height*0.1f),TextureFormat.RGB24,true);
		//normally like this:
		//screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
		//try to cut out lower part (the watermark) to fit gallery preview
		screenTexture.ReadPixels(new Rect(0, (int)(Screen.height*0.1f), Screen.width, Screen.height), 0, 0, false);
		screenTexture.Apply();

		//flash
		flashPanel.CrossFadeAlpha (1.0f, 0.1f, false);
		yield return new WaitForSeconds (0.1f);

		//save screen shot
		byte[] dataToSave = screenTexture.EncodeToPNG();
		string destination = SaveLoad.path;
		string filename = System.DateTime.Now.ToString("yyyymmdd_hhmmss")+".png";
		try {
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}

			File.WriteAllBytes(destination+ "/" + filename, dataToSave);
			SaveLoad.saveImageName(filename,screenTexture);
			//refresh Gallery App
			/*
			using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
			using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
			using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
			using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
			{
				jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { destination+ "/" + filename }, null, null);
			}*/


		} catch (System.Exception ex) {
			errorText.text = ex.ToString();
		}
		flashPanel.CrossFadeAlpha (0.0f, FlashDuration, false);
		yield return new WaitForSeconds (FlashDuration-0.1f);
		//errorText.text = "Finished Capturing.. Uploading...";
		buttonPanel.SetActive (true);

	}

	public void clickCapture(){
		StartCoroutine (screenshot ());
	}

	public void clickGallery(){
		SceneManager.LoadScene (1); //temporary scene index
	}
}

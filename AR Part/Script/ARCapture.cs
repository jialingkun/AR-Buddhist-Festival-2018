using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ARCapture : MonoBehaviour {
	private GameObject captureButton;
	private GameObject galleryButton;
	private Text errorText;

	//flash
	private Image flashPanel;
	public float FlashIntensity = 0.7f;
	public float FlashDuration = 0.7f;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.AutoRotation;
		captureButton = GameObject.Find ("Capture");
		galleryButton = GameObject.Find ("Gallery");
		errorText = GameObject.Find ("Error").GetComponent<Text>();

		//flash
		flashPanel = GameObject.Find ("FlashPanel").GetComponent<Image>();
		flashPanel.CrossFadeAlpha (0.0f, 0.1f, false);

		//Load
		SaveLoad.loadImageName(); //temporary, should be execute at start menu
		SaveLoad.loadImageTexture();

		// Get a reference to the storage service, using the default Firebase App
		//Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

		// Create a storage reference from our storage service
		//Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://buddhist-festival-ar-2018.appspot.com");
	}

	public IEnumerator screenshot(){
		captureButton.SetActive (false);
		galleryButton.SetActive (false);
		yield return new WaitForEndOfFrame();
		//take screen shot

		Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,true);
		screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
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
		} catch (System.Exception ex) {
			errorText.text = ex.ToString();
		}
		flashPanel.CrossFadeAlpha (0.0f, FlashDuration, false);
		yield return new WaitForSeconds (FlashDuration-0.1f);
		//errorText.text = "Finished Capturing.. Uploading...";
		captureButton.SetActive (true);
		galleryButton.SetActive (true);

	}

	public void clickCapture(){
		StartCoroutine (screenshot ());
	}

	public void clickGallery(){
		SceneManager.LoadScene (1); //temporary scene index
	}
}

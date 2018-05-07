using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ARCapture : MonoBehaviour {
	GameObject captureButton;
	Text errorText;
	// Use this for initialization
	void Start () {
		captureButton = GameObject.Find ("Capture");
		errorText = GameObject.Find ("Error").GetComponent<Text>();

		// Get a reference to the storage service, using the default Firebase App
		Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

		// Create a storage reference from our storage service
		Firebase.Storage.StorageReference storage_ref =
			storage.GetReferenceFromUrl("gs://buddhist-festival-ar-2018.appspot.com");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator screenshot(){
		captureButton.SetActive (false);
		yield return new WaitForEndOfFrame();
		//take screen shot
		Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,true);
		screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
		screenTexture.Apply();

		//save screen shot
		byte[] dataToSave = screenTexture.EncodeToPNG();
		string destination = Application.persistentDataPath + "/../../../../Pictures/ARBuddhistFestival2018";
		string filename = "test.png";
		try {
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}

			File.WriteAllBytes(destination+ "/" + filename, dataToSave);
		} catch (System.Exception ex) {
			errorText.text = ex.ToString();
		}
		//errorText.text = "Finished Capturing.. Uploading...";
		captureButton.SetActive (true);

	}

	public void clickCapture(){
		StartCoroutine (screenshot ());
	}
}

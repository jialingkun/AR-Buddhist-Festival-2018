using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gallery : MonoBehaviour {
	public GameObject galleryImagePrefab;

	private RectTransform content;
	//x coordinate spawn
	private float spawnPointLeft;

	public int padding;
	public int gap;

	//preview image
	private GameObject preview;
	private GameObject imagePreview;




	// Use this for initialization
	void Start () {
		//preview image
		preview = GameObject.Find ("Preview");
		imagePreview = GameObject.Find ("ImagePreview");
		preview.SetActive (false);

		//scroll content
		content = GameObject.Find ("Content").GetComponent<RectTransform> ();
		//x Coordinate spawn
		spawnPointLeft = GameObject.Find ("SpawnPointLeft").GetComponent<RectTransform> ().anchoredPosition.x;

		refreshGallery ();
	}

	public void refreshGallery(){
		//remove all gallery image
		GameObject[] collectionClone;
		collectionClone = GameObject.FindGameObjectsWithTag ("GalleryImage");
		foreach (GameObject buttonObject in collectionClone) {
			Destroy (buttonObject);
		}


		RectTransform galleryImageTransform;
		float galleryImageOffset = galleryImagePrefab.GetComponent<RectTransform> ().sizeDelta.y;
		Vector2 galleryImagePosition = new Vector2 (spawnPointLeft, -(galleryImageOffset/2+galleryImageOffset/padding));
		float galleryNextPositionY = 0f;
		int galleryCount = SaveLoad.imageTexture.Count;
		float ratio = 1f;

		GameObject galleryImageObject;
		bool isleft = true;

		for (int i = 0; i < galleryCount; i++) {
			galleryImageObject = Instantiate (galleryImagePrefab);

			//fix bug call by reference on clickedit() parameter
			int index = i;
			galleryImageObject.GetComponent<Button> ().onClick.AddListener (delegate() {
				clickGalleryImage(index);
			});

			galleryImageObject.transform.Find("RawImage").GetComponent<RawImage> ().texture = SaveLoad.imageTexture[i];

			//change aspect ratio texture
			ratio = (float)SaveLoad.imageTexture[i].width / (float)SaveLoad.imageTexture[i].height;
			galleryImageObject.transform.Find("RawImage").GetComponent<AspectRatioFitter> ().aspectRatio = ratio;

			galleryImageTransform = galleryImageObject.GetComponent<RectTransform> ();
			galleryImageTransform.SetParent (content,false);
			galleryImageTransform.anchoredPosition = galleryImagePosition;

			galleryImagePosition.x = -galleryImagePosition.x;
			if (isleft) {
				galleryNextPositionY = galleryImagePosition.y - galleryImageOffset - galleryImageOffset / gap;
				isleft = false;
			} else {
				galleryImagePosition.y = galleryNextPositionY;
				isleft = true;
			}

		}

		//scroll space width = (last position + gap) - gap + padding
		//The last position after for loop only store (last position + gap), so convert it back to last position by - gap
		content.sizeDelta = new Vector2 (content.sizeDelta.x, - galleryNextPositionY - (galleryImageOffset + galleryImageOffset / gap) + (galleryImageOffset/2 + galleryImageOffset/padding));

	}


	public void clickGalleryImage(int index){
		imagePreview.GetComponent<RawImage> ().texture = SaveLoad.imageTexture[index];
		float ratio = (float)SaveLoad.imageTexture[index].width / (float)SaveLoad.imageTexture[index].height;
		imagePreview.GetComponent<AspectRatioFitter> ().aspectRatio = ratio;
		preview.SetActive (true);
	}


	public void clickARCamera(){
		SceneManager.LoadScene (0); //temporary scene index
	}
}

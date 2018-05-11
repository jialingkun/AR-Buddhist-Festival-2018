using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoad{
	public static List<string> imageNameList;

	public static void loadImageName(){
		string tempImageList = PlayerPrefs.GetString("ImageName","").Trim();
		if (tempImageList == "") {
			imageNameList = new List<string> ();
		} else {
			imageNameList = new List<string> (tempImageList.Split(","[0]));
		}
	}

	public static void saveImageName(string filename){
		imageNameList.Add (filename);
		PlayerPrefs.SetString("ImageName",string.Join(",", imageNameList.ToArray()));
	}
}

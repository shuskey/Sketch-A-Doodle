using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

public class CaptureSquareScreenShot : MonoBehaviour
{

	[SerializeField] private Camera captureCamera;
	[SerializeField] private RenderTexture renderTexture;
	[SerializeField] private Text filenameText;

	public void GrabSquare()
	{		
		StartCoroutine(GrabSquareCoroutine());
		var devices = WebCamTexture.devices;
		WebCamTexture webcamTexture = new WebCamTexture(devices[0].name, 1920, 1000);		
		webcamTexture.Stop();
		SceneManager.LoadScene("Scenes/Intro");
	}

	IEnumerator GrabSquareCoroutine()
	{
		yield return new WaitForEndOfFrame();

		captureCamera.targetTexture = renderTexture;
		captureCamera.Render();

		RenderTexture.active = renderTexture;
		var cropWidthOffset = (renderTexture.width - renderTexture.height) / 2;
		Texture2D virtualPhoto = new Texture2D(renderTexture.height, renderTexture.height, TextureFormat.RGB24, false);
		virtualPhoto.ReadPixels(new Rect(cropWidthOffset, 0, renderTexture.height, renderTexture.height), 0, 0);
		RenderTexture.active = null;
		captureCamera.targetTexture = null;

		// Encode texture into PNG
		byte[] bytes = virtualPhoto.EncodeToPNG();
		UnityEngine.Object.Destroy(virtualPhoto);
		var filename = string.IsNullOrEmpty(filenameText.text) ? "Maze-" + System.DateTime.Now.ToString("ddMMMM-HH-mm-ss") : filenameText.text;
		var invalids = System.IO.Path.GetInvalidFileNameChars();
		var newName = System.String.Join("_", filename.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
		var completeFileName = Application.dataPath + $"/Mazes/{newName}.png";
		File.WriteAllBytes(completeFileName, bytes);

		//File.ReadAllBytes
		CreateScriptableObject(newName, completeFileName, invertToUseBlackLines: true);
	}

	void CreateScriptableObject(string name, string fullFileName, bool invertToUseBlackLines)
    {
		var saveObj = ScriptableObject.CreateInstance<MazeLevel_ScriptableObject>();

		saveObj.name = name;
		//saveObj.mazeTexture = texture;
		saveObj.mazeTextureFileName = fullFileName;
		saveObj.invertToUseBlackLines = invertToUseBlackLines;

		AssetDatabase.CreateAsset(saveObj, $"Assets/Mazes/ScriptableObjects/{name}.asset");
		EditorUtility.SetDirty(saveObj);
		AssetDatabase.SaveAssets();
	}
}

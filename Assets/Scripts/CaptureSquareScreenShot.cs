using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using Assets.Scripts.DataProviders;
using Assets.Scripts.DataObjects;

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
		SceneManager.LoadScene("Scenes/EditMazeLevel");
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
		var filename = "Maze-" + System.DateTime.Now.ToString("ddMMMM-HH-mm-ss");
		var invalids = System.IO.Path.GetInvalidFileNameChars();
		var newName = System.String.Join("_", filename.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
		//var completeFileName = Application.dataPath + $"/Mazes/{newName}.png";
		var completeFileName = $"{Application.persistentDataPath}/{newName}.png";

		Debug.Log($"complete filename of newly grabbed maze = {completeFileName}");

		File.WriteAllBytes(completeFileName, bytes);
		Texture2D textureFromFile = new Texture2D(2, 2);
        textureFromFile.LoadImage(bytes);
		//var relativePathAndFileName = $"Assets/Mazes/{newName}.png";
		//CreateMazeLevelInDataBase(newName, relativePathAndFileName, invertToUseBlackLines: true);
		CreateMazeLevelInDataBase(newName, completeFileName, invertToUseBlackLines: true, textureFromFile);
	}

	void CreateMazeLevelInDataBase(string name, string fullFileName, bool invertToUseBlackLines, Texture2D texture)
    {
		var newMazeLevel = new MazeLevel(fullFileName, invertToUseBlackLines);
		newMazeLevel.mazeTexture = texture;

#if UNITY_WEBGL
		MazeList.Mazes.Add(newMazeLevel);
#else
		var mazeListDataBase = new ListOfMazesFromDataBase();
		mazeListDataBase.CreateDataBaseFileIfNotExists();
		mazeListDataBase.CreateTableForListOfMazesIfNotExists();		
		mazeListDataBase.AddMaze(newMazeLevel);
#endif
		MazePlayMode.currentMazeLevel = newMazeLevel;		
	}
}

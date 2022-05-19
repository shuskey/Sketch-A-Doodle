using Assets.Scripts.DataProviders;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoosePlayMode : MonoBehaviour
{  
    [SerializeField] private Image mazeImage;
    [SerializeField] private Text mazeTitle;

    [SerializeField] private List<Text> place_2D_text_list = new List<Text>();
    [SerializeField] private List<Text> place_3D_text_list = new List<Text>();

    [SerializeField] private Button Play2DButton;
    [SerializeField] private Button Play3DButton;
    [SerializeField] private Button CancelButton;
    [SerializeField] private Button EditButton;

    // Start is called before the first frame update
    void Start()
    {
        if (BackGroundManager.bgInstance != null)
            BackGroundManager.bgInstance.Audio.Stop();
       
        var mazeLevel = MazePlayMode.currentMazeLevel;

        mazeTitle.text = $"Title: {mazeLevel.title}\nBy: {mazeLevel.creator}";
        var textureFromFile = MazePlayMode.currentMazeLevel.mazeTexture;
        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));


//#if !UNITY_WEBGL
//        var imageAssetBytes = File.ReadAllBytes(mazeLevel.mazeTextureFileName);
//        Texture2D textureFromFile = new Texture2D(2, 2);
//        textureFromFile.LoadImage(imageAssetBytes);
//        textureFromFile.name = mazeLevel.mazeTextureFileName;
//        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
//        mazeLevel.mazeTexture = textureFromFile;   // pass this on to PlayMode
//#endif

        ListTopScores(EnumMazePlayMode.PlayMode2D);
        ListTopScores(EnumMazePlayMode.PlayMode3D);

        Play2DButton.onClick.AddListener(Play2DOnClick);
        Play3DButton.onClick.AddListener(Play3DOnClick);
        CancelButton.onClick.AddListener(CancelOnClick);
        EditButton.onClick.AddListener(EditOnClick);
    }

    void ListTopScores(EnumMazePlayMode mazePlayMode)
    {
        var placeSting = new string[] {  "1st", "2nd", "3rd", "4th", "5th"  };
        var listOfTopScores = new ListOfHighScoresFromDataBase();
        listOfTopScores.CreateTableForListOfHighScoresIfNotExists();
        listOfTopScores.GetListOfHighScoresFromDataBase(MazePlayMode.currentMazeLevel.mazeId, mazePlayMode);
        for (int i=0; i<5; i++)
        {
            var thisLinesValue = $"{placeSting[i]} ";
            if (listOfTopScores.highScores.Count <= i)
            {
                thisLinesValue += "--:--.-- xxxxx ............";
            } 
            else 
            {
                thisLinesValue += (string)TimeSpan.FromSeconds((float)listOfTopScores.highScores[i].timeInOneHundredsOfSeconds / 100.0f).ToString("mm':'ss'.'ff") + " ";
                thisLinesValue += listOfTopScores.highScores[i].scoreAwarded.ToString("D5") + " ";
                thisLinesValue += listOfTopScores.highScores[i].playerName.Length <= 12 ? 
                    listOfTopScores.highScores[i].playerName.PadRight(12) : 
                    listOfTopScores.highScores[i].playerName.Substring(0, 12);
            }
            if (mazePlayMode == EnumMazePlayMode.PlayMode2D)
            {
                place_2D_text_list[i].text = thisLinesValue;
            }
            else
            {
                place_3D_text_list[i].text = thisLinesValue;
            }
        }
    }

    void Play2DOnClick()
    {
        MazePlayMode.mazePlayMode = EnumMazePlayMode.PlayMode2D;
        MazePlayMode.currentPlayerNewScore = null;
        SceneManager.LoadScene("Scenes/SketchADoodle");
    }

    void Play3DOnClick()
    {
        MazePlayMode.mazePlayMode = EnumMazePlayMode.PlayMode3D;
        MazePlayMode.currentPlayerNewScore = null;
        SceneManager.LoadScene("Scenes/SketchADoodle");
    }

    void CancelOnClick()
    {
        MazePlayMode.currentPlayerNewScore = null;
        SceneManager.LoadScene("Scenes/Intro");
    }

    void EditOnClick()
    {
        SceneManager.LoadScene("Scenes/EditMazeLevel");
    }

//    void Update()
//    {
//#if UNITY_WEBGL
//        StartCoroutine(GetTexture());
//#endif

//    }

//    IEnumerator GetTexture()
//    {
//        var mazeLevel = MazePlayMode.currentMazeLevel;

//        using (UnityWebRequest loader = UnityWebRequestTexture.GetTexture(mazeLevel.mazeTextureFileName))
//        {
//            yield return loader.SendWebRequest();

//            if (string.IsNullOrEmpty(loader.error))
//            {
//                var textureFromFile = DownloadHandlerTexture.GetContent(loader);
//                textureFromFile.name = mazeLevel.mazeTextureFileName;
//                TextureScale.Scale(textureFromFile, 200, 200);

//                mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
//                mazeLevel.mazeTexture = textureFromFile; // pass this on to PlayMode
//            }
//            else
//            {
//                Debug.LogError($"Error loading Texture '{loader.uri}': {loader.error}");
//            }
//        }
//    }
}

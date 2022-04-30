using Assets.Scripts.DataObjects;
using Assets.Scripts.DataProviders;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditMaze : MonoBehaviour
{
    [SerializeField] private CurrentMazeLevel_ScriptableObject level_SO;
    [SerializeField] private InputField titleInput;
    [SerializeField] private InputField creatorInput;
    [SerializeField] private Text fileName;
    [SerializeField] private Image mazeImage;
    [SerializeField] private GameObject startPositionMarker;
    [SerializeField] private GameObject endPositionMarker;

    [SerializeField] private Button SaveButton;
    [SerializeField] private Button CancelButton;

    private MazeLevel mazeLevel;
    private float panelWidth;
    private float panelHeight;

    // Start is called before the first frame update
    void Start()
    {
        mazeLevel = level_SO.CurrentMazeLevel_DO;

        titleInput.text = mazeLevel.title;
        creatorInput.text = mazeLevel.creator;
        fileName.text = $"Filename: {mazeLevel.mazeTextureFileName}";
        var imageAssetBytes = File.ReadAllBytes(mazeLevel.mazeTextureFileName);
        Texture2D textureFromFile = new Texture2D(2, 2);
        textureFromFile.LoadImage(imageAssetBytes);
        textureFromFile.name = mazeLevel.mazeTextureFileName;
        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
        panelWidth = GetComponent<RectTransform>().rect.width;
        panelHeight = GetComponent<RectTransform>().rect.height;
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(mazeLevel.startPositionRatio.x * panelWidth, mazeLevel.startPositionRatio.y * panelHeight);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(mazeLevel.endPositionRatio.x * panelWidth, mazeLevel.endPositionRatio.y * panelHeight);
        SaveButton.onClick.AddListener(SaveOnClick);
        CancelButton.onClick.AddListener(CancelOnClick);
    }

    void SaveOnClick()
    {
        Debug.Log("You have clicked the SAVE button!");

        mazeLevel.title = titleInput.text;
        mazeLevel.creator = creatorInput.text;
        mazeLevel.startPositionRatio.x = startPositionMarker.GetComponent<RectTransform>().anchoredPosition.x / panelWidth;
        mazeLevel.startPositionRatio.y = startPositionMarker.GetComponent<RectTransform>().anchoredPosition.y / panelHeight;
        mazeLevel.endPositionRatio.x = endPositionMarker.GetComponent<RectTransform>().anchoredPosition.x / panelWidth;
        mazeLevel.endPositionRatio.y = endPositionMarker.GetComponent<RectTransform>().anchoredPosition.y / panelHeight;

        var listOfMazesFromDataBase = new ListOfMazesFromDataBase();
        listOfMazesFromDataBase.UpdateMaze(mazeLevel.mazeId, mazeLevel);
        SceneManager.LoadScene("Scenes/Intro");
    }

    void CancelOnClick()
    {
        Debug.Log("You have clicked the CANCEL button!");
        SceneManager.LoadScene("Scenes/Intro");
    }
}

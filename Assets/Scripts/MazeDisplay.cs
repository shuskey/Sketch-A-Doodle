using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using Assets.Scripts.DataObjects;
using Assets.Scripts.DataProviders;

public class MazeDisplay : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MazeLevel maze_DO;    
    [SerializeField] private CurrentMazeLevel_ScriptableObject currentLevel_DO;

    [SerializeField] private Image mazeImage;
    [SerializeField] private GameObject startPositionMarker;
    [SerializeField] private GameObject endPositionMarker;
    [SerializeField] private Button editButton;
    [SerializeField] private Text fileName;

    // Start is called before the first frame update
    void Start()
    {
        var imageAssetBytes = File.ReadAllBytes(maze_DO.mazeTextureFileName);
        Texture2D textureFromFile = new Texture2D(2, 2);
        textureFromFile.LoadImage(imageAssetBytes);
        textureFromFile.name = maze_DO.mazeTextureFileName;
        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
        
        var panelWidth = GetComponent<RectTransform>().rect.width;
        var panelHeight = GetComponent<RectTransform>().rect.height;
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(maze_DO.startPositionRatio.x * panelWidth, maze_DO.startPositionRatio.y * panelHeight);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(maze_DO.endPositionRatio.x * panelWidth, maze_DO.endPositionRatio.y * panelHeight);
        editButton.onClick.AddListener(EditOnClick);
        fileName.text = maze_DO.mazeTextureFileName;
    }

    public void Initialize(MazeLevel maze_DO_toUse)
    {
        maze_DO = maze_DO_toUse;
    }

    void EditOnClick()
    {
        currentLevel_DO.CurrentMazeLevel_DO = maze_DO;
        SceneManager.LoadScene("Scenes/EditMazeLevel");        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        currentLevel_DO.CurrentMazeLevel_DO = maze_DO;        
        SceneManager.LoadScene("Scenes/ChoosePlayMode");
    }
}

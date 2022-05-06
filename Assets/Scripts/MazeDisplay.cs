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
    [SerializeField] private MazeLevel mazeLevel;    

    [SerializeField] private Image mazeImage;
    [SerializeField] private GameObject startPositionMarker;
    [SerializeField] private GameObject endPositionMarker;
    [SerializeField] private Button editButton;

    // Start is called before the first frame update
    void Start()
    {
        var imageAssetBytes = File.ReadAllBytes(mazeLevel.mazeTextureFileName);
        Texture2D textureFromFile = new Texture2D(2, 2);
        textureFromFile.LoadImage(imageAssetBytes);
        textureFromFile.name = mazeLevel.mazeTextureFileName;
        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
        
        var panelWidth = GetComponent<RectTransform>().rect.width;
        var panelHeight = GetComponent<RectTransform>().rect.height;
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(mazeLevel.startPositionRatio.x * panelWidth, mazeLevel.startPositionRatio.y * panelHeight);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(mazeLevel.endPositionRatio.x * panelWidth, mazeLevel.endPositionRatio.y * panelHeight);
        editButton.onClick.AddListener(EditOnClick);
    }

    public void Initialize(MazeLevel maze_DO_toUse)
    {
        mazeLevel = maze_DO_toUse;
    }

    void EditOnClick()
    {     
        MazePlayMode.currentMazeLevel = mazeLevel;
        SceneManager.LoadScene("Scenes/EditMazeLevel");        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {         
        MazePlayMode.currentMazeLevel = mazeLevel;
        SceneManager.LoadScene("Scenes/ChoosePlayMode");
    }
}

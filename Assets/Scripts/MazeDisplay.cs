using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class MazeDisplay : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MazeLevel_ScriptableObject maze_SO;
    [SerializeField] private CurrentMazeLevel_ScriptableObject currentLevel_SO;
    [SerializeField] private Image mazeImage;
    [SerializeField] private GameObject startPositionMarker;
    [SerializeField] private GameObject endPositionMarker;
    [SerializeField] private Button editButton;
    [SerializeField] private Text fileName;

    // Start is called before the first frame update
    void Start()
    {

        if (maze_SO.mazeTexture == null)
        {
            var imageAssetBytes = File.ReadAllBytes(maze_SO.mazeTextureFileName);
            Texture2D textureFromFile = new Texture2D(2, 2);
            textureFromFile.LoadImage(imageAssetBytes);
            textureFromFile.name = maze_SO.name;
            mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            mazeImage.sprite = Sprite.Create(maze_SO.mazeTexture, new Rect(0.0f, 0.0f, maze_SO.mazeTexture.width, maze_SO.mazeTexture.height), new Vector2(0.5f, 0.5f));
        }
        var panelWidth = GetComponent<RectTransform>().rect.width;
        var panelHeight = GetComponent<RectTransform>().rect.height;
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(maze_SO.startPositionRatio.x * panelWidth, maze_SO.startPositionRatio.y * panelHeight);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(maze_SO.endPositionRatio.x * panelWidth, maze_SO.endPositionRatio.y * panelHeight);
        editButton.onClick.AddListener(EditOnClick);
        fileName.text = maze_SO.name;
    }

    public void Initialize(MazeLevel_ScriptableObject maze_SO_toUse)
    {
        maze_SO = maze_SO_toUse;
    }

    void EditOnClick()
    {
        currentLevel_SO.CurrentMazeLevel = maze_SO;
        EditorUtility.SetDirty(currentLevel_SO);
        AssetDatabase.SaveAssets();

        SceneManager.LoadScene("Scenes/EditMazeLevel");
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        currentLevel_SO.CurrentMazeLevel = maze_SO;
        EditorUtility.SetDirty(currentLevel_SO);
        AssetDatabase.SaveAssets();

        SceneManager.LoadScene("Scenes/ChoosePlayMode");
    }
}

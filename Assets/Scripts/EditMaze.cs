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

    private MazeLevel_ScriptableObject maze_ScriptableObject;
    private float panelWidth;
    private float panelHeight;

    // Start is called before the first frame update
    void Start()
    {
        maze_ScriptableObject = level_SO.CurrentMazeLevel;

        titleInput.text = maze_ScriptableObject.title;
        creatorInput.text = maze_ScriptableObject.creator;
        fileName.text = $"Filename: {maze_ScriptableObject.name}";
        if (maze_ScriptableObject.mazeTexture == null)
        {
            var imageAssetBytes = File.ReadAllBytes(maze_ScriptableObject.mazeTextureFileName);
            Texture2D textureFromFile = new Texture2D(2, 2);
            textureFromFile.LoadImage(imageAssetBytes);
            textureFromFile.name = maze_ScriptableObject.name;
            mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            mazeImage.sprite = Sprite.Create(maze_ScriptableObject.mazeTexture, new Rect(0.0f, 0.0f, maze_ScriptableObject.mazeTexture.width, maze_ScriptableObject.mazeTexture.height), new Vector2(0.5f, 0.5f));
        }
        panelWidth = GetComponent<RectTransform>().rect.width;
        panelHeight = GetComponent<RectTransform>().rect.height;
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(maze_ScriptableObject.startPositionRatio.x * panelWidth, maze_ScriptableObject.startPositionRatio.y * panelHeight);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(maze_ScriptableObject.endPositionRatio.x * panelWidth, maze_ScriptableObject.endPositionRatio.y * panelHeight);
        SaveButton.onClick.AddListener(SaveOnClick);
        CancelButton.onClick.AddListener(CancelOnClick);
    }

    void SaveOnClick()
    {
        Debug.Log("You have clicked the SAVE button!");

        maze_ScriptableObject.title = titleInput.text;
        maze_ScriptableObject.creator = creatorInput.text;
        maze_ScriptableObject.startPositionRatio.x = startPositionMarker.GetComponent<RectTransform>().anchoredPosition.x / panelWidth;
        maze_ScriptableObject.startPositionRatio.y = startPositionMarker.GetComponent<RectTransform>().anchoredPosition.y / panelHeight;
        maze_ScriptableObject.endPositionRatio.x = endPositionMarker.GetComponent<RectTransform>().anchoredPosition.x / panelWidth;
        maze_ScriptableObject.endPositionRatio.y = endPositionMarker.GetComponent<RectTransform>().anchoredPosition.y / panelHeight;

        //// Now flag the object as "dirty" in the editor so it will be saved
        EditorUtility.SetDirty(maze_ScriptableObject);

        //// And finally, prompt the editor database to save dirty assets, committing your changes to disk.
        AssetDatabase.SaveAssets();

        SceneManager.LoadScene("Scenes/Intro");
    }

    void CancelOnClick()
    {
        Debug.Log("You have clicked the CANCEL button!");
        SceneManager.LoadScene("Scenes/Intro");
    }
}

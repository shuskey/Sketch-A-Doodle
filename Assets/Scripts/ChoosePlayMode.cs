using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoosePlayMode : MonoBehaviour
{
    [SerializeField] private CurrentMazeLevel_ScriptableObject level_SO;
    [SerializeField] private Image mazeImage;
    [SerializeField] private Text mazeTitle;

    [SerializeField] private Button Play2DButton;
    [SerializeField] private Button Play3DButton;
    [SerializeField] private Button CancelButton;

    // Start is called before the first frame update
    void Start()
    {
        if (BackGroundManager.bgInstance != null)
            BackGroundManager.bgInstance.Audio.Stop();

        var maze_ScriptableObject = level_SO.CurrentMazeLevel;

        mazeTitle.text = $"Title: {maze_ScriptableObject.title}, Creator: {maze_ScriptableObject.creator}";

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
        Play2DButton.onClick.AddListener(Play2DOnClick);
        Play3DButton.onClick.AddListener(Play3DOnClick);
        CancelButton.onClick.AddListener(CancelOnClick);
    }

    void Play2DOnClick()
    {
        MazePlayMode.mazePlayMode = EnumMazePlayMode.PlayMode2D;
           
        SceneManager.LoadScene("Scenes/SketchADoodle");
    }

    void Play3DOnClick()
    {
        MazePlayMode.mazePlayMode = EnumMazePlayMode.PlayMode3D;

        SceneManager.LoadScene("Scenes/SketchADoodle");
    }

    void CancelOnClick()
    {
        SceneManager.LoadScene("Scenes/Intro");
    }
}

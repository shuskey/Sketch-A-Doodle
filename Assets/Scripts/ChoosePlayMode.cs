using Assets.Scripts.Enums;
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

        var mazeLevel = level_SO.CurrentMazeLevel_DO;

        mazeTitle.text = $"Title: {mazeLevel.title}, Creator: {mazeLevel.creator}";

        var imageAssetBytes = File.ReadAllBytes(mazeLevel.mazeTextureFileName);
        Texture2D textureFromFile = new Texture2D(2, 2);
        textureFromFile.LoadImage(imageAssetBytes);
        textureFromFile.name = mazeLevel.mazeTextureFileName;
        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));

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

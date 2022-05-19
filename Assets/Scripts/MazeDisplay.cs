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
using UnityEngine.Networking;

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
        Texture2D textureFromFile = new Texture2D(2, 2);

#if !UNITY_WEBGL
        var imageAssetBytes = File.ReadAllBytes(mazeLevel.mazeTextureFileName);
        textureFromFile.LoadImage(imageAssetBytes);
        textureFromFile.name = mazeLevel.mazeTextureFileName;
        TextureScale.Scale(textureFromFile, 200, 200);

        mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
        mazeLevel.mazeTexture = textureFromFile; // pass this on to PlayMode

#endif

        var panelWidth = GetComponent<RectTransform>().rect.width;
        var panelHeight = GetComponent<RectTransform>().rect.height;
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(mazeLevel.startPositionRatio.x * panelWidth, mazeLevel.startPositionRatio.y * panelHeight);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(mazeLevel.endPositionRatio.x * panelWidth, mazeLevel.endPositionRatio.y * panelHeight);
        editButton.onClick.AddListener(EditOnClick);
    }

    void Update()
    {
#if UNITY_WEBGL
        StartCoroutine(GetTexture());
#endif

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

    IEnumerator GetTexture()
    {
        using (UnityWebRequest loader = UnityWebRequestTexture.GetTexture(mazeLevel.mazeTextureFileName))
        {
            yield return loader.SendWebRequest();

            if (string.IsNullOrEmpty(loader.error))
            {
                var textureFromFile = DownloadHandlerTexture.GetContent(loader);
                textureFromFile.name = mazeLevel.mazeTextureFileName;
                TextureScale.Scale(textureFromFile, 200, 200);

                mazeImage.sprite = Sprite.Create(textureFromFile, new Rect(0.0f, 0.0f, textureFromFile.width, textureFromFile.height), new Vector2(0.5f, 0.5f));
                mazeLevel.mazeTexture = textureFromFile; // pass this on to PlayMode                
            }
            else
            {
                Debug.LogError($"Error loading Texture '{loader.uri}': {loader.error}");
            }
        }
    }
}

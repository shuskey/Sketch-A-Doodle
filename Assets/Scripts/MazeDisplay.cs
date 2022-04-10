using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeDisplay : MonoBehaviour
{
    [SerializeField]
    private MazeLevel_ScriptableObject maze;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text creatorText;
    [SerializeField]
    private Image mazeImage;
    [SerializeField]
    private GameObject startPositionMarker;
    [SerializeField]
    private GameObject endPositionMarker;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = maze.name;
        creatorText.text = maze.creator;
        mazeImage.sprite = Sprite.Create(maze.mazeTexture, new Rect(0.0f, 0.0f, maze.mazeTexture.width, maze.mazeTexture.height), new Vector2(0.5f, 0.5f));
        startPositionMarker.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(maze.startPositionRatio.x * mazeImage.rectTransform.sizeDelta.x, maze.startPositionRatio.y * mazeImage.rectTransform.sizeDelta.y);
        endPositionMarker.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(maze.endPositionRatio.x * mazeImage.rectTransform.sizeDelta.x, maze.endPositionRatio.y * mazeImage.rectTransform.sizeDelta.y);
    }
}

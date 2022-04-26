using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class HeightMapFromMazeLevel : MonoBehaviour
{
    [SerializeField]
    private CurrentMazeLevel_ScriptableObject currentMazeLevel;
    [SerializeField]
    private GameObject startingPlatform_GameObject;
    [SerializeField]
    private GameObject endingGoal_GameObject;
    [SerializeField]
    private TMP_Text mazeCreditsText;

    [SerializeField] private GameObject miniMapCamera;
    [SerializeField] private GameObject mapToggleButton;

    [SerializeField] private GameObject playerArmature;

    [SerializeField] private GameObject pointerSphere;

    private readonly Rect fullScreenViewportRect = new Rect(0, 0, 1, 1);
    private readonly Rect minimapViewportRect = new Rect(0.7f, 0.6f, 0.3f, 0.4f);

    // Start is called before the first frame update
    void Start()
    {
        SetPlayMode();  // 2D or 3D

        ApplyHeightmap(currentMazeLevel, invertToUseBlackLines: currentMazeLevel.CurrentMazeLevel.invertToUseBlackLines);
        endingGoal_GameObject.transform.localPosition = new Vector3(
            currentMazeLevel.CurrentMazeLevel.endPositionRatio.x * 100f,
            1.3f,
            currentMazeLevel.CurrentMazeLevel.endPositionRatio.y * 100f);
        startingPlatform_GameObject.transform.localPosition = new Vector3(
            currentMazeLevel.CurrentMazeLevel.startPositionRatio.x * 100f,
            .15f,
            currentMazeLevel.CurrentMazeLevel.startPositionRatio.y * 100f);

        mazeCreditsText.text = $"Title: {currentMazeLevel.CurrentMazeLevel.title}, Creator: {currentMazeLevel.CurrentMazeLevel.creator}";       
    }

    void SetPlayMode()
    {
        var camera = miniMapCamera.GetComponent<Camera>();
        var currentPlayMode = MazePlayMode.mazePlayMode;
        switch (currentPlayMode)
        {
            case EnumMazePlayMode.PlayMode2D:            
                camera.rect = fullScreenViewportRect;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
                playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().enabled = true;
                playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().Restart();
                pointerSphere.GetComponent<Renderer>().enabled = false;                                
                break;

            default:    // 3D Mode
                camera.rect = minimapViewportRect;
                playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().enabled = false;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().Restart();
                pointerSphere.GetComponent<Renderer>().enabled = true;                               
                break;            
        }
    }


    static void ApplyHeightmap(CurrentMazeLevel_ScriptableObject currentMazeLevel, bool invertToUseBlackLines = false)
    {
        var heightmap = currentMazeLevel.CurrentMazeLevel.mazeTexture;
        if (heightmap == null)
        {
            var imageAssetBytes = File.ReadAllBytes(currentMazeLevel.CurrentMazeLevel.mazeTextureFileName);
            heightmap = new Texture2D(2, 2);
            heightmap.LoadImage(imageAssetBytes);
            heightmap.name = currentMazeLevel.CurrentMazeLevel.name;            
        }

        bool floorIsWhite = invertToUseBlackLines;
        float floorTotal;
        float floorCount;
        float floorAverageGray;

        var terrain = Terrain.activeTerrain.terrainData;
        int w = heightmap.width;
        int h = heightmap.height;
        int w2 = terrain.heightmapResolution;
        float[,] heightmapData = terrain.GetHeights(0, 0, w2, w2);
        Color[] mapColors = heightmap.GetPixels();
        Color[] map = new Color[w2 * w2];
        if (w2 != w || h != w)
        {
            // Resize using nearest-neighbor scaling if texture has no filtering
            if (heightmap.filterMode == FilterMode.Point)
            {
                float dx = (float)w / (float)w2;
                float dy = (float)h / (float)w2;
                for (int y = 0; y < w2; y++)
                {
                    int thisY = Mathf.FloorToInt(dy * y) * w;
                    int yw = y * w2;
                    for (int x = 0; x < w2; x++)
                    {
                        map[yw + x] = mapColors[Mathf.FloorToInt(thisY + dx * x)];
                    }
                }
            }
            // Otherwise resize using bilinear filtering
            else
            {
                float ratioX = (1.0f / ((float)w2 / (w - 1)));
                float ratioY = (1.0f / ((float)w2 / (h - 1)));
                for (int y = 0; y < w2; y++)
                {
                    int yy = Mathf.FloorToInt(y * ratioY);
                    int y1 = yy * w;
                    int y2 = (yy + 1) * w;
                    int yw = y * w2;
                    for (int x = 0; x < w2; x++)
                    {
                        int xx = Mathf.FloorToInt(x * ratioX);
                        Color bl = mapColors[y1 + xx];
                        Color br = mapColors[y1 + xx + 1];
                        Color tl = mapColors[y2 + xx];
                        Color tr = mapColors[y2 + xx + 1];
                        float xLerp = x * ratioX - xx;
                        map[yw + x] = Color.Lerp(Color.Lerp(bl, br, xLerp), Color.Lerp(tl, tr, xLerp), y * ratioY - (float)yy);
                    }
                }
            }
        }
        else
        {
            // Use original if no resize is needed
            map = mapColors;
        }

        DetectFloor(invertToUseBlackLines, out floorTotal, out floorCount, out floorAverageGray, w2, map);

        // Assign texture data to heightmap
        for (int y = 0; y < w2; y++)
        {
            for (int x = 0; x < w2; x++)
            {
                heightmapData[y, x] = normalizeHeightData(
                    map[y * w2 + x].grayscale,
                    floorAverageGray,
                    invertToUseBlackLines);
            }
        }
        terrain.SetHeights(0, 0, heightmapData);

        static void DetectFloor(bool invertToUseBlackLines, out float floorTotal, out float floorCount, out float floorAverageGray, int w2, Color[] map)
        {
            // find floor Average
            float expectedFloor = 0.3f;
            floorCount = 0;
            floorTotal = 0;
            for (int y = 0; y < w2; y++)
            {
                for (int x = 0; x < w2; x++)
                {
                    var grayBeingProcessed = invertToUseBlackLines ? 1f - map[y * w2 + x].grayscale : map[y * w2 + x].grayscale;
                    if (grayBeingProcessed < expectedFloor)
                    {
                        floorTotal += grayBeingProcessed;
                        floorCount++;
                    }
                }
            }
            if (floorCount == 0)
                floorAverageGray = expectedFloor;
            else
                floorAverageGray = floorTotal / floorCount;
        }
    }

    static float normalizeHeightData(float grayscaleInput, float floorAverageGray, bool invertToUseBlackLines)
    {
        var grayToUse = invertToUseBlackLines ? 1f - grayscaleInput : grayscaleInput;
        var rounded = Mathf.Round(grayToUse * 10f) / 10f;
        var floorLimit = (floorAverageGray + 0.1f) * 2.0f;
        return (rounded <= floorLimit) ? 0f : rounded;
    }
}

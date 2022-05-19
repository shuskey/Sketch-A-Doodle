using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Assets.Scripts.Enums;

public class HeightMapFromMazeLevel : MonoBehaviour
{
    [SerializeField] private GameObject startingPlatform_GameObject;
    [SerializeField] private GameObject endingGoal_GameObject;

    [SerializeField] private GameObject miniMapCamera;
    [SerializeField] private GameObject mapToggleButton;

    [SerializeField] private GameObject playerArmature;

    [SerializeField] private GameObject pointerSphere;

    private readonly Rect fullScreenViewportRect = new Rect(0, 0, 1, 1);
    private readonly Rect minimapViewportRect = new Rect(0.7f, 0.0f, 0.3f, 0.4f);

    private void Awake()
    {
       SetPlayMode(allDisabled: true); // put a pause on the Character Controllers while we re-set the scene    
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplyHeightmap(invertToUseBlackLines: MazePlayMode.currentMazeLevel.invertToUseBlackLines);
        
        endingGoal_GameObject.transform.localPosition = new Vector3(
            MazePlayMode.currentMazeLevel.endPositionRatio.x * 100f,
            1.3f,
            MazePlayMode.currentMazeLevel.endPositionRatio.y * 100f);
 
        startingPlatform_GameObject.transform.localPosition = new Vector3(
            MazePlayMode.currentMazeLevel.startPositionRatio.x * 100f,
            0.0f,
            MazePlayMode.currentMazeLevel.startPositionRatio.y * 100f);

        SetPlayMode();  // 2D or 3D
    }

    void SetPlayMode(bool allDisabled = false)
    {
        if (allDisabled)
        { 
            playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
            playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().enabled = false;
            return;
        }
        
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

    void ApplyHeightmap(bool invertToUseBlackLines = false)
    {

        //var imageAssetBytes = File.ReadAllBytes(MazePlayMode.currentMazeLevel.mazeTextureFileName);
        //var heightmap = new Texture2D(2, 2);
        //heightmap.LoadImage(imageAssetBytes);
        //heightmap.name = MazePlayMode.currentMazeLevel.mazeTextureFileName;

        var heightmap = MazePlayMode.currentMazeLevel.mazeTexture;

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

        // break into 4 x 4 grids and do each grid separately
        for (int gridY = 0; gridY < 4; gridY++)
        {
            for (int gridX = 0; gridX < 4; gridX++)
            {
                DetectFloor(invertToUseBlackLines, out floorTotal, out floorCount, out floorAverageGray, w2, map, gridX, gridY);

                // Assign texture data to heightmap

                for (int y = QuadStart(gridY, w2); y <= QuadEnd(gridY, w2); y++)
                {
                    for (int x = QuadStart(gridX, w2); x <= QuadEnd(gridX, w2); x++)
                    {
                        heightmapData[y, x] = normalizeHeightData(
                            map[y * w2 + x].grayscale,
                            floorAverageGray,
                            invertToUseBlackLines);
                    }
                }
            }
        }

        terrain.SetHeights(0, 0, heightmapData);

        int QuadStart(int quadIndex, int w2) => (quadIndex * w2) / 4;

        int QuadEnd(int quadIndex, int w2) => QuadStart(quadIndex + 1, w2) - 1;
        
        void DetectFloor(bool invertToUseBlackLines, out float floorTotal, out float floorCount, out float floorAverageGray, int w2,
            Color[] map, int gridX, int gridY)
        {
            // find floor Average (for this quadrant)
            float greyTotal = 0;
            float greyCount = 0;
            for (int y = QuadStart(gridY, w2); y <= QuadEnd(gridY, w2); y++)
            {
                for (int x = QuadStart(gridX, w2); x <= QuadEnd(gridX, w2); x++)
                {
                    var grayBeingProcessed = map[y * w2 + x].grayscale;
                    greyTotal += grayBeingProcessed;
                    greyCount++;
                }
            }
            float averageForThisQuadrant = greyTotal / greyCount;

            // find floor Average (for this quadrant)
            float expectedFloor = (1.0f - averageForThisQuadrant) / 2.50f;
            // This denominator might make a great 'brightness' factor
            floorCount = 0;
            floorTotal = 0;
            for (int y = QuadStart(gridY, w2); y < QuadEnd(gridY, w2); y++)
            {
                for (int x = QuadStart(gridX, w2); x < QuadEnd(gridX, w2); x++)
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
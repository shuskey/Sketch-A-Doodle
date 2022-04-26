using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickToggleMiniMap: MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite miniMapOnIcon;
    [SerializeField] private Sprite miniMapFogOffIcon;
    [SerializeField] private Sprite miniMapCloseIcon;

    [SerializeField] private GameObject miniMapCamera;

    private enum MiniMapState
    {
        TurnOffFogNext,
        CloseMapNext,
        TurnOnFogNext
    }

    private MiniMapState currentState = MiniMapState.TurnOffFogNext;

    void Start()
    {
        currentState = MiniMapState.TurnOffFogNext;
        this.GetComponent<Image>().sprite = miniMapFogOffIcon;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        var fogOfWarPlane = GameObject.FindGameObjectsWithTag("FogOfWar");

        switch (currentState)
        {
            case MiniMapState.TurnOffFogNext:
                // find and turn off Fog of War                
                if (fogOfWarPlane != null && fogOfWarPlane[0] != null)
                    fogOfWarPlane[0].GetComponent<Renderer>().enabled = false;
                // Setup buttons for next transition
                if (MazePlayMode.mazePlayMode != EnumMazePlayMode.PlayMode2D)
                {
                    currentState = MiniMapState.CloseMapNext;
                    this.GetComponent<Image>().sprite = miniMapCloseIcon;
                }
                else
                {
                    currentState = MiniMapState.TurnOnFogNext;
                    this.GetComponent<Image>().sprite = miniMapOnIcon;
                }
                break;
                // 3D ONLY
            case MiniMapState.CloseMapNext:
                // turn fog of war back on
                if (fogOfWarPlane != null && fogOfWarPlane[0] != null)
                    fogOfWarPlane[0].GetComponent<Renderer>().enabled = true;
                // turn off mini map
                miniMapCamera.SetActive(false);
                // Setup buttons for next transition
                currentState = MiniMapState.TurnOnFogNext;
                this.GetComponent<Image>().sprite = miniMapOnIcon;
                break;
            case MiniMapState.TurnOnFogNext:
                // turn fog of war back on
                if (fogOfWarPlane != null && fogOfWarPlane[0] != null)
                    fogOfWarPlane[0].GetComponent<Renderer>().enabled = true;
                // turn off mini map
                miniMapCamera.SetActive(true);
                // Setup buttons for next transition
                currentState = MiniMapState.TurnOffFogNext;
                this.GetComponent<Image>().sprite = miniMapFogOffIcon;
                break;

            default:
                break;
        }
    }
}

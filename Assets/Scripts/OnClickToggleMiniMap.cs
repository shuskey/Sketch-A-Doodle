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
        OpenMapNext
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
                currentState = MiniMapState.CloseMapNext;
                this.GetComponent<Image>().sprite = miniMapCloseIcon;
                break;
            case MiniMapState.CloseMapNext:
                // turn fog of war back on
                if (fogOfWarPlane != null && fogOfWarPlane[0] != null)
                    fogOfWarPlane[0].GetComponent<Renderer>().enabled = true;
                // turn off mini map
                miniMapCamera.SetActive(false);
                currentState = MiniMapState.OpenMapNext;
                this.GetComponent<Image>().sprite = miniMapOnIcon;
                break;
            case MiniMapState.OpenMapNext:
                // turn on mini map
                miniMapCamera.SetActive(true);
                currentState = MiniMapState.TurnOffFogNext;
                this.GetComponent<Image>().sprite = miniMapFogOffIcon;
                break;
            default:
                break;
        }
    }
}

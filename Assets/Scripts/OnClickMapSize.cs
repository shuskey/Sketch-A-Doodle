using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickMapSize: MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite miniMapExpandIcon;
    [SerializeField] private Sprite miniMapShrinkIcon;

    [SerializeField] private GameObject miniMapCamera;    

    [SerializeField] private GameObject playerArmature;

    private readonly Rect fullScreenViewportRect = new Rect(0, 0, 1, 1);
    private readonly Rect minimapViewportRect = new Rect(0.7f, 0.6f, 0.3f, 0.4f);

    private enum MiniMapSizeState
    {
        FullScreenSizeNext,
        MiniMapSizeNext
    }

    private MiniMapSizeState currentState = MiniMapSizeState.FullScreenSizeNext;

    void Start()
    {
        currentState = MiniMapSizeState.FullScreenSizeNext;
        this.GetComponent<Image>().sprite = miniMapExpandIcon;
        playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
        playerArmature.GetComponent<StarterAssets.FirstPersonController>().enabled = true;
    }

    void Update()
    {
        if (currentState == MiniMapSizeState.MiniMapSizeNext)
            playerArmature.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        var camera = miniMapCamera.GetComponent<Camera>();
        switch (currentState)
        {
            case MiniMapSizeState.FullScreenSizeNext:                
                camera.rect = fullScreenViewportRect;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
                playerArmature.GetComponent<StarterAssets.FirstPersonController>().enabled = true;
                currentState = MiniMapSizeState.MiniMapSizeNext;
                this.GetComponent<Image>().sprite = miniMapShrinkIcon;
                break;
            case MiniMapSizeState.MiniMapSizeNext:
                camera.rect = minimapViewportRect;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
                playerArmature.GetComponent<StarterAssets.FirstPersonController>().enabled = false;
                currentState = MiniMapSizeState.FullScreenSizeNext;
                this.GetComponent<Image>().sprite = miniMapExpandIcon;
                break;

            default:
                break;
        }
    }
}

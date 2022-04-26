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
    [SerializeField] private GameObject mapToggleButton;

    [SerializeField] private GameObject playerArmature;

    [SerializeField] private GameObject pointerSphere;

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
        pointerSphere.GetComponent<Renderer>().enabled = true;
    }

    //void Update()
    //{
    //    if (currentState == MiniMapSizeState.MiniMapSizeNext)
    //    {

    //        // TODO help the controller maintain up/down left/right in 2D mode
    //        //playerArmature.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    //        //var cinimachineTargetGO = GameObject.FindGameObjectWithTag("CinemachineTarget");
    //        //cinimachineTargetGO.transform.rotation = Quaternion.Euler(0, 0, 0);
    //    }
    //}

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        var camera = miniMapCamera.GetComponent<Camera>();        
        switch (currentState)
        {
            case MiniMapSizeState.FullScreenSizeNext:     // 2D Mode            
                camera.rect = fullScreenViewportRect;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
                playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().enabled = true;
                playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().Restart();
                currentState = MiniMapSizeState.MiniMapSizeNext;
                pointerSphere.GetComponent<Renderer>().enabled = false;
           //     mapToggleButton.SetActive(false);
                this.GetComponent<Image>().sprite = miniMapShrinkIcon;
                break;
            case MiniMapSizeState.MiniMapSizeNext:       // 3D Mode
                camera.rect = minimapViewportRect;
                playerArmature.GetComponent<StarterAssets.FirstPersonMoveOnlyController>().enabled = false;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
                playerArmature.GetComponent<StarterAssets.ThirdPersonController>().Restart();
                currentState = MiniMapSizeState.FullScreenSizeNext;
                pointerSphere.GetComponent<Renderer>().enabled = true;
                mapToggleButton.SetActive(true);
                this.GetComponent<Image>().sprite = miniMapExpandIcon;
                break;

            default:
                break;
        }
    }
}

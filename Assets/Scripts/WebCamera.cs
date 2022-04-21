using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WebCamera : MonoBehaviour
{
   // static WebCamTexture webCamTexture;
    [SerializeField] private MeshRenderer meshRenderer;

    private WebCamTexture webcamTexture;

    private void Start()
    {        
        var devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, 1920, 1000);
        meshRenderer = GetComponent<MeshRenderer>();        
        meshRenderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    public void StopWebcam()
    {
        webcamTexture.Stop();
    }

    public void StopWebcamAndCancel()
    {
        webcamTexture.Stop();
        SceneManager.LoadScene("Scenes/Intro");
    }
}

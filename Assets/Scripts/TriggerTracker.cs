using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerTracker : MonoBehaviour
{
    Timer timerScript;

    // Start is called before the first frame update
    void Start()
    {
        timerScript = gameObject.GetComponent<Timer>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {

        } 
        else if (other.gameObject.CompareTag("WinTheLevel"))
        {
            BackGroundManager.bgInstance.Audio.Stop();
            SfxManager.sfxInstance.Audio.PlayOneShot(SfxManager.sfxInstance.Cherry);
            timerScript.StopTimer();
            Destroy(other.gameObject);
        } 

        else if (other.gameObject.CompareTag("RestartTheLevel"))
        {
            SceneManager.LoadScene("SketchADoodle");
        }
        else if (other.gameObject.CompareTag("StartFallingRegion"))
        {
            SfxManager.sfxInstance.Audio.PlayOneShot(SfxManager.sfxInstance.Falling);
        }
    }
}

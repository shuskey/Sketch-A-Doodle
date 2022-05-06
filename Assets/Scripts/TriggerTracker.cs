using Assets.Scripts.DataObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerTracker : MonoBehaviour
{
    [SerializeField] private GameObject yourNewScoreObject;
    [SerializeField] private GameObject trackingSphereObject;

    Timer timerScript;

    // Start is called before the first frame update
    void Start()
    {
        timerScript = gameObject.GetComponent<Timer>();
        yourNewScoreObject.SetActive(false);
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

            int elapsedTimeInHundreths = (int)(timerScript.GetElapsedTime() * 100);
            /*
             *  - Is this a personal best for the current player
                  No, Great Time, but not a personal best (let user change current player for a different evaluation)
	              Yes, Excellent, New Personal Speed Record!  This puts you in 10th place (or what ever)
                      - Button choices [Try Again (goes to playmode chooser screen)] [Home/Back/(back to play this maze)]  <If new personal record, then save New High Score>
             */
            MazePlayMode.currentPlayerNewScore = new MazeHighScore(
                MazePlayMode.currentMazeLevel.mazeId,
                MazePlayMode.mazePlayMode,
                MazePlayMode.currentPlayer,
                System.DateTime.Now.Date,
                scoreAwarded: 0,
                elapsedTimeInHundreths);

            var yourNewScoreScript = yourNewScoreObject.GetComponent<YourNewScoreScript>();

            yourNewScoreScript.GoalAchieved();
            yourNewScoreObject.SetActive(true);

            gameObject.SetActive(false); // Pause
        }

        else if (other.gameObject.CompareTag("RestartTheLevel"))
        {
            SceneManager.LoadScene("Scenes/SketchADoodle");
        }
        else if (other.gameObject.CompareTag("StartFallingRegion"))
        {
            trackingSphereObject.SetActive(false);
            SfxManager.sfxInstance.Audio.PlayOneShot(SfxManager.sfxInstance.Falling);
        }
    }
}

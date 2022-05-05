using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnClickGoBack : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneToGoBackTo = "Scenes/ChoosePlayMode";
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        SceneManager.LoadScene(sceneToGoBackTo);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForOnEnterPressed : MonoBehaviour
{
    [SerializeField] private GameObject destinationGameObjectForOnEnterPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterPressed()
    {        
        destinationGameObjectForOnEnterPressed.SendMessage("OnEnterPressed");
    }
}

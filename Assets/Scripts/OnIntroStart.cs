using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnIntroStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (BackGroundManager.bgInstance != null)
            BackGroundManager.bgInstance.Audio.Stop();              
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

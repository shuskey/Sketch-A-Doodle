using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float secondsUntilDestroy = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Object.Destroy(gameObject, secondsUntilDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private GameObject rightFootprint;
    [SerializeField]
    private GameObject leftFootprint;
    [SerializeField]
    private Transform rightFootLocation;
    [SerializeField]
    private Transform leftFootLocation;
    [SerializeField]
    private float footprintOffset = 0.05f;
    [SerializeField]
    private AudioClip land;

    [SerializeField]
    private AudioClip[] clips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void RightFootstep()
    {
        var clip = GetRandomClip();
        audioSource.PlayOneShot(clip);

        //Raycast out and create footprint
        RaycastHit hit;
        if (Physics.Raycast(rightFootLocation.position, rightFootLocation.forward, out hit))
        {
            Instantiate(rightFootprint, hit.point + hit.normal * footprintOffset, Quaternion.LookRotation(hit.normal, rightFootLocation.up));
        }
    }

    private void LeftFootstep()
    {
        var clip = GetRandomClip();
        audioSource.PlayOneShot(clip);

        //Raycast out and create footprint
        RaycastHit hit;
        if (Physics.Raycast(leftFootLocation.position, leftFootLocation.forward, out hit))
        {
            Instantiate(leftFootprint, hit.point + hit.normal * footprintOffset, Quaternion.LookRotation(hit.normal, leftFootLocation.up));
        }
    }

    private void Step()
    {
        var clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }
    private void Land()
    {
        audioSource.PlayOneShot(land);
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

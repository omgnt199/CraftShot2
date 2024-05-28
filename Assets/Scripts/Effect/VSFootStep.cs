using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSFootStep : MonoBehaviour
{
    [Header("Ground")]
    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public AudioSource Audio;
    [SerializeField] private AudioClip FootStepClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

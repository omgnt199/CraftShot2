using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSPlayerSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource FootStepSound;
    public AudioSource JumpDownSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableFootStep() => FootStepSound.enabled = true;
    public void DisableFootStep() => FootStepSound.enabled = false;
    public void EnableJumpDown() => JumpDownSound.Play();
}

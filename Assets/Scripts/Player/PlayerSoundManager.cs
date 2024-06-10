using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource FootStepSound;
    public AudioSource JumpDownSound;
    public AudioSource DashSound;
    public AudioSource BulletSound;
    public void EnableFootStepSound() => FootStepSound.enabled = true;
    public void DisableFootStepSound() => FootStepSound.enabled = false;
    public void EnableJumpDownSound() => JumpDownSound.Play();
    public void EnableDashSound() => DashSound.Play();
    public void EnableBulletSound(AudioClip bulletSound) => DashSound.PlayOneShot(bulletSound);
}

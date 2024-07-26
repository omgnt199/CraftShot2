using System.Collections;
using UnityEngine;

public class BotControlAnimator : MonoBehaviour
{
    public Animator Controller;


    public void Shoot()
    {
        Controller?.SetBool("IsIdle", false);
        Controller?.SetBool("IsShoot", true);
    }
    public void Idle()
    {
        Controller?.SetBool("IsIdle", true);
        Controller?.SetBool("IsShoot", false);
    }
}
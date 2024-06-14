using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public static int frames = 0;

    // Update is called once per frame
    void Update()
    {
        frames++;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(200, 100, 300, 50), "started since: " + Time.realtimeSinceStartup);
        GUI.Label(new Rect(200, 150, 300, 50), "frames : " + frames);
    }
}
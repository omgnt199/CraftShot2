using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class VSReloadingTextEffect : MonoBehaviour
{
    public Text text;
    private void OnEnable()
    {
        string textTo = "Reloading...";
        text.DOText(textTo, 0.5f).SetLoops(-1, LoopType.Restart);
    }
    private void OnDisable()
    {
        text.DOKill();
        text.text= "Reloading";
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

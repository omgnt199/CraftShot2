using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class VSTakeDamageUI : MonoBehaviour
{
    private void OnEnable()
    {
        if (!DOTween.IsTweening(GetComponent<Image>()))
            GetComponent<Image>().DOColor(new Color32(255,255,255,100), 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(()=> 
            {
                gameObject.SetActive(false);
                for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
            });
    }
    private void OnDisable()
    {

    }
}

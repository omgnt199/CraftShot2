using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalContentScrollViewAutoSize : MonoBehaviour
{
    public Transform Content;
    public Transform ContentChild;
    public int MaxRowView;
    private int _currentRow;
    private float _Spacing;
    private Vector2 _sizeDeltaContentChild;
    private void Awake()
    {
        _Spacing = Content.GetComponent<VerticalLayoutGroup>().spacing;
        _sizeDeltaContentChild = ContentChild.GetComponent<RectTransform>().sizeDelta;
    }
    private void OnEnable()
    {
        _currentRow = MaxRowView;
    }

    // Update is called once per frame
    void Update()
    {
        if(Content.childCount != _currentRow)
        {
            Content.GetComponent<RectTransform>().sizeDelta += new Vector2(0, (Content.childCount - _currentRow) * (_Spacing + _sizeDeltaContentChild.y));
            _currentRow = Content.childCount;
        }
    }
}

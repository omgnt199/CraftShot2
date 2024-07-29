using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkinSpinUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject SkinSpinedLocate;
    public float deltaRotateY = 5f;
    private Vector2 beginDragPos;
    private Vector2 currentDragPos, lastDragPos;
    public bool IsAutoRotate;
    bool isDragging = false;

    // Update is called once per frame
    void Update()
    {
        if (IsAutoRotate)
        {
            if (!isDragging)
            {
                SkinSpinedLocate.transform.eulerAngles = SkinSpinedLocate.transform.eulerAngles + new Vector3(0, deltaRotateY * Time.deltaTime * 10f, 0);
            }
        }
    }

    public void Reset() => SkinSpinedLocate.transform.eulerAngles = new Vector3(0, 180f, 0);
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragPos = eventData.position;
        lastDragPos = beginDragPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        currentDragPos = eventData.position;
        if (currentDragPos.x < lastDragPos.x)
        {
            Vector3 currentRotate = SkinSpinedLocate.transform.eulerAngles;
            SkinSpinedLocate.transform.eulerAngles = currentRotate + new Vector3(0, deltaRotateY, 0);
        }
        else
        {
            Vector3 currentRotate = SkinSpinedLocate.transform.eulerAngles;
            SkinSpinedLocate.transform.eulerAngles = currentRotate + new Vector3(0, -deltaRotateY, 0);
        }
        lastDragPos = currentDragPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}

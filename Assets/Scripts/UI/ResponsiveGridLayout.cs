using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ResponsiveGridLayout : LayoutGroup, ILayoutSelfController
{
    [SerializeField]
    float cellRatio = 1;
    [SerializeField]
    float hSpacing, vSpacing;
    float hS, vS;
    [SerializeField]
    bool isRelative = false;
    [SerializeField]
    bool autoFitHeight = true;
    [SerializeField]
    public int columns = 1;
    [SerializeField]
    public int rows = 1;

    public bool dependHeight = false;
    public bool autoCenter = false;

    private DrivenRectTransformTracker m_Tracker;
    public override void CalculateLayoutInputVertical()
    {
        if (columns == 0)
            return;
        base.CalculateLayoutInputHorizontal();
        float pWidth = rectTransform.rect.width - padding.horizontal;


        if (isRelative)
        {
            hS = hSpacing * pWidth / ((float)columns - 1);
            vS = vSpacing * pWidth / ((float)columns - 1);
        }
        else
        {
            hS = hSpacing;
            vS = vSpacing;
        }

        float cellWidth = 0;
        float cellHeight = 0;
        float offsetCenter = 0;
        if (dependHeight)
        {
            cellHeight = (rectTransform.rect.height - padding.vertical - vS * (rows - 1)) / (float)rows;
            cellWidth = cellHeight / cellRatio;
            if (autoCenter)
                offsetCenter = (pWidth - (cellWidth + hS) * columns + hS) / 2;
        }
        else
        {
            cellWidth = (pWidth - hS * (columns - 1)) / (float)columns;
            cellHeight = cellWidth * cellRatio;
        }
        float childPosX = 0, childPosY = 0;
        int colIndex, rowIndex;


        for (int i = 0; i < rectChildren.Count; i++)
        {
            colIndex = i % columns;
            rowIndex = i / columns;

            var childRect = rectChildren[i];

            childPosX = (cellWidth + hS) * colIndex + offsetCenter + padding.left;
            childPosY = (cellHeight + vS) * rowIndex + padding.top;

            SetChildAlongAxis(childRect, 0, childPosX, cellWidth);
            SetChildAlongAxis(childRect, 1, childPosY, cellHeight);
        }
        if (autoFitHeight)
        {
            if (!dependHeight)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, childPosY + cellHeight + padding.bottom);
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
            }
            {
                // auto fit width
                float width = (cellWidth + hS) * columns - hS;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + padding.horizontal);
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
            }
        }
        else
        {
            m_Tracker.Clear();
        }
    }

    public override void SetLayoutHorizontal()
    { }

    public override void SetLayoutVertical()
    { }

    // Use this for initialization
}

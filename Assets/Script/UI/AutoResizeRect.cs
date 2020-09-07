using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoResizeRect : MonoBehaviour
{
    public bool vertical = false;
    public bool horizon = false;

    public float gap = 0;
    public Transform anotherGridRoot = null;



    // Start is called before the first frame update
    void Start()
    {

        GridLayoutGroup grid = this.gameObject.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = GetWhichHasMoreChildGrid(anotherGridRoot);
            if (grid == null)
            {
                return;
            }
        }
        else
        {
        }

        float child = grid.transform.childCount / grid.constraintCount;
        child += gap;
        Vector2 spacing = grid.spacing;
        Vector2 cellsize = grid.cellSize;
        if (vertical == true)
            this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (cellsize.y + spacing.y) * child);
        if (horizon == true)
            this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (cellsize.x + spacing.x) * child);
    }

    public void RefreshReSize(bool isVertical, int count)
    {
        GridLayoutGroup grid = this.gameObject.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = GetWhichHasMoreChildGrid(anotherGridRoot);
            if (grid == null) return;
        }

        float child = count / grid.constraintCount;
        child += gap;
        Vector2 spacing = grid.spacing;
        Vector2 cellsize = grid.cellSize;
        if (isVertical == true)
            this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (cellsize.y + spacing.y) * (child + 1));
        if (isVertical == true)
            this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (cellsize.x + spacing.x) * child);

    }

    private GridLayoutGroup GetWhichHasMoreChildGrid(Transform _transform)
    {
        List<GridLayoutGroup> temp;
        temp = new List<GridLayoutGroup>(_transform.GetComponentsInChildren<GridLayoutGroup>());

        GridLayoutGroup result = null;

        for (int i = 1; i < temp.Count; i++)
        {
            if (temp[i-1].transform.childCount <= temp[i].transform.childCount)
            {
                result = temp[i];
            }
        }

        if (temp.Count == 0)
        {
            return null;
        }
        else
        {
            return result;
        }
    }
}

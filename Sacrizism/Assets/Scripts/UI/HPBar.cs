using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public RectTransform image;

    private float initialWidth;

    private void Awake()
    {
        initialWidth = image.sizeDelta.x;
    }

    public void SetWidthPercentage(float percentage)
    {
        image.sizeDelta = new Vector2(initialWidth * percentage, image.sizeDelta.y);
    }
}

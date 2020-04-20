using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour
{
    public RectTransform Bar;
    private float initialWidth;
    private void Start()
    {
        initialWidth = Bar.sizeDelta.x;
    }

    public void SetHealthbar(float current, float max )
    {
        Bar.sizeDelta = new Vector2( (current / max) * initialWidth, Bar.sizeDelta.y);
    }
}

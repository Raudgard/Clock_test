using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class DigitalClock : Clock
{
    [SerializeField] private TextMeshProUGUI timeLabel;
    [SerializeField] private Color originalColor;
    
    public override void ShowTime(DateTime time)
    {
        timeLabel.text = time.ToLongTimeString();
    }

    public override void SetValuesForColorsAndLocalScale(float value)
    {
        timeLabel.color = Color.Lerp(new Color(originalColor.r, originalColor.g, originalColor.b, 0), originalColor, value);
        timeLabel.transform.localScale = Vector3.Lerp(endLocalScale, Vector3.one, value);
    }

}

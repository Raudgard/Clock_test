using UnityEngine;
using UnityEngine.UI;
using System;

public class RoundClock : Clock
{
    [SerializeField] private Image secondsHand;
    [SerializeField] private Image minutesHand;
    [SerializeField] private Image clockWise;
    [SerializeField] private Image dial;
    [SerializeField] private Image centerLabel;

    private Image[] images = new Image[5];
    private Color[] originalColors = new Color[5];

    private void Awake()
    {
        images[0] = secondsHand;
        images[1] = minutesHand;
        images[2] = clockWise;
        images[3] = dial;
        images[4] = centerLabel;
        for (int i = 0; i < originalColors.Length; i++)
        {
            originalColors[i] = images[i].color;
        }
    }
    

    public override void ShowTime(DateTime time)
    {
        secondsHand.transform.localEulerAngles = new Vector3(0, 0, GetSecondHandRotation(time));
        minutesHand.transform.localEulerAngles = new Vector3(0, 0, GetMinutesHandRotation(time));
        clockWise.transform.localEulerAngles = new Vector3(0, 0, GetClockwiseRotation(time));
    }


    private float GetSecondHandRotation(DateTime time)
    {
        return -6 * time.Second;
    }

    private float GetMinutesHandRotation(DateTime time)
    {
        return -((time.Minute * 60 + time.Second) * 0.1f);
    }

    private float GetClockwiseRotation(DateTime time)
    {
        return -((time.Hour % 12 * 3600 + time.Minute * 60 + time.Second) * 360f / 43200f);
    }

    public override void SetValuesForColorsAndLocalScale(float value)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.Lerp(new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0), originalColors[i], value);
            images[i].transform.localScale = Vector3.Lerp(endLocalScale, Vector3.one, value);
        }
    }





    
}

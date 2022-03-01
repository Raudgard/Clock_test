using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlarmClockRound : Clock
{
    [SerializeField] private Image secondsHand;
    [SerializeField] private Image minutesHand;
    [SerializeField] private Image clockWise;
    [SerializeField] private Image dial;
    [SerializeField] private Image centerLabel;
    [SerializeField] private TextMeshProUGUI handOfClockValue_TMProMesh;
    [SerializeField] private Button AM_PM_Button;
    [SerializeField] private TextMeshProUGUI AM_PM_Button_textMesh;

    private Image[] images = new Image[5];
    private Color[] originalColors = new Color[5];

    private int hourAlarmTime, minuteAlarmTime, secondAlarmTime;
    private bool isPMtime = false;

    private int clockHandValue;
    [SerializeField] float timeToAppearingClockHandValue;
    [SerializeField] float timeToDisappearingClockHandValue;
    [SerializeField] float timeForShowClockHandValue;

    private Coroutine handOfClockValueAppearingCoroutine = null;


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

    private void OnEnable()
    {
        handOfClockValue_TMProMesh.text = string.Empty;
    }


    public override void SetValuesForColorsAndLocalScale(float value)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.Lerp(new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0), originalColors[i], value);
            images[i].transform.localScale = Vector3.Lerp(endLocalScale, Vector3.one, value);
        }
    }

    public override void ShowTime(DateTime time) { }


    public void SetAlarmTime(HandOfClockDragController.HandType handType)
    {
        switch(handType)
        {
            case HandOfClockDragController.HandType.Hour: hourAlarmTime = clockHandValue; break;
            case HandOfClockDragController.HandType.Minute: minuteAlarmTime = clockHandValue; break;
            case HandOfClockDragController.HandType.Second: secondAlarmTime = clockHandValue; break;
        }
        TimeController.Instance.SetAlarmTime(hourAlarmTime, minuteAlarmTime, secondAlarmTime);
    }

    public void SetHandOfClockValue(int value, HandOfClockDragController.HandType handType)
    {
        if (value != clockHandValue)
        {
            if (handType == HandOfClockDragController.HandType.Hour && isPMtime) value += 12;
            if (handOfClockValueAppearingCoroutine != null) StopCoroutine(handOfClockValueAppearingCoroutine);
            handOfClockValueAppearingCoroutine = StartCoroutine(AppearingHandOfClockValue(value));
        }
        clockHandValue = value;

    }

    private IEnumerator AppearingHandOfClockValue(int value)
    {
        handOfClockValue_TMProMesh.text = value.ToString();
        handOfClockValue_TMProMesh.CrossFadeAlpha(1, timeToAppearingClockHandValue, false);
        yield return new WaitForSeconds(timeForShowClockHandValue);
        handOfClockValue_TMProMesh.CrossFadeAlpha(0, timeToDisappearingClockHandValue, false);
        handOfClockValueAppearingCoroutine = null;

    }


    public void AM_PM_Click()
    {
        if(isPMtime)
        {
            isPMtime = false;
            AM_PM_Button_textMesh.text = "AM";
        }
        else
        {
            isPMtime = true;
            AM_PM_Button_textMesh.text = "PM";
        }
    }

}

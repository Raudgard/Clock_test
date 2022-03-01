using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;
    public DateTime CURRENT_TIME;
    private NTP_Client m_NTP_Client;
    [SerializeField]
    private string[] servers;
    private Coroutine increaseForOneSecond_Coroutine;
    public static WaitForSecondsRealtime oneSecondDelay = new WaitForSecondsRealtime(1);

    [SerializeField]
    private int minuteWhenUpdateTime;
    [SerializeField]
    private int secondWhenUpdateTime;
    [SerializeField]
    private GameObject alarmLabel;
    [SerializeField]
    private TextMeshProUGUI alarmTimeTextMesh;

    private DateTime alarmTime;
    private bool isAlarmSet;



    private void Start()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        m_NTP_Client = new NTP_Client(servers);
        UpdateTime();
        ShowTime();
    }

    /// <summary>
    /// Set time from Internet server
    /// </summary>
    public void UpdateTime()
    {
        var networkTime = m_NTP_Client.GetNetworkTime();
        CURRENT_TIME = networkTime;
        if(m_NTP_Client.State == GettingNetworkTimeState.Fail)
        {
            MessageController.Instance.ShowError("Can't connect to server. Error: " + m_NTP_Client.Error);
        }
        else
        {
            MessageController.Instance.ShowMessage("Time updated successfully.");
        }

        if (increaseForOneSecond_Coroutine != null) StopCoroutine(increaseForOneSecond_Coroutine);
        increaseForOneSecond_Coroutine = StartCoroutine(IncreaseCurrentTimeForOneSecond());
    }

    private IEnumerator IncreaseCurrentTimeForOneSecond()
    {
        float delay = 1 - (float)CURRENT_TIME.Millisecond / 1000;

        yield return new WaitForSeconds(delay);
        CURRENT_TIME = CURRENT_TIME.AddMilliseconds(delay);
        ShowTime();

        while (true)
        {
            yield return oneSecondDelay;
            if(CURRENT_TIME.Minute == minuteWhenUpdateTime && CURRENT_TIME.Second == secondWhenUpdateTime)
            {
                UpdateTime();
            }

            CURRENT_TIME = CURRENT_TIME.AddSeconds(1);
            ShowTime();
            if (isAlarmSet && IsAlarmTime()) Alarm();
        }
    }

    public void ShowTime()
    {
        SwapController.instance.ActiveClock.ShowTime(CURRENT_TIME);
    }

    
    public void SetAlarmTime(int hour, int minute, int second)
    {
        alarmTime = new DateTime(2000, 1, 1, hour, minute, second);

    }

    public void SetAlarmOn()
    {
        isAlarmSet = true;
        alarmLabel.SetActive(true);
        string GetTwoCharsString(int value) => value < 10 ? ("0" + value.ToString()) : value.ToString();
        alarmTimeTextMesh.text = GetTwoCharsString(alarmTime.Hour) + ":" + GetTwoCharsString(alarmTime.Minute) + ":" + GetTwoCharsString(alarmTime.Second);
    }

    public void CancelAlarm()
    {
        isAlarmSet = false;
        alarmLabel.SetActive(false);
    }

    private bool IsAlarmTime() => CURRENT_TIME.Second == alarmTime.Second && CURRENT_TIME.Minute == alarmTime.Minute && CURRENT_TIME.Second == alarmTime.Second;

    private void Alarm()
    {
        print("ALARM!!!!!");
        MessageController.Instance.Alarm();
    }
}

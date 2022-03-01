using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HandOfClockDragController : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    public enum HandType
    {
        Hour,
        Minute,
        Second
    }

    [SerializeField] private RectTransform handOfClock;
    [SerializeField] private AlarmClockRound alarmClockRound;
    [SerializeField] private HandType handType;
    private int angle;
    private int m_valueOfClockHand;

    private void Awake()
    {
        angle = handType == HandType.Hour ? 30 : 6;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 sub = eventData.position - (Vector2)handOfClock.position;
        float rotationZ = -Vector2.SignedAngle(sub, Vector2.up);
        int roundedRotation = Rounding(rotationZ, out m_valueOfClockHand);
        handOfClock.eulerAngles = new Vector3(0, 0, roundedRotation);

        m_valueOfClockHand = m_valueOfClockHand <= 0 ? -m_valueOfClockHand : (handType == HandType.Hour ? Mathf.Abs(m_valueOfClockHand - 12) : Mathf.Abs(m_valueOfClockHand - 60));
        if (m_valueOfClockHand == 60) m_valueOfClockHand = 0;
        alarmClockRound.SetHandOfClockValue(m_valueOfClockHand, handType);

    }

    private int Rounding(float value, out int valueOfClockHand)
    {
        int min = (int)(value / angle);
        int max = min < 0 ? min - 1 : min + 1;
        int minAngle = min * angle;
        int maxAngle = max * angle;
        int res;
        if(Mathf.Abs(value - minAngle) < Mathf.Abs(value - maxAngle))
        {
            res = minAngle;
            valueOfClockHand = min;
        }
        else
        {
            res = maxAngle;
            valueOfClockHand = max;
        }
        return res;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        alarmClockRound.SetAlarmTime(handType);
    }
}

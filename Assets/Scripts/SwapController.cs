using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SwapController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private DigitalClock digitalClock;
    [SerializeField]
    private RoundClock roundClock;
    [SerializeField]
    private AlarmClockDigital alarmClockDigital;
    [SerializeField]
    private AlarmClockRound alarmClockRound;

    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private TimeController timeController;
    [SerializeField]
    [Range(0,1)]
    private float swapMinValueForFullDisappear;
    [SerializeField]
    [Range(0,1)]
    private float swapTreshold;
    [SerializeField]
    [Range(0.9f, 1)]
    private float returningTreshold;

    [SerializeField]
    private Button setAlarmButton;

    private bool isReturning;
    public static SwapController instance;

    private Clock activeClock;
    public Clock ActiveClock => activeClock;

    private void Awake()
    {
        instance = this;
        GetActiveClock();
        activeClock.isActive = true;
    }

    private void SetTransparencyForClock(float value)
    {
        ActiveClock.Shifting(value);
    }


    public void OnDrag(PointerEventData eventData)
    {
        SetTransparencyForClock(Mathf.InverseLerp(swapMinValueForFullDisappear, 1, scrollbar.size));
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isReturning = false;
        ActiveClock.isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ActiveClock.isDragging = false;

        if (Mathf.Abs(scrollbar.size) < swapTreshold)
        {
            SwapActiveClock(false);
        }
        else
        {
            isReturning = true;
            StartCoroutine(Returning());
        }

    }


    private IEnumerator Returning()
    {
        while (isReturning && scrollbar.size < returningTreshold)
        {
            OnDrag(new PointerEventData(EventSystem.current));
            yield return null;
        }

    }


    private void GetActiveClock()
    {
        if (digitalClock.gameObject.activeSelf) activeClock = digitalClock;
        else activeClock = roundClock;
    }

    private void SwapActiveClock(bool toAlarmClock)
    {
        Clock appearingClock;
        if (toAlarmClock) appearingClock = GetPairedAlarmClock();
        else appearingClock = GetPairedInactiveClock();

        activeClock.isActive = false;
        activeClock.DisappearToEnd(Mathf.InverseLerp(swapMinValueForFullDisappear, 1, scrollbar.size));
        activeClock = appearingClock;
        activeClock.isActive = true;
        activeClock.Appear();
    }

    private Clock GetPairedInactiveClock()
    {
        if (digitalClock.isActive) return roundClock;
        else if (roundClock.isActive) return digitalClock;
        else if (alarmClockDigital.isActive) return alarmClockRound;
        else return alarmClockDigital;
    }

    private Clock GetPairedAlarmClock()
    {
        if (digitalClock.isActive) return alarmClockDigital;
        else if (roundClock.isActive) return alarmClockRound;
        else if (alarmClockDigital.isActive) return digitalClock;
        else return roundClock;
    }

   

    public void SwapToAlarmClock()
    {
        SwapActiveClock(true);
    }

}

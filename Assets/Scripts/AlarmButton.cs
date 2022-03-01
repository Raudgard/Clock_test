using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlarmButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Vector3 sizeOnPressed;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color alarmColor;
    [HideInInspector] private bool isAlarmMode = false;


    private void Awake()
    {
        button.onClick.AddListener(Click);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = sizeOnPressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }


    private void Click()
    {
        if(isAlarmMode)
        {
            isAlarmMode = false;
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = colorBlock.selectedColor = normalColor;
            colorBlock.pressedColor = alarmColor;
            button.colors = colorBlock;

            TimeController.Instance.SetAlarmOn();
        }
        else
        {
            isAlarmMode = true;
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = colorBlock.selectedColor = alarmColor;
            colorBlock.pressedColor = normalColor;
            button.colors = colorBlock;
        }

        SwapController.instance.SwapToAlarmClock();
    }




}

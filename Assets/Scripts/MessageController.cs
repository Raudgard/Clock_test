using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public static MessageController Instance;
    [SerializeField] private TextMeshProUGUI messageMesh;
    [SerializeField] private Color errorMessageColor;
    [SerializeField] private Color normalMessageColor;
    [SerializeField] private float messageTime;
    [SerializeField] private Button cancelAlarmButton;
    [SerializeField] private TextMeshProUGUI alarmMessage;


    [SerializeField] private float alarmMessageTimeAppearance;


    private Coroutine alarmCoroutine = null;
    private bool isAlarmON = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowError(string text)
    {
        StartCoroutine(ShowMessage(text, errorMessageColor));
    }

    public void ShowMessage(string text)
    {
        StartCoroutine(ShowMessage(text, normalMessageColor));
    }

    private IEnumerator ShowMessage(string text, Color messageColor)
    {
        messageMesh.gameObject.SetActive(true);
        messageMesh.SetText(text);
        messageMesh.color = messageColor;
        yield return new WaitForSeconds(messageTime);
        messageMesh.gameObject.SetActive(false);
    }

    public void Alarm()
    {
        if (alarmCoroutine == null) StartCoroutine(Alarming());
    }

    private IEnumerator Alarming()
    {
        isAlarmON = true;
        alarmMessage.gameObject.SetActive(true);
        cancelAlarmButton.gameObject.SetActive(true);

        while (isAlarmON)
        {
            alarmMessage.CrossFadeAlpha(1, alarmMessageTimeAppearance, false);
            Handheld.Vibrate();
            yield return new WaitForSeconds(alarmMessageTimeAppearance);
            alarmMessage.CrossFadeAlpha(0, alarmMessageTimeAppearance, false);
            yield return new WaitForSeconds(alarmMessageTimeAppearance);
        }

        alarmMessage.gameObject.SetActive(false);
        alarmCoroutine = null;
    }

    public void CancelAlarmButton_Click()
    {
        isAlarmON = false;
        cancelAlarmButton.gameObject.SetActive(false);
    }
}

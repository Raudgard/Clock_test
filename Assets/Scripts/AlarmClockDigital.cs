using System;
using UnityEngine;
using TMPro;

public class AlarmClockDigital : Clock
{
    [SerializeField] private TMP_InputField clockWise_InputField;
    [SerializeField] private TMP_InputField minutesHand_InputField;
    [SerializeField] private TMP_InputField secondHand_InputField;

    [SerializeField] private TextMeshProUGUI clockWise_text;
    [SerializeField] private TextMeshProUGUI minutesHand_text;
    [SerializeField] private TextMeshProUGUI secondHand_text;
    [SerializeField] private TextMeshProUGUI twoPoints_text;
    [SerializeField] private TextMeshProUGUI twoPoints2_text;


    [SerializeField] private Color originalColor;

    private void Awake()
    {
        clockWise_InputField.onValidateInput += ValidateInput;
        minutesHand_InputField.onValidateInput += ValidateInput;
        secondHand_InputField.onValidateInput += ValidateInput;

        clockWise_InputField.onEndEdit.AddListener((string value) => { EndEdit(value, clockWise_InputField, 23); });
        minutesHand_InputField.onEndEdit.AddListener((string value) => { EndEdit(value, minutesHand_InputField, 59); });
        secondHand_InputField.onEndEdit.AddListener((string value) => { EndEdit(value, secondHand_InputField, 59); });
    }

    public override void SetValuesForColorsAndLocalScale(float value)
    {
        clockWise_text.color = minutesHand_text.color = secondHand_text.color = twoPoints_text.color = twoPoints2_text.color =
            Color.Lerp(new Color(originalColor.r, originalColor.g, originalColor.b, 0), originalColor, value);
    }

    public override void ShowTime(DateTime time) { }

    private char ValidateInput(string input, int charIndex, char addedChar)
    {
        if (int.TryParse(addedChar.ToString(), out int res))
        {
            return res.ToString()[0];
        }
        else return '\0';
    }

    private void EndEdit(string value, TMP_InputField tMP_InputField, int maxValue)
    {
        if (string.IsNullOrEmpty(value)) tMP_InputField.text = "00";
        int val = int.Parse(tMP_InputField.text);
        if (val > maxValue) { val = maxValue; tMP_InputField.text = val.ToString(); }
        if (tMP_InputField.text.Length == 1) tMP_InputField.text = "0" + tMP_InputField.text;
        TimeController.Instance.SetAlarmTime(GetIntFromText(clockWise_InputField.text), GetIntFromText(minutesHand_InputField.text), GetIntFromText(secondHand_InputField.text));
    }


    private int GetIntFromText(string text)
    {
        if (int.TryParse(text, out int res)) return res;
        else return 0;
    }


}

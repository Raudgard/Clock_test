using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class Clock : MonoBehaviour
{
    [SerializeField] protected Vector3 endLocalScale;
    [SerializeField] protected float maxRotateAngle;
    [SerializeField] protected float appearingSpeed;
    [SerializeField] protected float disappearingSpeed;

    [SerializeField] protected float maxRotateAngleWhenAppearing;

    protected float timeToDisappearing;
    protected float deacreasingCoeff;
    [HideInInspector] public bool isDragging;
    [HideInInspector] public bool isActive = false;

    public abstract void ShowTime(DateTime time);

    public abstract void SetValuesForColorsAndLocalScale(float value);


    public void Shifting(float value)
    {
        SetValuesForColorsAndLocalScale(value);
        gameObject.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(maxRotateAngle, 0, value), 0);
    }

    public void DisappearToEnd(float startValue)
    {
        StartCoroutine(DisappearingToZero(startValue));
    }

    private IEnumerator DisappearingToZero(float startValue)
    {
        while (startValue > 0 && !isActive)
        {
            SetValuesForColorsAndLocalScale(startValue);
            gameObject.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(maxRotateAngle, 0, startValue), 0);

            yield return null;
            startValue -= Time.deltaTime * disappearingSpeed;
        }
        if (isActive) yield break;

        gameObject.transform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);
        
    }

    
    public void Appear()
    {
        gameObject.SetActive(true);
        ShowTime(TimeController.Instance.CURRENT_TIME);
        StartCoroutine(Appearing());
    }

    private IEnumerator Appearing()
    {
        float t = endLocalScale.x;

        while(!isDragging && t <= 1)
        {
            SetValuesForColorsAndLocalScale(t);
            gameObject.transform.localScale = Vector3.Lerp(endLocalScale, Vector3.one, t);
            gameObject.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(maxRotateAngleWhenAppearing, 0, t), 0);

            yield return null;
            t += Time.deltaTime * appearingSpeed;
        }
        gameObject.transform.localEulerAngles = Vector3.zero;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelledSlider : MonoBehaviour
{

    public Slider slider;
    public Text nameLabel;
    public Text max;
    public Text min;
    public Text currentValue;


    private void Awake()
    {
        SetRange(slider.minValue, slider.maxValue);
    }

    public void Init(float min, float max, float current)
    {
        SetRange(min,max);
        slider.value = current;
    }
    
    public void OnValueChanged(float value)
    {
        currentValue.text = "" + RoundOff(value);
    }
    public void SetRange(float minimum, float maximum)
    {
        slider.maxValue = maximum;
        slider.minValue = minimum;

        max.text = "" + RoundOff(maximum);
        min.text = "" + RoundOff(minimum);
        OnValueChanged(slider.value);
    }
    private float RoundOff(float num)
    {
        return ((int)(num*10))/10f;
    }
}

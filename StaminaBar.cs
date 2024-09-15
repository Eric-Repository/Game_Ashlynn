using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    //store slider value
    public Slider slider;
    //create a gradient
    public Gradient gradient;
    //use a image fill to indicate the hp bar color
    public Image fill;

    public void SetMaxStamina(int stamina)
    {
        //set the max HP value
        slider.maxValue = stamina;
        slider.value = stamina;

        //set it to green
        fill.color = gradient.Evaluate(1f);
    }

    public void SetStamina(int stamina)
    {
        //set the slider value to health
        slider.value = stamina;

        //change the slider value to 0.0 - 1.0 and set it to fill
        fill.color = gradient.Evaluate(slider.normalizedValue);

    }



}

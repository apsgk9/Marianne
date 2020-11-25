using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterProperties;
using UnityEngine.UI;
using System;

//[RequireComponent(typeof(Image))]
public class UIStaminaBar : MonoBehaviour
{
    private ICharacterStamina Stamina;
    public GameObject StaminaObject;
    public Image StaminaImage;

    private bool ChangeInStamina;
    public float FadeOutWaitTiming=0.5f;
    public float FadeOutTime=0.5f;

    private Timer FadeOutWaitTimer;
    private Timer FadeOutTimer;
    private void Start()
    {
        Stamina = StaminaObject.GetComponent<ICharacterStamina>();
        Stamina.OnStaminaChanged += HandleStamina;
        FadeOutWaitTimer = new Timer(FadeOutTime);
        FadeOutTimer = new Timer(FadeOutWaitTiming);
        ChangeImageAlphaValue(0f);
        
        FadeOutWaitTimer.timer=FadeOutWaitTiming+1f;
        FadeOutTimer.timer=FadeOutTime+1f;
    }

    private void HandleStamina(float CurrentStamina,float minStamina,float maxStamina)
    {
        float percentAlpha = 1f;
        ChangeImageAlphaValue(percentAlpha);

        StaminaImage.fillAmount = Math.Abs(CurrentStamina - minStamina) / (maxStamina - minStamina);
        FadeOutWaitTimer.ResetTimer();
        FadeOutTimer.ResetTimer();
    }

    

    void Update()
    {
        if(!FadeOutWaitTimer.Activated)
        {
            FadeOutWaitTimer.Tick();
        }
        else if(!FadeOutTimer.Activated)
        {
            FadeOutTimer.Tick();
            float percentAlpha =(1 - (FadeOutTimer.timer / FadeOutTime));
            ChangeImageAlphaValue(percentAlpha);
        }
        else
        {

        }
    }

    private void ChangeImageAlphaValue(float percentAlpha)
    {
        Color StaminaColor = StaminaImage.color;
        StaminaColor.a = percentAlpha;
        StaminaImage.color = StaminaColor;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterProperties;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
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

    public Color NormalColor=Color.yellow;

    public Color DangerColor=Color.red;
    public float ColorStartChangeThreshold=0.4f;
    public float ColorEndChangeThreshold=0.2f;
    private void Start()
    {
        Stamina = StaminaObject.GetComponent<ICharacterStamina>();
        Stamina.OnStaminaChanged += HandleStamina;
        FadeOutWaitTimer = new Timer(FadeOutTime);
        FadeOutTimer = new Timer(FadeOutWaitTiming);
        ChangeImageAlphaValue(0f);
        
        FadeOutWaitTimer.FinishTimer();
        FadeOutTimer.FinishTimer();
    }

    private void HandleStamina(float CurrentStamina,float minStamina,float maxStamina)
    {
        float percentAlpha = NormalColor.a;
        ChangeImageAlphaValue(percentAlpha);
        HandleStaminaImageColor(CurrentStamina/maxStamina);

        StaminaImage.fillAmount = Math.Abs(CurrentStamina - minStamina) / (maxStamina - minStamina);
        FadeOutWaitTimer.ResetTimer();
        FadeOutTimer.ResetTimer();
    }

    

    void Update()
    {
        FadeOutSequence();
    }

    private void OnValidate()
    {
        StaminaImage = GetComponent<Image>();
        StaminaImage.color = NormalColor;
    }

    private void FadeOutSequence()
    {
        
        if (!FadeOutWaitTimer.Activated)
        {
            FadeOutWaitTimer.Tick();
        }
        else if (!FadeOutTimer.Activated)
        {
            FadeOutTimer.Tick();

            float percentAlpha = (StaminaImage.color.a - (FadeOutTimer.timer / FadeOutTime));
            ChangeImageAlphaValue(percentAlpha);
        }
    }

    private void HandleStaminaImageColor(float percent)
    {
        if(percent<=ColorStartChangeThreshold && percent>ColorEndChangeThreshold)
        {
            Debug.Log(percent);
            float lerped=1-((percent-ColorEndChangeThreshold)/(ColorStartChangeThreshold-ColorEndChangeThreshold));
            Debug.Log(lerped);
            StaminaImage.color = Color.Lerp(NormalColor, DangerColor,lerped);
        }
        else if (percent<=ColorEndChangeThreshold)
        {
            StaminaImage.color = DangerColor;  
        }
        else
        {
            StaminaImage.color = NormalColor;            
        }
    }

    private void ChangeImageAlphaValue(float percentAlpha)
    {
        if(percentAlpha<0)
        {
            percentAlpha=0;
        }
        Color StaminaColor = StaminaImage.color;
        StaminaColor.a = percentAlpha;
        StaminaImage.color = StaminaColor;
    }

}

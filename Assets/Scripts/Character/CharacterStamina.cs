﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterProperties;
using System;

public class CharacterStamina : MonoBehaviour,ICharacterStamina
{
    public float CurrentStamina=100;
    public int MaxStamina=100;
    public int MinStamina=0;
    public bool CapMaxStamina=true;
    public bool CapMinStamina=true;
    public uint RegenRate=1;
    private bool _isStaminaBeingUsed=false;

    public bool IsStaminaBeingUsed { get =>_isStaminaBeingUsed; set => _isStaminaBeingUsed=value ; }

    public event Action<float> OnStaminaChanged;
    public float AddStamina(float changeToAdd)
    {
        CurrentStamina += changeToAdd;
        CapStamina();
        OnStaminaChanged?.Invoke(CurrentStamina);
        return CurrentStamina;
    }

    private void CapStamina()
    {
        if (CurrentStamina > MaxStamina && CapMaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
        else if (CurrentStamina < MinStamina && CapMinStamina)
        {
            CurrentStamina = MinStamina;
        }
    }

    public float ChangeStamina(float newStamina)
    {
        
        CurrentStamina=newStamina;
        CapStamina();
        OnStaminaChanged?.Invoke(CurrentStamina);
        return CurrentStamina;
    }

    public float GetCurrentStamina()
    {
        return CurrentStamina;
    }

    public bool HasStamina()
    {
        if(CurrentStamina>0)
        {
            return true;
        }
        return false;
    }
    private void Update()
    {
        RegenStamina();
    }

    private void RegenStamina()
    {
        if (!IsStaminaBeingUsed)
        {
            if(CapMaxStamina && CurrentStamina<MaxStamina)
            {
                AddStamina(RegenRate);
            }
        }
    }
}

using UnityEngine;
using CharacterProperties;
using System;

public class CharacterStamina : MonoBehaviour,ICharacterStamina
{
    public float CurrentStamina=100;
    public int MaxStamina=100;
    public int MinStaminaUsage=20;
    public int MinStamina=0;
    public bool CapMaxStamina=true;
    public bool CapMinStamina=true;
    public uint RegenRate=1;
    private bool _isStaminaBeingUsed=false;
    public bool IsStaminaBeingUsed { get => _isStaminaBeingUsed; set => _isStaminaBeingUsed=value ; }
    public bool HasDrained=false;
    
    [Tooltip("Wait time before stamina regen.")]
    public uint StaminaRechargeTime=2;
    private Timer StaminaRechargeTimer;

    public event Action<float,float,float> OnStaminaChanged;

    private void Start()
    {        
        StaminaRechargeTimer = new Timer(StaminaRechargeTime);
        StaminaRechargeTimer.FinishTimer();
    }

    //Try to call only if there is change.
    public float AddStamina(float changeToAdd,bool shouldWaitForRegen=true)
    {
        CurrentStamina += changeToAdd;
        CheckIfStaminaHasRecovered();
        ProcessStamina();
        OnStaminaChanged?.Invoke(CurrentStamina,MinStamina,MaxStamina);

        if(shouldWaitForRegen==true)
        {
            StaminaRechargeTimer.ResetTimer();            
        }

        return CurrentStamina;
    }

    private void CheckIfStaminaHasRecovered()
    {
        if (HasDrained == true)
        {
            if (CurrentStamina >= MinStaminaUsage)
            {
                HasDrained = false;
            }
        }
    }

    private void ProcessStamina()
    {
        if (CurrentStamina > MaxStamina && CapMaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
        else if (CurrentStamina < MinStamina && CapMinStamina)
        {
            CurrentStamina = MinStamina;
            HasDrained=true;
        }
    }
    
    public float ChangeStamina(float newStamina,bool shouldWaitForRegen=true)
    {        
        CurrentStamina=newStamina;
        ProcessStamina();
        OnStaminaChanged?.Invoke(CurrentStamina,MinStamina,MaxStamina);

        if(shouldWaitForRegen==true)
        {
            StaminaRechargeTimer.ResetTimer();            
        }
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
                
                if (StaminaRechargeTimer.Activated)
                {
                    AddStamina(RegenRate*Time.deltaTime,false);                    
                }
                else
                {
                    StaminaRechargeTimer.Tick();
                    //To prevent timer ui from fading out
                    OnStaminaChanged?.Invoke(CurrentStamina,MinStamina,MaxStamina);
                }
            }
        }
    }
    public bool CanUse()
    {
        return !HasDrained;
    }
}

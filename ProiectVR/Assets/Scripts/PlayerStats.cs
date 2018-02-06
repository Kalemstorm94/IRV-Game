using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public int maxHealth { get; set; }
    public int maxStamina { get; set; }
    public int maxMana { get; set; }

    public float currentHealth { get; set; }
    public int currentStamina { get; set; }
    public int currentMana { get; set; }

    private int hSliderIndex;
    private int sSliderIndex;
    private int mSliderIndex;

    public int maxExp  { get; set;}
    public int currentExp {get; set;}

    private Slider[] sliders;

    void Awake()
    {
        maxHealth = 100;
        maxStamina = 100;
        maxMana = 100;
        maxExp = 100;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;
        currentExp = 0;

        sliders = GetComponentsInChildren<Slider>();
            for (int i = 0; i < sliders.Length; i++)
            {
                if (sliders[i].name == "Life")
                {
                    hSliderIndex = i;
                    sliders[hSliderIndex].value = this.currentHealth;
                }
                else if (sliders[i].name == "Stamina")
                {
                    sSliderIndex = i;
                    sliders[sSliderIndex].value = this.currentStamina;
                }
                else
                {
                    mSliderIndex = i;
                    sliders[mSliderIndex].value = this.currentMana;
                }
            }
        }

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void setNewHealth(float givenHealth)
    {
        currentHealth = givenHealth;
        sliders[hSliderIndex].value = currentHealth;
    }

    public void SetNewStamina(int givenStamina)
    {
        currentStamina = givenStamina;
        sliders[sSliderIndex].value = currentStamina;
    }

    public void SetNewMana(int givenMana)
    {
        currentMana = givenMana;
        sliders[mSliderIndex].value = currentMana;
    }

    public int SetNewExp(int givenExp)
    {
        currentExp = currentExp + givenExp;
        return currentExp;
    }

}

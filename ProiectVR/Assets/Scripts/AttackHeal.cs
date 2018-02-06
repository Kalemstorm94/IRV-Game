using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHeal : PlayerStats {

    public AttackHeal() : base() { }

    private Animator playerAnimator;
    private float paladinHealth;
    private int paladinStamina;
    private int paladinMana;
    public ParticleSystem particles;

    // Use this for initialization
    void Start () {
        playerAnimator = GetComponent<Animator>();
        paladinHealth = this.currentHealth;
        paladinStamina = this.currentStamina;
        paladinMana = this.currentMana;
    }
	
	// Update is called once per frame
	void Update () {
        isAttacking();
        isHealing();
        isBeingAttacked();
        //MusicPlayer();
    }

    private void isAttacking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("isAttacking", true);
            if(paladinStamina >= 5)
            {
                paladinStamina -= 5;
            }
            this.SetNewStamina(paladinStamina);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("isAttacking", false);
        }
    }

    private void isHealing()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //if(paladinHealth != this.maxHealth || paladinStamina != this.maxStamina)
            //{
                if (paladinMana > 10)
                {
                    paladinHealth += 5;
                    paladinStamina += 5;
                    paladinMana -= 10;
                    healingStatsOutOfBound(paladinHealth, paladinStamina, paladinMana);
                    this.setNewHealth(paladinHealth);
                    this.SetNewStamina(paladinStamina);
                    this.SetNewMana(paladinMana);
                    playerAnimator.SetBool("isHealing", true);
                particles.Play();
            }
            //}
        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerAnimator.SetBool("isHealing", false);
        }
    }

    private void healingStatsOutOfBound(float health, int stamina, int mana)
    {
        if(health > this.maxHealth)
        {
            health = this.maxHealth;
        }
        if(stamina > this.maxStamina)
        {
            stamina = this.maxStamina;
        }
        if(mana <= 0)
        {
            mana = 0;
        }
    }

    private void isBeingAttacked()
    {
        GameObject[] attackingEnemies;
        attackingEnemies = GameObject.FindGameObjectsWithTag("EvilAttack");
        this.paladinHealth = this.paladinHealth - 0.04f * attackingEnemies.Length;
        if (this.paladinHealth <= 0)
        {
            this.paladinHealth = 0;
        }
        else
        {
            this.setNewHealth(this.paladinHealth);
        }
    }

}

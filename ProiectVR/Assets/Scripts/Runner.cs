using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runner : MonoBehaviour {
    private float mySpeed;
    public Slider stamina;
    public AudioClip Energy;
    public AudioClip Speed;
    public AudioClip Ambient;
    private AudioSource AudioSource;
    private bool menuToggled = false;
    
    // Use this for initialization
    void Start () {
        AudioSource = gameObject.GetComponent<AudioSource>();
        AudioSource = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>();
        AudioSource.clip = Ambient;
        AudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if(stamina.value > 1)
        {
            if (Input.GetKey(KeyCode.W))
            {
                stamina.value -= 3.3f;
                if (gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_AnimSpeedMultiplier > 1f)
                {
                    gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_AnimSpeedMultiplier -= 0.002f;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu();

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu();
            }
            gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_AnimSpeedMultiplier = 0.4f;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Speed")
        {
            AudioSource.PlayOneShot(Speed);
            if(stamina.value > 1)
            {
                gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_AnimSpeedMultiplier = 2f;
            }
            Destroy(collider);
        }
        else if(collider.tag == "Energy")
        {
            AudioSource.PlayOneShot(Energy);
            if(stamina.value <= 2)
            {
                gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().m_AnimSpeedMultiplier = 1f;
            }
            stamina.value += 500;
            Destroy(collider);
        }
    }

    public void PauseMenu()
    {
        if (menuToggled)
        {
            GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<Canvas>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = true;
            GameObject.FindGameObjectWithTag("Evil").GetComponent<ChaseMonster>().enabled = true;
            GameObject.FindGameObjectWithTag("Evil").GetComponent<Animator>().enabled = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<Canvas>().enabled = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
            GameObject.FindGameObjectWithTag("Evil").GetComponent<ChaseMonster>().enabled = false;
            GameObject.FindGameObjectWithTag("Evil").GetComponent<Animator>().enabled = false;
        }

        menuToggled = !menuToggled;
    }
}

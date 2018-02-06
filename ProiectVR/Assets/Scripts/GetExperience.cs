using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetExperience : PlayerStats {

    private bool show = false;
    public Text targetText;
    public int exp = 0;
    public int newExp;
    private bool menuToggled = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuToggled)
            {
                GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<Canvas>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<AttackHeal>().enabled = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().enabled = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = true;
                foreach (var obj in GameObject.FindGameObjectsWithTag("Evil"))
                {
                    obj.GetComponent<CubeCollision>().enabled = true;
                }
                foreach (var obj in GameObject.FindGameObjectsWithTag("EvilAttack"))
                {
                    obj.GetComponent<CubeCollision>().enabled = true;
                }
                foreach (var obj in GameObject.FindGameObjectsWithTag("Monster"))
                {
                    obj.GetComponent<Animator>().enabled = true;
                }
            }
            else
            {
                GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<Canvas>().enabled = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<AttackHeal>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().enabled = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
                foreach(var obj in GameObject.FindGameObjectsWithTag("Evil"))
                {
                    obj.GetComponent<CubeCollision>().enabled = false;
                }
                foreach (var obj in GameObject.FindGameObjectsWithTag("EvilAttack"))
                {
                    obj.GetComponent<CubeCollision>().enabled = false;
                }
                foreach (var obj in GameObject.FindGameObjectsWithTag("Monster"))
                {
                    obj.GetComponent<Animator>().enabled = false;
                }
            }

            menuToggled = !menuToggled;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Bottle")
        {
            exp = 10;
            newExp = this.SetNewExp(exp);
            targetText.text = "Experience: " + newExp.ToString();
            targetText.enabled = true;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        targetText.enabled = false;
    }

}

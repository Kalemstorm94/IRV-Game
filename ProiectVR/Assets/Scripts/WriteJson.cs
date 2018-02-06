using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class WriteJson : MonoBehaviour {

    public int hit;
    //public Character player = new Character(0, "Austin", 1337, false, new int[] { 7, 4, 8, 9 });
    void Start()
    {
        //GameManager go = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if(this.gameObject.tag == "Portal")
            {
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LoadGame();
                //Debug.Log("coliziune");
                hit += 100;
            }
        }
    }

}

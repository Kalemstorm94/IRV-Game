using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SmallAudio : MonoBehaviour {

    public AudioClip AudioClip;
    private AudioSource AudioSource;
    // Use this for initialization
    void Start () {
        AudioSource = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>();
        AudioSource.clip = AudioClip;
        AudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
    }
}

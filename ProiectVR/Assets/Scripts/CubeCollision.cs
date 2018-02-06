using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeCollision : MonoBehaviour {

    public GameObject monsterObject;
    public GameObject bottle;
    private Animator monsterAnimator;
    public Transform target;
    public int moveSpeed;
    public int rotationSpeed;
    public float maxDistance;
    public int minDistance;
    bool isCreated = false;

    private Transform myTransform;
    public Slider monsterHealth;

    private AudioSource AudioSource;

    public AudioClip AudioClip1;
    public AudioClip AudioClip2;

    private void Awake()
    {
        //monsterHealth.value = 70;
        myTransform = transform;
        minDistance = 10;
        maxDistance = 1;
    }

    // Use this for initialization
    void Start () {
        GameObject go = GameObject.FindGameObjectWithTag("NewCube");
        monsterAnimator = monsterObject.GetComponent<Animator>();
        target = go.transform;
        AudioSource = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>();
        AudioSource.clip = AudioClip1;
        AudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.DrawLine(target.position, myTransform.position, Color.blue);

        if(this.tag == "Evil" || this.tag == "EvilAttack")
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            if (Vector3.Distance(target.position, myTransform.position) < minDistance)
            {
                if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
                {
                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                    monsterAnimator.SetTrigger("AndGo");
                    this.tag = "Evil";
                }
                else
                {
                    monsterAnimator.SetTrigger("AndAttack");
                    if (AudioSource.clip == AudioClip1)
                    {

                        AudioSource.clip = AudioClip2;

                        AudioSource.Play();

                    }
                    this.tag = "EvilAttack";
                    //monsterAnimator.SetTrigger("AndDie");
                }
            }
            else
            {
                GameObject[] otherAttacking = GameObject.FindGameObjectsWithTag("EvilAttack");
                if(otherAttacking.Length == 0)
                {
                    if (AudioSource.clip == AudioClip2)
                    {

                        AudioSource.clip = AudioClip1;

                        AudioSource.Play();

                    }
                }
                monsterAnimator.SetTrigger("AndStop");
                this.tag = "Evil";
            }
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Weapon")
        {
            monsterHealth.value = monsterHealth.value - 10;
            if (monsterHealth.value > 0)
            {
                monsterAnimator.SetTrigger("AndHit");
            }
            else
            {
                monsterAnimator.SetTrigger("AndDie");
                this.tag = "EvilDead";
                if (AudioSource.clip == AudioClip2)
                {

                    AudioSource.clip = AudioClip1;

                    AudioSource.Play();

                }
                var spawnBottle = monsterObject.transform.position;
                spawnBottle.x = spawnBottle.x + 5;
                if (!isCreated)
                {
                    Instantiate(bottle, spawnBottle, Quaternion.identity);
                    isCreated = true;
                }
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4);
        Destroy(GameObject.FindWithTag("EvilDead"));
    }
}

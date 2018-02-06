using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChaseMonster : MonoBehaviour
{

    public Transform target;
    public int moveSpeed;
    public int rotationSpeed;
    public int maxDistance;
    public int minDistance;

    private Transform myTransform;
    private Animator monsterAnimator;

    //public Slider monsterHealth;


    private void Awake()
    {
        //monsterHealth.value = 100;
        myTransform = transform;
        minDistance = 10000;
        maxDistance = 0;
    }

    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        monsterAnimator = GetComponent<Animator>();
        target = go.transform;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(target.position, myTransform.position, Color.blue);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(target.position, myTransform.position) < minDistance)
        {
            if(Vector3.Distance(target.position, myTransform.position) > maxDistance)
            {
                myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                monsterAnimator.SetTrigger("AndGo");
                Debug.Log("merg");
                if(Vector3.Distance(target.position, myTransform.position) < 2f)
                {
                    Debug.Log("ACUMMM");
                    SceneManager.LoadScene(4);
                }
            }
            else
            {
                SceneManager.LoadScene(4);
            }
        }
        else
        {
            SceneManager.LoadScene(4);
        }
    }
}

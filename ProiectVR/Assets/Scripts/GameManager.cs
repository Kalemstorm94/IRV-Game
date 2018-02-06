using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameManager : PlayerStats
{
    public GameManager() : base() { }
    public static GameManager manager;
    public GameObject monsterPrefab;
    public GameObject paladinPrefab;
    public Slider volumeSlider;
    private GameObject[] monsterObjects;
    private GameObject myPlayer;
    private Slider auxSlider;
    private Vector3 auxPosition;
    private Quaternion auxRotation;
    AudioSource myAudioSource;

    private bool isNewScene = false;
    private bool accesPortal = true;
    private bool finishPortal = true;
    private float newVolValue;
    public bool loadLast = false;

    public AudioSource AudioSource;

    private float paladinHealth;
    private int paladinStamina;
    private int paladinMana;

    void Awake()
    {
        //if(this.tag == "GameManager")
        //{
        //    Destroy(GameObject.FindGameObjectWithTag("GameManagerMenu"));
        //}
        if(manager == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else if(manager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //myAudioSource = GetComponent<AudioSource>();
        //AudioSource.clip = AudioClip1;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4);
        Debug.Log(GameObject.FindGameObjectWithTag("GameManagerMenu"));

    }

    void Update()
    {
        if (isNewScene)
        {
            volumeSlider = GameObject.FindGameObjectWithTag("InGameMenu").GetComponentInChildren<Slider>();
            volumeSlider.value = newVolValue;
            GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<Canvas>().enabled = false;
            isNewScene = false;
        }
        AudioSource.volume = volumeSlider.value;

        if (Input.GetKeyDown(KeyCode.K)){
            volumeSlider.value = volumeSlider.value - 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            volumeSlider.value = volumeSlider.value + 0.2f;
        }
        var portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<WriteJson>();
        if(portal != null)
        {
            if(portal.hit == 100 && accesPortal)
            {
                LoadSecondGame();
            }
            if(portal.hit == 200 && finishPortal){
                FinishGame();
            }
        }

    }

    public void NewGame()
    {
        newVolValue = volumeSlider.value;
        SceneManager.LoadScene(1);
        isNewScene = true;
        //volumeSlider = GameObject.FindGameObjectWithTag("ExperienceCanvas").GetComponent<Slider>();
    }

    public void LoadSecondGame()
    {
        newVolValue = volumeSlider.value;
        accesPortal = false;
        SceneManager.LoadScene(2);
        isNewScene = true;
    }

    public void LoadLastSaved()
    {
        NewGame();
        loadLast = true;
        isNewScene = true;
    }

    public void FinishGame()
    {
        finishPortal = false;
        SceneManager.LoadScene(3);
    }

    public void Save()
    {
        List<MonsterData> monsterList = new List<MonsterData>();
        monsterObjects = GameObject.FindGameObjectsWithTag("Evil");
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = myPlayer.GetComponent<PlayerStats>();

        foreach (GameObject monsterObj in monsterObjects)
        {
            auxPosition = monsterObj.GetComponent<Transform>().position;
            auxRotation = monsterObj.GetComponent<Transform>().rotation;
            auxSlider = monsterObj.GetComponentInChildren<Slider>();
            MonsterData auxMonster = new MonsterData(auxPosition,auxRotation,auxSlider.value);
            monsterList.Add(auxMonster);
            auxMonster = null;
        }
        Transform myPlayerTransform = myPlayer.GetComponent<Transform>();
        WorldData worldData = new WorldData(myPlayerTransform.position, myPlayerTransform.rotation, stats.currentHealth,
            stats.currentStamina, stats.currentMana, stats.currentExp, stats.maxHealth, stats.maxStamina, stats.maxMana,
            stats.maxExp, monsterList);
        string jsonString = JsonUtility.ToJson(worldData);
        File.WriteAllText(Application.dataPath + "/playerInfo.json", jsonString);
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/playerInfo.json"))
        {
            Vector3 newVector3 = new Vector3(0, 0, 0);
            Quaternion newRotation = new Quaternion(0, 0, 0, 0);
            List<MonsterData> newList = new List<MonsterData>();
            WorldData data = new WorldData(newVector3, newRotation, 0, 0, 0, 0, 0, 0, 0, 0, newList);

            string jsonString = File.ReadAllText(Application.dataPath + "/playerInfo.json");
            JsonUtility.FromJsonOverwrite(jsonString, data);

            //ClearBoard();
            GameObject[] idleMonsters = GameObject.FindGameObjectsWithTag("Evil");
            foreach (GameObject monster in idleMonsters)
            {
                int number = 0;
                foreach (MonsterData monst in data.monsters)
                {
                    Debug.Log(data.monsters);
                    Debug.Log(Vector3.Distance(monst.monsterPosition, monster.transform.position));
                    if (Vector3.Distance(monst.monsterPosition, monster.transform.position) == 0)
                    {
                        number += 1;
                    }
                }
                if (number == 0)
                {

                    Destroy(monster);
                }
            }

            //GameObject newEvil = GameObject.FindGameObjectWithTag("NewEvil");
            //foreach (MonsterData monster in data.monsters)
            //{
            //    Transform myTransform = transform;
            //    myTransform.position = monster.monsterPosition;
            //    myTransform.rotation = monster.monsterRotation;
            //    GameObject tmp = Instantiate(newEvil, myTransform);
            //    //GameObject newEvil = GameObject.FindGameObjectWithTag("NewEvil");
            //    tmp.GetComponentInChildren<Slider>().value = monster.monsterHealth;
            //    tmp.tag = "Evil";

            //}
            Transform auxTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            auxTransform.SetPositionAndRotation(data.playerPosition, data.playerRotation);
            PlayerStats stats = myPlayer.GetComponent<PlayerStats>();
            stats.setNewHealth(data.currentHealth);
            stats.SetNewStamina(data.currentStamina);
            stats.SetNewMana(data.currentMana);
        }
    }


    private void DestroyMonsterObjects(GameObject[] monsterArray)
    {
        foreach(GameObject monster in monsterArray)
        {
            Destroy(monster);
        }
    }

    private void ClearBoard()
    {
        GameObject[] idleMonsters = GameObject.FindGameObjectsWithTag("Evil");
        GameObject[] deadMonsters = GameObject.FindGameObjectsWithTag("EvilDead");
        GameObject[] attackingMonsters = GameObject.FindGameObjectsWithTag("EvilAttack");

        DestroyMonsterObjects(idleMonsters);
        DestroyMonsterObjects(deadMonsters);
        DestroyMonsterObjects(attackingMonsters);
    }
}

//[Serializable]
//public class MonsterData
//{
//    public Vector3 monsterPosition;
//    public Quaternion monsterRotation;
//    public float monsterHealth;

//    public MonsterData(Vector3 pos, Quaternion rot,float monsterHealth)
//    {
//        this.monsterPosition = pos;
//        this.monsterRotation = rot;
//        this.monsterHealth = monsterHealth;
//    }
//}

//[Serializable]
//public class WorldData
//{
//    public Vector3 playerPosition;
//    public Quaternion playerRotation;

//    public float currentHealth;
//    public int currentStamina;
//    public int currentMana;
//    public int currentExperience;

//    public int maxHealth;
//    public int maxStamina;
//    public int maxMana;
//    public int maxExperience;

//    public List<MonsterData> monsters;

//    public WorldData(Vector3 pos, Quaternion rot,float currHealth, int currStamina, int currMana, int currExp,
//        int maxHealth, int maxStamina, int maxMana, int maxExp, List<MonsterData> monsterData)
//    {
//        this.playerPosition = pos;
//        this.playerRotation = rot;
//        this.currentHealth = currHealth;
//        this.currentStamina = currStamina;
//        this.currentMana = currMana;
//        this.currentExperience = currExp;
//        this.maxHealth = maxHealth;
//        this.maxStamina = maxStamina;
//        this.maxMana = maxMana;
//        this.maxExperience = maxExp;
//        this.monsters = monsterData;
//    }
//}

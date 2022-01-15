using Completed;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;        //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by any other script.
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public int playerFoodPoints = 100;
    public int playerProtectPoints = 0;
    [HideInInspector] public bool playersTurn = true;

    private BoardManager boardScript;
    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();

        InitGame();
    }

    private void OnLevelWasLoaded (int index)
    {
        level++;
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "You Died on Level " + level;
        levelImage.SetActive(true);
        enabled = false;
    }

    //Update is called every frame.
    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup) return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++) 
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);//??
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}

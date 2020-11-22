using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject [] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text winText;
    public Text gameOverText;


    private bool gameOver;
    private bool restart;
    private int score;

    private PlayerController playerController;

    void Start()
    {
        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }
    

        gameOver = false;
        restart = false;
        restartText.text = "";
        winText.text = "";
        gameOverText.text = "";

        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());

    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SceneManager.LoadScene("Main");
            }
        }
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards [Random.Range (0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'G' for Restart";
                restart = true;
                break;
            }
        }
    }


    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "Points: " + score;
        if (score >= 100)
        {
            winText.text = "You win!" +
                "\n"+
                "Game Created by Jonathan Gonzalez";
            gameOver = true;
            restart = true;

            Destroy(playerController.GetComponent<MeshCollider>());
            
            //Might use this for final project
            //Destroy(playerController);            
                    
        }
    }


   public void GameOver()
    {
        gameOverText.text = "Game Over!" + 
            "\n" +
            "Game Created by Jonathan Gonzalez";
        gameOver = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SnakeHead : MonoBehaviour
{
    public GameObject apple;
    public GameObject endScreen;
    public GameObject appleExplosion;
    int score;
    int hiScore;
    public float startTime;
    public bool started = false;
    public bool stopped = false;
    public Counter gs;

    SnakeMovement snakeRef;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI timeText;


    private void Start()
    {
        snakeRef = GameObject.Find("Snake").GetComponent<SnakeMovement>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        timeText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        hiScore = PlayerPrefs.GetInt("score");
        SaveData.init();
    }

    private void Update()
    {
        if (started)
        {
            float t = Time.time - startTime;
            string min = ((int)t / 60).ToString();
            string sec = (t % 60).ToString("f2");
            timeText.text = min + ":" + sec;
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = "High Score " + hiScore.ToString();
        }
        if (stopped)
        {
            scoreText.text = score.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "tail" || other.gameObject.name.Contains("Wall"))
        {
            Cursor.lockState = CursorLockMode.None;
            stopped = true;
            started = false;
            endScreen.SetActive(true);
            StartCoroutine(snakeRef.Die());
        }
        if(other.gameObject.name == "apple")
        {
            GameObject.Find("Pickup").GetComponent<AudioSource>().Play();
            score++;
            if (score > hiScore)
            {
                hiScore = score;
                PlayerPrefs.SetInt("score", hiScore);
            }
            GameObject expl = Instantiate(appleExplosion, other.transform.position, Quaternion.identity);
            Destroy(expl, 2f);
            Destroy(other.gameObject);
            snakeRef.AddSnakeLength();
            makeApple(1);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void makeApple(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject myapple = Instantiate(apple, getRandomVector(new float[] { 5, 7.5f, 10, 12.5f, 15 }[SaveData.values["gridSize"] - 1] - 1), Quaternion.identity);
            myapple.name = "apple";
            myapple.layer = 2;
        }
    }

    public Vector3 getRandomVector(float range)
    {
        Vector3 pos = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        foreach(BodyPart part in snakeRef.bodyParts)
        {
            if(part.toPosition == pos)
            {
                return getRandomVector(range);
            }
        }
        return pos;
    }
}

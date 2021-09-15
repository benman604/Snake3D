using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 hoveredPosition;
    public float hoveredSize = 100;

    bool hovered = false;
    Vector2 toPosition;
    Vector2 initPosition;
    float toSize = 100;
    TextMeshProUGUI text;

    public GameObject[] onClickOpen;
    public GameObject[] onClickClose;

    AudioSource clickNoise;
    AudioSource hoverNoise;

    void Start()
    {
        toPosition = transform.position;
        initPosition = transform.position;
        text = GetComponent<TextMeshProUGUI>();
        clickNoise = GameObject.Find("Click").GetComponent<AudioSource>();
        hoverNoise = GameObject.Find("Hover").GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, toPosition, 0.5f);
        text.fontSize = Mathf.Lerp(text.fontSize, toSize, 0.5f);

        if(Input.GetMouseButtonDown(0) && hovered)
        {
            clickNoise.Play();
            if(transform.name == "Play Button")
            {
                GameObject.Find("Snake").GetComponent<SnakeMovement>().Begin();
                GameObject.Find("head").GetComponent<SnakeHead>().startTime = Time.time;
                GameObject.Find("head").GetComponent<SnakeHead>().started = true;

                Cursor.lockState = Cursor.lockState = CursorLockMode.Locked;
            }
            if(transform.name == "Play Again Button")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            foreach(GameObject thing in onClickOpen)
            {
                thing.SetActive(true);
            }
            foreach(GameObject thing in onClickClose)
            {
                thing.SetActive(false);
            }

            hovered = false;
            toPosition = initPosition;
            toSize = 100;
            text.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        toPosition = initPosition + hoveredPosition;
        toSize = 100 + hoveredSize;
        text.color = Color.black;
        hoverNoise.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        toPosition = initPosition;
        toSize = 100;
        text.color = Color.white;
    }
}

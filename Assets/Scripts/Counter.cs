using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    public int value = 0;
    public int min = 0;
    public int max = 20;

    AudioSource clickNoise;
    AudioSource hoverNoise;

    private void Start()
    {
        value = SaveData.values[transform.name];
        clickNoise = GameObject.Find("Click").GetComponent<AudioSource>();
        hoverNoise = GameObject.Find("Hover").GetComponent<AudioSource>();
    }
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = value.ToString();
        if(transform.name[0] == 'o')
        {
            GetComponent<TextMeshProUGUI>().text = (value == 1) ? "On" : "Off";
        }
    }

    public void increment()
    {
        if (value < max)
        {
            value++;
            clickNoise.Play();
        }
        else
        {
            hoverNoise.Play();
        }

        if(transform.name == "oMusic")
        {
            GameObject.Find("Music").GetComponent<AudioSource>().volume = 0.14f;
        }

        if (transform.name == "oSfx")
        {
            GameObject.Find("Hover").GetComponent<AudioSource>().volume = 0.62f;
            GameObject.Find("Click").GetComponent<AudioSource>().volume = 1f;
            GameObject.Find("Explode").GetComponent<AudioSource>().volume = 0.33f;
            GameObject.Find("Pickup").GetComponent<AudioSource>().volume = 0.59f;
        }
        //SaveData.initSound();
        SaveData.set(transform.name, value);
    }

    public void decrament()
    {
        if (value > min)
        {
            value--;
            clickNoise.Play();
        }
        else
        {
            hoverNoise.Play();
        }
        if (transform.name == "oMusic")
        {
            GameObject.Find("Music").GetComponent<AudioSource>().volume = 0f;
        }

        if (transform.name == "oSfx")
        {
            GameObject.Find("Hover").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Click").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Explode").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Pickup").GetComponent<AudioSource>().volume = 0f;
        }
        //SaveData.initSound();
        SaveData.set(transform.name, value);
    }
}

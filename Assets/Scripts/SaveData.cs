using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public static Dictionary<string, int> values = new Dictionary<string, int>()
    {
        { "initSpawnSize", 3 },
        { "gridSize", 3 },
        { "speed", 3 },
        { "oMusic", 1 },
        { "oSfx", 1 }
    };

    public static void set(string key, int value)
    {
        values[key] = value;
        PlayerPrefs.SetInt(key, value);
    }

    public static void init()
    {
        Dictionary<string, int> valuesClone = new Dictionary<string, int>(values);
        foreach (string key in valuesClone.Keys)
        {
            if(PlayerPrefs.HasKey(key))
                values[key] = PlayerPrefs.GetInt(key);
        }
        initSound();
    }

    public static void initSound()
    {
        if (values["oMusic"] == 0)
        {
            GameObject.Find("Music").GetComponent<AudioSource>().volume = 0f;
        }
        else
        {
            GameObject.Find("Music").GetComponent<AudioSource>().volume = 0.14f;
        }

        if (values["oSfx"] == 0)
        {
            GameObject.Find("Hover").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Click").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Explode").GetComponent<AudioSource>().volume = 0f;
            GameObject.Find("Pickup").GetComponent<AudioSource>().volume = 0f;
        }
        else
        {
            GameObject.Find("Hover").GetComponent<AudioSource>().volume = 0.62f;
            GameObject.Find("Click").GetComponent<AudioSource>().volume = 1f;
            GameObject.Find("Explode").GetComponent<AudioSource>().volume = 0.33f;
            GameObject.Find("Pickup").GetComponent<AudioSource>().volume = 0.59f;
        }
    }
}

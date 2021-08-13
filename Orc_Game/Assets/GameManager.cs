using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public PlayerHealth player;

    public Vector2 lastCheckpoint;
    // Update is called once per frame
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Platform2d");
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Platform2d");
    }
}

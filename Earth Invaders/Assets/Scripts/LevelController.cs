using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    private GameObject playerObject;
    [SerializeField] private GameObject winLoseText;
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void AddNewPlayer(GameObject newPlayer)
    {
        if (playerObject)
        {
            playerObject.GetComponent<Alien>().RemovePlayer();
        }
        playerObject = newPlayer;
    }

    public void GameLost()
    {
        winLoseText.GetComponent<Text>().text = "YOU LOSE";
        StartCoroutine(ResetGame());
    }

    public void GameWon()
    {
        winLoseText.GetComponent<Text>().text = "YOU WIN";
        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame()
    {
        Time.timeScale = 0.00000001f;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        sceneLoader.LoadStartScene();
    }
}

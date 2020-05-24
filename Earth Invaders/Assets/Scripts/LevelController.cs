using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private GameObject playerObject;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void AddNewPlayer(GameObject newPlayer)
    {
        if (playerObject)
        {
            playerObject.GetComponent<Alien>().RemovePlayer();
        }
        playerObject = newPlayer;
    }
}

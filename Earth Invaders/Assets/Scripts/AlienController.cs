using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{

    [SerializeField] private List<GameObject> alienRows;
    [SerializeField] private int alienColumns = 5;
    [SerializeField] private float horizontalPadding = 0.2f;
    [SerializeField] private float verticalPadding = 0.0f;
    [SerializeField] private float distanceUpFromCenter = 0f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float timeBetweenAliens = 0.2f;
    [SerializeField] private float distanceToWall = 5f;
    //private float newAlienRows;
    //private float newAlienColumns;
    private GameObject[,] aliens;
    private int moveDirection = 1;
    
    void Start()
    {
        //newAlienRows = alienRows.Count * (1f + horizontalPadding);
        //newAlienColumns = alienColumns * (1f + verticalPadding);
        aliens = new GameObject[alienRows.Count, alienColumns];
        SpawnAliens();
        StartCoroutine(MoveAliens());
    }
    private void SpawnAliens()
    {
        float startPositionX;
        float startPositionY;
        if (alienColumns % 2 == 0)
        {
            startPositionX = -((alienColumns - 1 ) / 2f) * (1 + horizontalPadding);
        }
        else
        {
            startPositionX = -((int)Math.Floor(alienColumns / 2f) * (1 + horizontalPadding));
        }

        if (alienRows.Count % 2 == 0)
        {
            startPositionY = -((alienRows.Count - 1) / 2f) * (1 + verticalPadding) - 1 + distanceUpFromCenter;
        }
        else
        {
            startPositionY = -((int) Math.Floor(alienRows.Count / 2f) * (1 + verticalPadding)) - distanceUpFromCenter;
        }
        Debug.Log(startPositionX + " startx value. starty value: " + startPositionY);
        int i = alienRows.Count;
        foreach (var alien in alienRows)
        {
            for (int j = 0; j < alienColumns; j++)
            {
                GameObject currentAlien = Instantiate(alien,gameObject.transform);
                aliens[i - 1, j] = currentAlien;
                float newX = startPositionX + j * (1f + horizontalPadding);
                float newY = startPositionY + i * (1f + verticalPadding);

                currentAlien.transform.position = new Vector2(newX,newY);
            }
            i--;
        }
    }

    private IEnumerator MoveAliens()
    {
        float currentPosition = 0;
        while (true)
        {
            for (int i = alienRows.Count - 1; i >= 0 ; i--)
            {
                for (int j = alienColumns - 1; j >= 0 ; j--)
                {
                    GameObject currentAlien = aliens[i,j];
                    float newX = currentAlien.transform.position.x + (moveSpeed * moveDirection);
                    currentAlien.transform.position = new Vector2(newX,currentAlien.transform.position.y);
                    currentAlien.GetComponent<Alien>().SwitchSprite();
                    yield return new WaitForSeconds(timeBetweenAliens);
                }
            }

            currentPosition += moveSpeed * moveDirection;
            if (moveDirection > 0 && currentPosition > 0 + (alienColumns * (1+ horizontalPadding)) / 2 - distanceToWall)
            {
                moveDirection = moveDirection * -1;
            }
            else if (moveDirection < 0 && currentPosition < 0 - (alienColumns * (1+ horizontalPadding)) / 2 + distanceToWall)
            {
                moveDirection = moveDirection * -1;
            }
        }
    }
    
}

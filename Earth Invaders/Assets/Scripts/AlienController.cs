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
    private float newAlienRows;
    private float newAlienColumns;
    private GameObject[,] aliens;
    void Start()
    {
        newAlienRows = alienRows.Count * (1f + horizontalPadding);
        newAlienColumns = alienColumns * (1f + verticalPadding);
        aliens = new GameObject[alienRows.Count, alienColumns];
        SpawnAliens();
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
}

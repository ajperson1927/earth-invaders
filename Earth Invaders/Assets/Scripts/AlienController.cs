using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [SerializeField] private float segmentsPerUnityUnit = 4f;
    [Header("Horizontal Properties:")]
    [SerializeField] private float horizontalPadding = 0.2f;
    [SerializeField] private float horizontalMoveSpeed = 5f;
    [SerializeField] private float distanceToWall = 5f;
    [SerializeField] private float timeBetweenHorizontalAliens = 0.2f;
    [SerializeField] private List<GameObject> alienRows;
    [Header("Vertical Properties:")]
    [SerializeField] private float verticalPadding = 0.0f;
    [SerializeField] private float verticalMoveSpeed = 2f;
    [SerializeField] private float distanceUpFromCenter = 0f;
    [SerializeField] private float timeBetweenVerticalAliens = 0.2f;
    [SerializeField] private int alienColumns = 5;
    
    //private float newAlienRows;
    //private float newAlienColumns;
    private GameObject[,] aliens;
    private int moveDirection = 1;
    private bool movingCoroutineRunning;
    private Coroutine movingCoroutine;
    private bool movingDown = false;
    float currentPosition = 0;
    private int timesRan = 0;
    
    void Start()
    {
        //newAlienRows = alienRows.Count * (1f + horizontalPadding);
        //newAlienColumns = alienColumns * (1f + verticalPadding);
        aliens = new GameObject[alienRows.Count, alienColumns];
        SpawnAliens();
    }

    private void Update()
    {
        if (!movingCoroutineRunning)
        {
            movingCoroutineRunning = true;
            movingCoroutine =StartCoroutine(MoveAliens());
        }
    }

    public float GetSegmentsPerUnityUnit()
    {
        return segmentsPerUnityUnit;
    }

    public float GetMoveSpeed()
    {
        return horizontalMoveSpeed;
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
            startPositionY = -((alienRows.Count - 1) / 2f) * (1 + verticalPadding) - 1 - distanceUpFromCenter;
        }
        else
        {
            startPositionY = -((int) Math.Floor(alienRows.Count / 2f) * (1 + verticalPadding)) - 1 + distanceUpFromCenter;
        }
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
        float alienCount = 1;
        
        while (!movingDown)
        {
            for (int i = 0; i <= alienRows.Count - 1; i++)
            {
                for (int j = alienColumns - 1; j >= 0; j--)
                {
                    GameObject currentAlienObject = aliens[i, j];
                    Alien currentAlien = currentAlienObject.GetComponent<Alien>();
                    currentAlien.SwitchSprite();
                    currentAlien.Move(new Vector3(horizontalMoveSpeed * moveDirection, 0, 0));


                    yield return new WaitForSeconds(timeBetweenHorizontalAliens);
                }
            }



            currentPosition += horizontalMoveSpeed * moveDirection;
            Debug.Log(currentPosition);
            if (moveDirection > 0 &&
                currentPosition > 0 + (alienColumns * (1 + horizontalPadding)) / 2 - distanceToWall)
            {
                Debug.Log("switch");
                moveDirection = moveDirection * -1;
                movingDown = true;
                StartCoroutine(MoveDown());

            }
            else if (moveDirection < 0 &&
                     currentPosition < 0 - (alienColumns * (1 + horizontalPadding)) / 2 + distanceToWall)
            {
                Debug.Log("switch");
                moveDirection = moveDirection * -1;
                movingDown = true;
                StartCoroutine(MoveDown());
            }

            timesRan++;
            if (timesRan > 10000)
            {
                Debug.Log("infinite loop");
                movingDown = true;
            }
        }
    }

    private IEnumerator MoveDown()
    {
        for (int i = 0; i <= alienRows.Count - 1; i++)
        {
            for (int j = 0; j <= alienColumns -1; j++)
            {
                aliens[i,j].GetComponent<Alien>().Move(new Vector3(0, -verticalMoveSpeed,0));
            }
            yield return new WaitForSeconds(timeBetweenVerticalAliens);
        }

        movingDown = false;
        StartCoroutine(MoveAliens());

    }
    
}

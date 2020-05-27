using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienController : MonoBehaviour
{
    [SerializeField] private float segmentsPerUnityUnit = 4f;
    [SerializeField] private float shootChance = 4f;
    [SerializeField] private bool moveAtOnce = false;
    [SerializeField] private float fasterSpeedMultiplier = 1f;
    [SerializeField] private float maxSpeed = 5f;
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
    private float startPositionX;
    private float startPositionY;
    private int alienCount = 0;
    private List<GameObject> alienList = new List<GameObject>();
    private float fasterMoveSpeed = 0f;
    
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
            movingCoroutine = StartCoroutine(MoveAliens());
        }
    }

    public float GetSegmentsPerUnityUnit()
    {
        return segmentsPerUnityUnit;
    }

    public float GetMoveSpeed()
    {
        return fasterMoveSpeed * moveDirection;
    }

    public void AddAlien(GameObject alien)
    {
        alienCount++;
        alienList.Add(alien);
    }

    public void RemoveAlien(GameObject alien)
    {
        alienCount--;
        alienList.Remove(alien);
    }

    public int GetAlienCount()
    {
        return alienCount;
    }

    public float GetAverageAlienPosition()
    {
        float totalX = 0f;
        foreach (GameObject alien in alienList)
        {
            totalX += alien.transform.position.x;
        }

        return totalX / alienList.Count;
    }

    public List<GameObject> GetAlienList()
    {
        return alienList;
    }

    private void SpawnAliens()
    {
        CheckIfEvenOrOdd();
        
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

    private void CheckIfEvenOrOdd()
    {
        if (alienColumns % 2 == 0)
        {
            startPositionX = -((alienColumns - 1) / 2f) * (1 + horizontalPadding);
        }
        else
        {
            startPositionX = -((int) Math.Floor(alienColumns / 2f) * (1 + horizontalPadding));
        }

        if (alienRows.Count % 2 == 0)
        {
            startPositionY = -((alienRows.Count - 1) / 2f) * (1 + verticalPadding) - 1 - distanceUpFromCenter;
        }
        else
        {
            startPositionY = -((int) Math.Floor(alienRows.Count / 2f) * (1 + verticalPadding)) - 1 + distanceUpFromCenter;
        }
    }

    private IEnumerator MoveAliens()
    {
        while (!movingDown)
        {
            fasterMoveSpeed = Mathf.Clamp((((float)(alienColumns * alienRows.Count) / (float)alienCount) - 1f) * fasterSpeedMultiplier + horizontalMoveSpeed, 0, maxSpeed);
            Debug.Log("New move speed is " + fasterMoveSpeed);
            for (int i = 0; i <= alienRows.Count - 1; i++)
            {
                for (int j = alienColumns - 1; j >= 0; j--)
                {
                    GameObject currentAlienObject = aliens[i, j];
                    if (currentAlienObject)
                    {
                        Alien currentAlien = currentAlienObject.GetComponent<Alien>();
                        currentAlien.SwitchSprite();
                        currentAlien.Move(new Vector3((fasterMoveSpeed) * moveDirection, 0, 0));

                        Shoot(i, j, currentAlien);
                    }


                    if (!moveAtOnce)
                    {
                        yield return new WaitForSeconds(timeBetweenHorizontalAliens);
                    }
                }

                if (moveAtOnce)
                {
                    yield return new WaitForSeconds(timeBetweenHorizontalAliens);
                }
            }

            
            CheckIfEndReached();
        }
    }

    private void CheckIfEndReached()
    {
        currentPosition += fasterMoveSpeed * moveDirection;
        if (moveDirection > 0 &&
            currentPosition > 0 + (alienColumns * (1 + horizontalPadding)) / 2 - distanceToWall)
        {
            moveDirection = moveDirection * -1;
            movingDown = true;
            StartCoroutine(MoveDown());
        }
        else if (moveDirection < 0 &&
                 currentPosition < 0 - (alienColumns * (1 + horizontalPadding)) / 2 + distanceToWall)
        {
            moveDirection = moveDirection * -1;
            movingDown = true;
            StartCoroutine(MoveDown());
        }
    }

    private void Shoot(int i, int j, Alien currentAlien)
    {
        bool alienBelow = false;
        for (int k = i - 1; k >= 0; k--)
        {
            GameObject testingAlien = aliens[k, j];

            if (testingAlien)
            {
                alienBelow = true;
            }
        }

        if (!alienBelow)
        {
            if (Random.Range(0f, 1f) <= 1f / shootChance)
            {
                currentAlien.Shoot();
            }
        }
    }

    private IEnumerator MoveDown()
    {
        for (int i = 0; i <= alienRows.Count - 1; i++)
        {
            for (int j = 0; j <= alienColumns -1; j++)
            {
                GameObject alien = aliens[i, j];
                if (alien)
                {
                    alien.GetComponent<Alien>().Move(new Vector3(0, -verticalMoveSpeed,0));
                }
            }
            yield return new WaitForSeconds(timeBetweenVerticalAliens);
        }

        movingDown = false;
        StartCoroutine(MoveAliens());

    }
    
}

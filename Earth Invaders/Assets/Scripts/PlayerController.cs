﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{ 
    private SpriteRenderer spriteRenderer;
    private float movementSpeed = 2f;
    private float segmentsPerUnityUnit = 2f;
    private float padding = 0.5f;
    private float xPos;
    private float yPos;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private Vector3 tempTransform;
    private Sprite[] sprites;
    private int currentSprite = -1;
    private int movementCounter = 0;

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    public void SetSegmentsPerUnityUnit(float segments)
    {
        segmentsPerUnityUnit = segments;
    }

    public void SetPadding(float padding)
    {
        this.padding = padding;
    }
    void Start()
    {
        tag = "Player";
        tempTransform = transform.position;
        transform.parent = null;
        xPos = transform.position.x;
        yPos = transform.position.y;
        sprites = GetComponent<Alien>().GetSprites();
        SetupMoveBoundaries();
        SetupSprite();
    }

    private void SetupSprite()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.color = Color.green;
        spriteRenderer.sprite = sprites[(currentSprite + 1) / 2];
    }

    void Update()
    {
        Move();
        Animate();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed * (1f + 1f / 4f);
        float inputY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed * (1f + 1f / 4f);

        xPos = Mathf.Clamp(xPos + inputX, xMin, xMax);
        yPos = Mathf.Clamp(yPos + inputY, yMin, yMax);

        

        var newX = Mathf.RoundToInt(xPos * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        var newY = Mathf.RoundToInt(yPos * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        
        transform.position = new Vector3(newX, newY); 
        
    }

    private void Animate()
    {
        if (transform.position != tempTransform)
        {
            movementCounter++;
            tempTransform = transform.position;
            if (movementCounter >= movementSpeed * 2)
            {
                currentSprite  = currentSprite * -1;
                spriteRenderer.sprite = sprites[(currentSprite + 1) / 2];
                movementCounter = 0;
            }
        }
    }

    private void SetupMoveBoundaries()
    {
        Camera camera = Camera.main;
        xMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}

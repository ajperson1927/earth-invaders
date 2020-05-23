using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    private float movementSpeed = 2f;
    private float segmentsPerUnityUnit = 2f;
    private float padding = 0.5f;
    private float xPos;
    private float yPos;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

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
        transform.parent = null;
        xPos = transform.position.x;
        yPos = transform.position.y;
        SetupMoveBoundaries();
        GetComponent<SpriteRenderer>().color = Color.green;
    }
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float inputY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        xPos = Mathf.Clamp(xPos + inputX, xMin, xMax);
        yPos = Mathf.Clamp(yPos + inputY, yMin, yMax);

        

        var newX = Mathf.RoundToInt(xPos * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        var newY = Mathf.RoundToInt(yPos * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        
        transform.position = new Vector2(newX, newY); 

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

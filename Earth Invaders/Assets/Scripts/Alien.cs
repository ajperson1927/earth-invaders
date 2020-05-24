using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float segmentsPerUnityUnit = 4f;
    [SerializeField] private float padding = 0.5f;
    [SerializeField] Sprite[] sprites = new Sprite[2];
    private int currentSprite = -1;
    private Vector3 unroundedPos;
    
    void Start()
    {
        unroundedPos = transform.position;
        segmentsPerUnityUnit = GetComponentInParent<AlienController>().GetSegmentsPerUnityUnit();
    }

    private void Update()
    {
        //Move();
    }

    private void OnMouseDown()
    {
        if (!GetComponent<PlayerControl>())
        {
            PlayerControl playerControl = gameObject.AddComponent<PlayerControl>();
            playerControl.SetMovementSpeed(movementSpeed);
            playerControl.SetSegmentsPerUnityUnit(segmentsPerUnityUnit);
            playerControl.SetPadding(padding);
        }
    }

    public void Move(Vector3 moveBy)
    {
        unroundedPos += moveBy;

        var newX = Mathf.RoundToInt(unroundedPos.x * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        var newY = Mathf.RoundToInt(unroundedPos.y * segmentsPerUnityUnit) / segmentsPerUnityUnit;

        if (!GetComponent<PlayerControl>())
        {
            transform.position = new Vector2(newX, newY);
        }
    }

    public Sprite[] GetSprites()
    {
        return sprites;
    }

    public void SwitchSprite()
    {
        currentSprite = currentSprite * -1;
        if (!GetComponent<PlayerControl>())
        {
            GetComponent<SpriteRenderer>().sprite = sprites[(currentSprite + 1) / 2];
        }
    }
}

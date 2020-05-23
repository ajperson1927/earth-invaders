using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float segmentsPerUnityUnit = 2f;
    [SerializeField] private float padding = 0.5f;
    [SerializeField] Sprite[] sprites = new Sprite[2];
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
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

    public Sprite[] GetSprites()
    {
        return sprites;
    }
}

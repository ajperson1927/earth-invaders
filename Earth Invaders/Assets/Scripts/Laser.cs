using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LaserDetector laserDetector;

    private void Start()
    {
        laserDetector = FindObjectOfType<LaserDetector>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CompareTag("Player Laser") && other.gameObject.CompareTag("Enemy Bullet"))
        {
            Destroy(other.gameObject);
        }
        else if (CompareTag("Alien Laser") && other.gameObject.CompareTag("Enemy Bullet"))
        {
            Destroy(other.gameObject);
            laserDetector.RemoveLaser(gameObject);
            Destroy(gameObject);
        }
    }
}

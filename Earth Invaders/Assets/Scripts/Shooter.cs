using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 2;
    [SerializeField] private float laserDelay = 0.5f;
    [SerializeField] private float laserDownAmount = 0.5f;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private int maxShots = 5;

    private bool canShoot = true;
    private Coroutine shootingCoroutine;
    private bool shootingCoroutineRunning = false;
    private LaserDetector laserDetector;
    private float health = 0f;
    private SpriteRenderer spriteRenderer;
    private bool dying = false;

    private void Start()
    {
        laserDetector = FindObjectOfType<LaserDetector>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxShots;
    }

    void Update()
    {
        if (CompareTag("Player"))
        {
            spriteRenderer.color = healthGradient.Evaluate(health / maxShots);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && CompareTag("Player") && !dying)
        {
            shootingCoroutine = StartCoroutine(Shoot());
        } else if (Input.GetKeyUp(KeyCode.Space) && shootingCoroutineRunning)
        {
            StopCoroutine(shootingCoroutine);
        }
    }

    private IEnumerator Shoot()
    {
        while (!dying)
        {
            shootingCoroutineRunning = true;
            Vector3 laserPosition = new Vector3(transform.position.x,transform.position.y - laserDownAmount,0);
            GameObject laser = Instantiate(laserPrefab, laserPosition, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = Vector2.down * laserSpeed;
            laser.tag = "Player Laser";
            laserDetector.AddLaser(laser);
            health--;
            if (health < 0)
            {
                GameObject deathLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                deathLaser.tag = "Enemy Bullet";
                dying = true;
                //Destroy(GetComponent<Shooter>());
            }
            yield return new WaitForSeconds(laserDelay);
        }
    }
}

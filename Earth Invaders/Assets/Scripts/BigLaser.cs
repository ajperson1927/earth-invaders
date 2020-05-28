using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLaser : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Gradient rechargeGradient;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private float laserSpeed = 3f;
    [SerializeField] private float laserDownAmount = 1f;
    
    private float currentTime = 0f;
    private bool canShoot = true;
    private float rechargeAmount = 0f;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        currentTime = shootDelay;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (CompareTag("Player"))
        {
            if (currentTime >= shootDelay)
            {
                currentTime = shootDelay;
                canShoot = true;
            }
            else
            {
                canShoot = false;
                currentTime += Time.deltaTime;
            }

            spriteRenderer.color = rechargeGradient.Evaluate((currentTime / shootDelay));

            if (canShoot && Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y - laserDownAmount, 0);
                GameObject laser = Instantiate(laserPrefab, laserPosition, Quaternion.identity);
                laser.GetComponent<Rigidbody2D>().velocity = Vector2.down * laserSpeed;
                laser.tag = "Player Laser";
                FindObjectOfType<LaserDetector>().AddLaser(laser);
                currentTime = 0;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
    
}

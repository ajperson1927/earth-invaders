using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestoyer : MonoBehaviour
{
    [SerializeField] private Sprite explosionSprite;
    [SerializeField] private float explosionTimer = 1f;
    [SerializeField] private float destroyTimer = 1f;
    [SerializeField] private float laserMoveAmount = 0.2f;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy Bullet"))
        {
            StartCoroutine(RedLaser(other.gameObject));
        }
    }

    private IEnumerator RedLaser(GameObject laser)
    {
        laser.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(explosionTimer);
        StartCoroutine(ExplodeLaser(laser));
    }

    private IEnumerator ExplodeLaser(GameObject laser)
    {
        SpriteRenderer spriteRenderer = laser.GetComponent<SpriteRenderer>();
        laser.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        laser.transform.rotation = Quaternion.Euler(0,0,180);
        laser.transform.localScale = new Vector3(1,1.25f,0);
        laser.transform.position = new Vector3(laser.transform.position.x, transform.position.y + laserMoveAmount,0);
        spriteRenderer.sprite = explosionSprite;
        yield return new WaitForSeconds(destroyTimer);
        Destroy(laser);
    }

    
}

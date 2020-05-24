using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 2f;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private bool isShooting = false;

    private bool currentlyShooting = false;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (!currentlyShooting && isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        currentlyShooting = true;
        GameObject laser = Instantiate(laserPrefab,transform.position,Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = Vector2.up * laserSpeed;
        laser.tag = "Enemy Bullet";
        yield return new WaitForSeconds(shootDelay);
        currentlyShooting = false;
    }
}

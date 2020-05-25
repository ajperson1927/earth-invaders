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
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 2f;
    [SerializeField] float explodeDelay = 1f;

    [SerializeField] Sprite explodeSprite;

    private int currentSprite = -1;
    private Vector3 unroundedPos;
    private Color color;
    private AlienController alienController;
    
    void Start()
    {
        tag = "Alien";
        color = GetComponent<SpriteRenderer>().color;
        unroundedPos = transform.position;
        segmentsPerUnityUnit = GetComponentInParent<AlienController>().GetSegmentsPerUnityUnit();
        alienController = FindObjectOfType<AlienController>();
        alienController.AddAlien(gameObject);
    }

    private void OnMouseDown()
    {
        if (!GetComponent<PlayerController>())
        {
            PlayerController playerController = gameObject.AddComponent<PlayerController>();
            playerController.SetMovementSpeed(movementSpeed);
            playerController.SetSegmentsPerUnityUnit(segmentsPerUnityUnit);
            playerController.SetPadding(padding);
            FindObjectOfType<LevelController>().AddNewPlayer(gameObject);
        }
    }

    public void Move(Vector3 moveBy)
    {
        unroundedPos += moveBy;

        var newX = Mathf.RoundToInt(unroundedPos.x * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        var newY = Mathf.RoundToInt(unroundedPos.y * segmentsPerUnityUnit) / segmentsPerUnityUnit;

        if (CompareTag("Alien"))
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
        if (CompareTag("Alien"))
        {
            GetComponent<SpriteRenderer>().sprite = sprites[(currentSprite + 1) / 2];
        }
    }

    public void RemovePlayer()
    {
        Destroy(GetComponent<PlayerController>());
        tag = "Alien";
        GetComponent<SpriteRenderer>().color = color;
        Move(new Vector3(0,0,0));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy Bullet") && !CompareTag("Explosion"))
        {
            
            Destroy(other.gameObject);
            alienController.RemoveAlien(gameObject);
            StartCoroutine(Explode());
        }
    }
    public void Shoot()
    {
        if (CompareTag("Alien"))
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.tag = "Alien Laser";
            laser.GetComponent<Rigidbody2D>().velocity = Vector2.down * laserSpeed;
            FindObjectOfType<LaserDetector>().AddLaser(laser);
        }
        
        
    }

    private IEnumerator Explode()
    {
        tag = "Explosion";
        GetComponent<SpriteRenderer>().sprite = explodeSprite;
        yield return new WaitForSeconds(explodeDelay);
        Destroy(gameObject);
    }
}

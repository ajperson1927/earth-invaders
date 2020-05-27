using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed = 2f;
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
    private int playerLives = 3;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        tag = "Alien";
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
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
            playerController.SetMovementSpeed(playerMovementSpeed);
            playerController.SetSegmentsPerUnityUnit(segmentsPerUnityUnit);
            playerController.SetPadding(padding);
            FindObjectOfType<LevelController>().AddNewPlayer(gameObject);
            ShieldController shieldController = GetComponent<ShieldController>();
            if (shieldController)
            {
                shieldController.PlayerAttached();
            }
        }
    }

    public void Move(Vector3 moveBy)
    {
        unroundedPos += moveBy;

        //var newX = Mathf.RoundToInt(unroundedPos.x * segmentsPerUnityUnit) / segmentsPerUnityUnit;
        //var newY = Mathf.RoundToInt(unroundedPos.y * segmentsPerUnityUnit) / segmentsPerUnityUnit;

        if (CompareTag("Alien"))
        {
            transform.position = unroundedPos; //new Vector2(newX, newY);
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
            spriteRenderer.sprite = sprites[(currentSprite + 1) / 2];
        }
    }

    public void RemovePlayer()
    {
        Destroy(GetComponent<PlayerController>());
        tag = "Alien";
        GetComponent<SpriteRenderer>().color = color;
        Move(new Vector3(0,0,0));
        ShieldController shieldController = GetComponent<ShieldController>();
        if (shieldController)
        {
            shieldController.PlayerDetached();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy Bullet") && !CompareTag("Explosion"))
        {
            
            Destroy(other.gameObject);
            if (CompareTag("Player"))
            {
                playerLives--;
                switch (playerLives)
                {
                    case 2:
                        spriteRenderer.color = Color.yellow;
                        break;
                    case 1:
                        spriteRenderer.color = Color.red;
                        break;
                    case 0:
                        StartCoroutine(Explode());
                        break;
                }
            }
            else
            {
                StartCoroutine(Explode());
            }
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
        if (CompareTag("Player"))
        {
            Destroy(GetComponent<PlayerController>());
            ShieldController shieldController = GetComponent<ShieldController>();
            if (shieldController)
            {
                shieldController.PlayerDetached();
            }
        }
        alienController.RemoveAlien(gameObject);
        tag = "Explosion";
        spriteRenderer.sprite = explodeSprite;
        yield return new WaitForSeconds(explodeDelay);
        Destroy(gameObject);
    }
}

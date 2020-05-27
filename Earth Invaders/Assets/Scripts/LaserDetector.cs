using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    [SerializeField] private float sideLaserDistance = 7f;
    [SerializeField] private float sideLaserDistanceUp = 1f;

    [SerializeField] Sprite explosionSprite;
    [SerializeField] private float explosionTime = 1f;
    [SerializeField] private float explosionDistanceUp = 1f;
    private List<GameObject> lasers = new List<GameObject>();

    private void Start()
    {
        CreateSideLasers();
    }

    private void CreateSideLasers()
    {
        GameObject leftLaser = new GameObject("Left Laser");
        //leftLaser.transform.parent = transform;
        leftLaser.tag = "Alien Laser";
        leftLaser.transform.position = new Vector3(-sideLaserDistance, transform.position.y + sideLaserDistanceUp, 0);
        AddLaser(leftLaser);

        GameObject rightLaser = new GameObject("Right Laser");
        //rightLaser.transform.parent = transform;
        rightLaser.tag = "Alien Laser";
        rightLaser.transform.position = new Vector3(sideLaserDistance, transform.position.y + sideLaserDistanceUp, 0);
        AddLaser(rightLaser);
    }

    public void AddLaser(GameObject laser)
    {
        lasers.Add(laser);
        laser.transform.parent = transform;
    }

    public void RemoveLaser(GameObject laser)
    {
        if (lasers.Contains(laser))
        {
            lasers.Remove(laser);
        }
    }

        public List<GameObject> GetLasers()
    {
        return lasers;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Alien Laser") || other.gameObject.CompareTag("Player Laser"))
        {
            lasers.Remove(other.gameObject);
            StartCoroutine(DestroyLaser(other.gameObject));
        }
    }

    private IEnumerator DestroyLaser(GameObject laser)
    {
        laser.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Destroy(laser.GetComponent<BoxCollider2D>());
        laser.GetComponent<SpriteRenderer>().sprite = explosionSprite;
        laser.GetComponent<SpriteRenderer>().color = new Color(0,255,0,255);
        laser.transform.localScale = new Vector3(1,1.25f,0);
        laser.transform.position = new Vector3(laser.transform.position.x, transform.position.y + explosionDistanceUp, 0);
        yield return new WaitForSeconds(explosionTime);
        Destroy(laser);
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    [SerializeField] private float sideLaserDistance = 7f;
    [SerializeField] private float sideLaserDistanceUp = 1f;
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

    public List<GameObject> GetLasers()
    {
        return lasers;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        lasers.Remove(other.gameObject);
        Destroy(other.gameObject);
    }
}

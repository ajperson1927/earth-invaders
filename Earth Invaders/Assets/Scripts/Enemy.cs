using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 2f;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private float verticalLaserRange = 3f;
    [SerializeField] private float horizontalLaserRange = 0.5f;

    [SerializeField] private bool orderLasers = false;
    [SerializeField] private float moveSpeed = 5f;
    
    private bool currentlyShooting = false;
    private LaserDetector laserDetector;
    
    void Start()
    {
        laserDetector = FindObjectOfType<LaserDetector>();
    }

    void Update()
    {
        if (!currentlyShooting && isShooting)
        {
            StartCoroutine(Shoot());
        }

        if (!RunFromLasers())
        {
            AvoidLasers();
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

    private void AvoidLasers()
    {
        List<GameObject> closeLasers = new List<GameObject>();
        foreach (var laser in laserDetector.GetLasers())
        {
            float distanceBetween = laser.transform.position.y - transform.position.y;
            if (distanceBetween < verticalLaserRange && distanceBetween < 0) ;
            {
                closeLasers.Add(laser);
            }
        }

        var ordered = closeLasers.OrderBy(gameObject => gameObject.transform.position.x);
        List<GameObject> orderedLasers = new List<GameObject>();
        foreach (GameObject laser in ordered)
        {
            orderedLasers.Add(laser);
        }

        int safeIndex = 1;
        float safestDistance = 0f;
        for (int i = 1; i < orderedLasers.Count(); i++)
        {
            if (orderedLasers[i].transform.position.x - orderedLasers[i - 1].transform.position.x > safestDistance)
            {
                safestDistance = orderedLasers[i].transform.position.x - orderedLasers[i - 1].transform.position.x;
                safeIndex = i;
            }
        }

        float finalPosition = (orderedLasers[safeIndex].transform.position.x + orderedLasers[safeIndex - 1].transform.position.x) / 2f;
        float newPosition = Mathf.MoveTowards(transform.position.x, finalPosition, moveSpeed * Time.deltaTime);
        transform.position = new Vector2(newPosition, transform.position.y);
    }

    private bool RunFromLasers()
    {
        List<GameObject> closeLasers = new List<GameObject>();
        foreach (var laser in laserDetector.GetLasers())
        {
            if (laser.transform.position.y - transform.position.y < verticalLaserRange)
            {
                closeLasers.Add(laser);
            }
        }

        int closestIndex = 0;
        float closestDistance = 1000f;
        bool running = false;
        for (int i = 0; i < closeLasers.Count; i++)
        {
            if (Mathf.Abs(closeLasers[i].transform.position.x - transform.position.x) < closestDistance)
            {
                if (Mathf.Abs(closeLasers[i].transform.position.x - transform.position.x) < horizontalLaserRange)
                {
                    running = true;
                    closestIndex = i;
                }
            }
        }


        if (Mathf.Abs(closeLasers[closestIndex].transform.position.x - transform.position.x) > horizontalLaserRange)
        {
            running = false;
        }
        if (closeLasers.Count > 2 && running)
        {
            float avoidPosition = closeLasers[closestIndex].transform.position.x;
            float newPosition = Mathf.MoveTowards(transform.position.x, avoidPosition, -moveSpeed * Time.deltaTime);
            transform.position = new Vector2(newPosition, transform.position.y);
        }

        return running;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float distanceDown = 1f;

    private SpriteRenderer spriteRenderer;
    private GameObject shield;
    void Start()
    {
        Vector3 shieldPosition = new Vector3(0, transform.position.y - distanceDown, 0);
        shield = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);
        shield.SetActive(false);
        shield.transform.parent = transform;
        spriteRenderer = shield.GetComponent<SpriteRenderer>();
    }
    
    public void PlayerAttached()
    {
        shield.SetActive(true);
    }

    public void PlayerDetached()
    {
        shield.SetActive(false);
    }

    public void DestroyShield()
    {
        Destroy(shield);
        Destroy(GetComponent<ShieldController>());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int shieldLives = 3;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy Bullet"))
        {
            shieldLives--;
            switch (shieldLives)
            {
                case 2:
                    spriteRenderer.color = Color.yellow;
                    break;
                case 1:
                    spriteRenderer.color = Color.red;
                    break;
                case 0:
                    GetComponent<ShieldController>().DestroyShield();
                    break;
            }
        }
    }
}

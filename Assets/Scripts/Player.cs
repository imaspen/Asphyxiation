﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    float oxygenTank = 100; // players health
    
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    public int jumpForce = 1000;
    public Sprite[] rightSprites = new Sprite[3];
    public Sprite[] leftSprites = new Sprite[3];
    public Sprite centerSprite;
    public float speed = 3.0f;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var move = Input.GetAxis("Horizontal");
        UpdateSprite(move);
        
        //transform.position += move * speed * Time.deltaTime;
        MovePlayer(move);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        LoseOxygenOverTime();
    }

    void MovePlayer(float direction)
    {
        rigidbody2D.velocity = new Vector2(direction * speed, rigidbody2D.velocity.y);
    }

    void UpdateSprite(float direction)
    {
        float time = (Time.fixedTime % 1);
        int sprite = 0;
        if (time < 0.33)
        {
            sprite = 0;
        }
        else if (time < 0.66)
        {
            sprite = 1;
        }
        else sprite = 2;

        if (direction > 0)
        {
            spriteRenderer.sprite = rightSprites[sprite];
        }
        else if (direction < 0)
        {
            spriteRenderer.sprite = leftSprites[sprite];
        }
        else
        {
            spriteRenderer.sprite = centerSprite;
        }
    }

    void LoseOxygenOverTime()
    {
        oxygenTank -= 1 * Time.deltaTime;
    }
}

using System.Collections;
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
    void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        var sprite = 0;
        float time = (Time.fixedTime % 1);
        if (time < 0.33)
        {
            sprite = 0;
        }
        else if (time < 0.66)
        {
            sprite = 1;
        }
        else sprite = 2;

        if (move.x > 0)
        {
            spriteRenderer.sprite = rightSprites[sprite];
        } else if (move.x < 0)
        {
            spriteRenderer.sprite = leftSprites[sprite];
        } else
        {
            spriteRenderer.sprite = centerSprite;
        }

        transform.position += move * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        LoseOxygenOverTime();
    }

    void LoseOxygenOverTime()
    {
        oxygenTank -= 1 * Time.deltaTime;
    }
}

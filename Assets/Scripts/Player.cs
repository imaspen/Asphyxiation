using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    float oxygenTank = 100; // players health
    
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    public float jumpForce;
    public Sprite[] rightSprites = new Sprite[3];
    public Sprite[] leftSprites = new Sprite[3];
    public Sprite centerSprite;
    public float speed = 3.0f;
    public Transform[] groundPoints = new Transform[3];
    public LayerMask ground;

    bool isGrounded;
    bool jump;

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

        isGrounded = IsGrounded();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        UpdateSprite(move);
        MovePlayer(move);

        LoseOxygenOverTime();
    }

    void MovePlayer(float direction)
    {
        rigidbody2D.velocity = new Vector2(direction * speed, rigidbody2D.velocity.y);
        if (isGrounded && jump)
        {
            isGrounded = false;
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }
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

    private bool IsGrounded()
    {
        if (rigidbody2D.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 0.1f, ground);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject) return true;
                }
            }
        }
        return false;
    }
}

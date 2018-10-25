using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{

    public int maxOxygenCapacity = 3;
    public static float oxygenTank = 1; // players health
    public float collectedTanks = 0;

    public float stunCoolDown = 0;
    public bool stunAvailable = true;

    public int score = 0;
    //public Text scoreText; // include when text on canvas

    float curTime = 0; // damage delay on collision
    float nextDamage = 1;
    float damageDelay;

    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    public float jumpForce;
    public Sprite[] rightSprites = new Sprite[3];
    public Sprite[] leftSprites = new Sprite[3];
    public Sprite centerSprite;
    public float speed = 3.0f;
    public Transform[] groundPoints = new Transform[3];
    public LayerMask ground;
    public LayerMask ladder;

    bool isGrounded;
    BoxCollider2D climbing = null;
    bool jump;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var move = Input.GetAxis("Horizontal");
        HandleInput();

        isGrounded = IsGrounded();

        UpdateSprite(move);
        MovePlayer(move);

        LoseOxygenOverTime();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (climbing != null)
            {
                climbing = null;
                rigidbody2D.isKinematic = false;
            }
            else
            {
                CanClimb();
            }
        }
    }

    void MovePlayer(float direction)
    {
        if (climbing != null)
        {
            transform.position = new Vector2(climbing.transform.position.x, transform.position.y);
            var move = Input.GetAxis("Vertical");
            var ladderTop = climbing.transform.position.y + (climbing.size.y / 2);
            var ladderBottom = climbing.transform.position.y - (climbing.size.y / 2);

            if (groundPoints[0].position.y >= ladderTop && move >= 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                transform.position = new Vector2(transform.position.x, ladderTop + boxCollider.size.y / 2);
            }
            else if (groundPoints[0].position.y <= ladderBottom && move <= 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                transform.position = new Vector2(transform.position.x, ladderBottom + boxCollider.size.y / 2);
            }
            else
            {
                rigidbody2D.velocity = new Vector2(0, move * speed);
            }
        }
        else
        {
            rigidbody2D.velocity = new Vector2(direction * speed, rigidbody2D.velocity.y);
            if (isGrounded && jump)
            {
                isGrounded = false;
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
                jump = false;
            }
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
        if (oxygenTank > 0)
        {
           
            oxygenTank -= 0.05f * Time.deltaTime;
            Debug.Log(oxygenTank);
            if (oxygenTank <= 0.0)
            {
                SceneManager.LoadScene("gameover");
            }

        }
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

    private void CanClimb()
    {
        foreach (Transform point in groundPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 0.1f, ladder);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    climbing = (BoxCollider2D) colliders[i];
                    rigidbody2D.isKinematic = true;
                    return;
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Oxygen Canister"))
        {
            collectedTanks++;
            oxygenTank += 0.25f;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Gem")
        {
            score += 10;
            updateScoreText();
            collision.gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

            if (stunAvailable)
            {
                StartCoroutine(StunEnemy());
            }

        }
    }

    IEnumerator StunEnemy()
    {
        var enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        var enemyBody = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody2D>();
        enemy.chase = false;
        Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        enemy.enabled = false;

        stunCoolDown = 20;
        yield return new WaitForSeconds(5);

        enemy.chase = true;
        Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), false);
        enemy.enabled = true;
    }

    void StunCoolDown()
    {
        if (stunCoolDown > 0)
        {
            stunAvailable = false;
            stunCoolDown -= Time.deltaTime;
        }
        else
        {
            stunAvailable = true;
        }
    }

    private void updateScoreText()
    {
        // scoreText.text = score.ToString(); //include when text on canvas
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            oxygenTank -= 1.5f * Time.deltaTime;

            Debug.Log("-0.05");

            if (oxygenTank <= 0.0)
            {
                SceneManager.LoadScene("gameover");
            }

            if (curTime <= 0)
            {
                Debug.Log("Damage");

                curTime = nextDamage;
            }
            else
            {
                curTime -= Time.deltaTime;
            }

            damageDelay = 15;
        }

    }
}

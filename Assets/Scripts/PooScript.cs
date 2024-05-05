using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooScript : MonoBehaviour
{
    public AudioSource deathSource;
    public float pooSpeed;
    public int chanceToFall;
    public bool isFalling;


    public float lastSpeed;
    private float noSpeed = 0;
    Rigidbody2D rb;
    Collider2D col;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        lastSpeed = pooSpeed;
        chanceToFall = 1;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(pooSpeed, 0); 
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            pooSpeed = -(pooSpeed);
            lastSpeed = pooSpeed;
        }

        if (collision.gameObject.CompareTag("BreakPoo"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("JvLz"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.DeadPlayer();
            playerController.canMove = false;
            deathSource.Play();
            playerController.playerAnim.Play("Death");
            playerController.enabled = false;
            StartCoroutine(Death());
            Debug.Log("Dead");
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isFalling = false;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LadderCheck"))
        {
            chanceToFall = Random.Range(1, 5);
            if (chanceToFall == 1)
            {
                pooSpeed = noSpeed;
                isFalling = true; 
                if (isFalling)
                {
                    col.isTrigger = true;
                    lastSpeed = -(lastSpeed);
                    StartCoroutine(FallPoo());
                }
            }
        }
        if (collision.gameObject.CompareTag("JvLz"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.DeadPlayer();
            playerController.canMove = false;
            deathSource.Play();
            playerController.playerAnim.Play("Death");
            playerController.enabled = false;
            StartCoroutine(Death());
            Debug.Log("Dead");
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isFalling)
            {
                col.isTrigger = false;
                pooSpeed = lastSpeed;
                chanceToFall = 0;
            }
        }
    }


    IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        yield break;
    }

    IEnumerator FallPoo()
    {
        yield return new WaitForSeconds(0.3f);
        isFalling = false;
    }


}

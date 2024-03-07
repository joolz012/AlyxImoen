using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] playerClips;
    public CameraFollow cameraFollow;
    public GameObject playerMoveSfx;
    public GameObject gameOverCanvas;

    public SfxManager sfxManager;
    public DialogManager dialogManager;
    public GameObject attackManager;
    public Animator playerAnim;

    private float moveInputHorizontal;
    private float moveInputVertical;
    public float distance;
    public LayerMask ladderLayerMask;
    private bool isClimbing;

    
    public LayerMask groundLayerMask;
    public float jumpDistance;
    private bool isGrounded;

    public LayerMask pooLayerMask;
    public float pooDistance;
    private bool isPoo;

    public float moveSpeed;
    public float jumpForce;
    public float climbSpeed;

    private Rigidbody2D rb;
    public bool canMove = true;

    void Start()
    {
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); 
        isPoo = false;
    }

    void Update()
    {
        if (canMove)
        {
            ButtonController();
            //moveInputHorizontal = Input.GetAxisRaw("Horizontal");

            rb.velocity = new Vector2(moveInputHorizontal * moveSpeed, rb.velocity.y);
            if (moveInputHorizontal > 0)
            {
                transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
            else if (moveInputHorizontal < 0)
            {
                transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
            }

            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, jumpDistance, groundLayerMask);

            if (jump && isGrounded)
            {
                if (!isClimbing)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }

            RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.up, distance, ladderLayerMask);

            if (hitinfo.collider != null)
            {
                if (up)
                {
                    Debug.Log("Up");
                    isClimbing = true;
                }
                else if (left || right)
                {
                    isClimbing = false;
                }
            }
            else
            {
                isClimbing = false;
            }

            if (isClimbing && hitinfo.collider != null)
            {
                //moveInputVertical = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(rb.velocity.x, moveInputVertical * climbSpeed);
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 1;
            }


            RaycastHit2D checkPoo = Physics2D.Raycast(transform.position, Vector2.down, pooDistance, pooLayerMask);

            if (!isPoo && checkPoo.collider != null)
            {
                Debug.Log("Get Points");
                StartCoroutine(PointsTimer());
                isPoo = true;
            }

            AnimationController();
        }
        
    }

    public void DeadPlayer()
    {
        gameOverCanvas.SetActive(true);
    }
    bool up, down, left, right, jump;
    void ButtonController()
    {
        if(left)
        {
            moveInputHorizontal = -moveSpeed;
        }
        else if(right)
        {
            moveInputHorizontal = moveSpeed;
        }
        else
        {
            moveInputHorizontal = 0;
        }

        if (up && isClimbing)
        {
            moveInputVertical = climbSpeed;
        }
        else if (down && isClimbing)
        {
            moveInputVertical = -climbSpeed;
        }
        else
        {
            moveInputVertical = 0;
        }

        if (jump && isGrounded && !isClimbing)
        {
            audioSource.PlayOneShot(playerClips[0]);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void LeftButton()
    {
        left = true;
    }
    public void LeftButtonUp()
    {
        left = false;
    }
    public void RightButton()
    {
        right = true;
    }
    public void RightButtonUp()
    {
        right = false;
    }
    public void UpButton()
    {
        up = true;
    }
    public void UpButtonUp()
    {
        up = false;
    }
    public void DownButton()
    {
        down = true;
    }
    public void DownButtonUp()
    {
        down = false;
    }
    public void JumpButton()
    {
        jump = true;
    }
    public void JumpButtonUp()
    {
        jump = false;
    }

    public void IdleTrigger()
    {
        left = false;
        right = false;
        up = false;
        down = false;
        jump = false;
    }

    void AnimationController()
    {
        if (moveInputHorizontal > 0 && isGrounded)
        {
            playerMoveSfx.SetActive(true);
            playerAnim.Play("Run");
        }
        // If moving left
        else if (moveInputHorizontal < 0 && isGrounded)
        {
            playerMoveSfx.SetActive(true);
            playerAnim.Play("Run");
        }
        else if (moveInputHorizontal == 0 && isGrounded)
        {
            playerMoveSfx.SetActive(false);
            //IdleTrigger();
            playerAnim.Play("Idle");
        }
        else if (!isClimbing  && jump)
        {
            playerMoveSfx.SetActive(false);
            playerAnim.Play("Jump");
        }
        else if(isClimbing)
        {
            playerMoveSfx.SetActive(false);
            playerAnim.Play("Climb");
        }
    }

    IEnumerator PointsTimer()
    {
        yield return new WaitForSeconds(0.5f);
        isPoo = false;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alyx"))
        {
            StartCoroutine(dialogManager.CountdownBday());
            playerMoveSfx.SetActive(false);
            dialogManager.audioSource.volume = 0.7f;
            dialogManager.DelaySound();
            dialogManager.enabled = true;
            cameraFollow.yOffset = 0;
            playerAnim.Play("Idle"); 
            rb.velocity = new Vector2(0, 0);
            sfxManager.BdayBgm();
            attackManager.SetActive(false); 
            canMove = false;
        }
    }
}

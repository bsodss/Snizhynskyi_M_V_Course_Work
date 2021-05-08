
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float SpeedX = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform playerModel;

    float SpeedXMultiplier = 50f;
    bool isGround = false;
    bool isFacingRight = true;
    bool isJump = false;
    float horizontal;
    private FinishController finish;
    private bool isFinish = false;
    private LeverArm leverArm;
    bool isLevelArm = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<FinishController>();
        leverArm = FindObjectOfType<LeverArm>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("speedX", System.Math.Abs(horizontal));

        if (Input.GetKey(KeyCode.W) && isGround)
        {
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFinish)
            {
                finish.Finishlevel();
            }
            if (isLevelArm)
            {
                leverArm.ActivateLevelArm();
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * SpeedX * SpeedXMultiplier * Time.fixedDeltaTime, rb.velocity.y);
        if (horizontal > 0f && !isFacingRight)
        {
            FlipPlayer();
        }
        else if (horizontal < 0f && isFacingRight)
        {
            FlipPlayer();
        }
        if (isJump)
        {

            rb.AddForce(new Vector2(0f, 540f));
            isGround = false;
            isJump = false;
        }
    }

    void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = playerModel.localScale;
        playerScale.x *= -1;
        playerModel.localScale = playerScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();
        if (other.CompareTag("Finish"))
        {
            isFinish = false;
        }

        if (leverArmTemp != null)
        {
            isLevelArm = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();

        if (other.CompareTag("Finish"))
        {
            isFinish = true;
        }
        if (leverArmTemp != null)
        {
            isLevelArm = true;
        }
    }

}

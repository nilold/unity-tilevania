using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // config
    [SerializeField] float speed = 10f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float jumpForce = 10f;

    // state
    bool isAlive = true;
    float startingGravity;

    // cached component references
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private Collider2D collider;

	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        startingGravity = myRigidBody.gravityScale;
    }
	
	void Update () {
        Run();
        Jump();
        FlipSprite();
        ClimbLadder();
    }

    private void Run(){
        float xSpeed = CrossPlatformInputManager.GetAxis("Horizontal") * speed;
        myRigidBody.velocity = new Vector2(xSpeed, myRigidBody.velocity.y);

        if(Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon)
            animator.SetBool("running", true);
    }

    private void Jump(){
        bool jump = CrossPlatformInputManager.GetButtonDown("Jump");
        bool grounded = collider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (jump && grounded)
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpForce);
    }

    private void FlipSprite(){
        //if the player is moving horizontally
        bool playerIsMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x),1);
        }else{
            animator.SetBool("running", false);
        }
    }

    private void ClimbLadder(){
        bool touchingLadder = collider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if(touchingLadder){
            float ySpeed = CrossPlatformInputManager.GetAxis("Vertical") * climbSpeed;
            animator.SetBool("climbing", Mathf.Abs(ySpeed) > Mathf.Epsilon);
            //animator.SetBool("climbing", true);
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, ySpeed);
            myRigidBody.gravityScale = 0f;
        }else{
            animator.SetBool("climbing", false);
            myRigidBody.gravityScale = startingGravity;
        }
    }
}

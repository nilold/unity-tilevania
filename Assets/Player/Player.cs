using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // config
    [SerializeField] float speed = 10f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpHorizotalPush = 1.5f;
    [SerializeField] float deathDrama = 10f;
    [SerializeField] float respawnDelay = 3f;

    // state
    bool isAlive = true;
    bool freezed = false;
    float startingGravity;

    // cached component references
    private Rigidbody2D myRigidBody;
    private Animator animator;
    private Collider2D myCollider;
    private Collider2D feetCollider;

	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        feetCollider = GetComponentInChildren<BoxCollider2D>();
        startingGravity = myRigidBody.gravityScale;
    }
	
	void Update () {
        if (!isAlive || freezed) return;
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

        bool grounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool laddered = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));

        if (jump && (grounded || laddered)){
            float xVel = jumpHorizotalPush * myRigidBody.velocity.x;
            myRigidBody.velocity = new Vector2(xVel, jumpForce);
        }
           
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
        bool touchingLadder = myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyMovement>())
            Die();
        if(myRigidBody.IsTouchingLayers(LayerMask.GetMask("Letal")))
            Die();
    }

    private void Die()
    {
        if (!isAlive) { return; }
        isAlive = false;
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, deathDrama);
        animator.SetTrigger("die");
        StartCoroutine(ProcessDeath());
    }

    IEnumerator ProcessDeath(){
        yield return new WaitForSeconds(respawnDelay);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    public void Freeze(){
        myRigidBody.velocity = new Vector2(0f, 0f);
        freezed = true;
    }

    public void Unfreeze(){
        freezed = false;
    }
}

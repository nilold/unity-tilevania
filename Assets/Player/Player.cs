using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    [SerializeField] float speed = 10f;
    private Rigidbody2D myRigidBody;

	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        Run();
        FlipSprite();
	}

    private void Run(){
        float xSpeed = CrossPlatformInputManager.GetAxis("Horizontal") * speed;
        myRigidBody.velocity = new Vector2(xSpeed, myRigidBody.velocity.y);
    }

    private void FlipSprite(){
        //if the player is moving horizontally
        bool playerIsMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x),1);
        }
    }
}

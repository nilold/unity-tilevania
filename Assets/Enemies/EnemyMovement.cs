using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;
    BoxCollider2D myCollider;

	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
        myRigidBody.velocity = new Vector2(moveSpeed, 0);
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground"){
            // flip
            moveSpeed *= -1f;
            transform.localScale = new Vector2(Mathf.Sign(moveSpeed), 1);
        }
    }

}

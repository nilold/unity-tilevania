using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int scoreValue = 15;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Vector3 cameraPos = Camera.main.transform.position;
        AudioSource.PlayClipAtPoint(coinPickUpSFX, cameraPos);
        Destroy(gameObject);
    }
}

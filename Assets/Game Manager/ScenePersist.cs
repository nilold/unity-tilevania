using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour {

    int startingSceneIndex;

    private void Awake()
    {
        int objectCount = FindObjectsOfType<ScenePersist>().Length;
        if(objectCount > 1){
            Destroy(gameObject);
        } else{
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start () {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame
	void Update () {
        int currentScenaIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentScenaIndex != startingSceneIndex)
            Destroy(gameObject);
	}
}

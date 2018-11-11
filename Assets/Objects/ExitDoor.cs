using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour {

    [SerializeField] float LevelLoadDelay = 1f;
    //[SerializeField] float LevelExitSlowMoFactor = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()){
            IEnumerator coroutine = LoadNexScene(collision.GetComponent<Player>());
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator LoadNexScene(Player player){

        //Time.timeScale = LevelExitSlowMoFactor;
        player.Freeze();
        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

        player.Unfreeze();
        //Time.timeScale = 1f;
    }

}

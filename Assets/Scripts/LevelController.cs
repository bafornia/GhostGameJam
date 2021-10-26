using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int index;
    public string levelName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //loading level with build index
            SceneManager.LoadScene(index);

            //loading level with scene name
            SceneManager.LoadScene(levelName);
        }
    }
}

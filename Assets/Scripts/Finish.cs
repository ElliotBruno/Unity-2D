
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish1 : MonoBehaviour
{

    private AudioSource finishSound;
    private bool levelCompleted;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        finishSound = GetComponent<AudioSource>();
        levelCompleted = false;
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && levelCompleted != true)
        {
            anim.SetTrigger("finish");
            finishSound.Play();
            levelCompleted = true;
            Invoke("CompleteLevel", 2f);
        }
    }
    void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}



















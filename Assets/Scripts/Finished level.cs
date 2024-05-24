using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Finishedlevel : MonoBehaviour

{
    public Animator anim;
    private AudioSource finishSound;
    private bool levelCompleted = false;

    [SerializeField] GameObject player;
    itemcollector itemcollector;


    public int enemyCount = 1;

    [SerializeField] private TextMeshProUGUI Enemytext;

    private float dirX;
    public Animator animator;
    private SpriteRenderer sprite;
    public int points = 8;

    [SerializeField] private TextMeshProUGUI Kiwitext;
  




void Start()
    {
        finishSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        itemcollector = player.GetComponent<itemcollector>();

    }



    void Update()
    {

        if (enemyCount == 0 && points == 0 && levelCompleted != true)
        {
            anim.SetTrigger("finished");
            finishSound.Play();
            levelCompleted = true;
            Invoke("CompleteLevel", 1f);
            Debug.Log("finish");

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (((collision.gameObject.CompareTag("Finish")) ))
        {
            anim.SetTrigger("finished");
            finishSound.Play();
            levelCompleted = true;
            Invoke("CompleteLevel", 1f);
            Debug.Log("finish");

        }
    }
    void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Kiwi"))
        {
            points -= 1;
            Destroy(collision.gameObject);
            Kiwitext.text = "Fruits remaining: " + points;
        }


    }
    void Die()
    {
        player.GetComponent<Finishedlevel>().enemyCount--;

        animator.SetBool("IsDead", true);
        Debug.Log("Died");
        GetComponent<Collider2D>().enabled = false;

        enemyCount -= 1;
        Enemytext.text = "Enemeis remaining: " + enemyCount;


    }


}


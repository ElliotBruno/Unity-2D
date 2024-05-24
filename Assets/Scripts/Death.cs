using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Death : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float healthAmount = 100;
    // Start is called before the first frame update

    [SerializeField] private AudioSource deathSoundEffect;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
      
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Spikes")) || (collision.gameObject.CompareTag("Flie")) || collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }

    }
    private void Die()
    {
        if (healthAmount<=0)
        {
            anim.SetTrigger("death");

            deathSoundEffect.Play();
            rb.bodyType = RigidbodyType2D.Static;
        }

    }
    private void RestartLive()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
     

   

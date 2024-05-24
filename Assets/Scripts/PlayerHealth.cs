using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthbar;
    public Animator animator;

    private float healthAmount = 120f;
    public Slider healthSlider;
    private Animator anim;
    private Rigidbody2D rb;

    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jummping, hurt }
    private MovementState state = MovementState.idle;

    void Start()
    {
        healthSlider.maxValue = healthAmount;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthSlider.value = healthAmount;
        UpdateHealthBar();

    }

   

    public void TakeDamage(float damage)
    {
       
        healthAmount -= damage;

        StartCoroutine(UpdateHealthBar());


        if (healthAmount <= 0)
        {
            Die();
        }
    }

    IEnumerator UpdateHealthBar()
    {
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, healthAmount, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healthSlider.value = healthAmount;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (((collision.gameObject.CompareTag("Flie"))))
        {
            animator.SetTrigger("Hit");
       

            TakeDamage(55f);

        }
        else if (collision.gameObject.CompareTag("Spikes"))
        {
       
            animator.SetTrigger("Hit");



            TakeDamage(70f);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Hit");

    


            TakeDamage(30f);
        }
        else if (collision.gameObject.CompareTag("Lava"))
        {
            animator.SetTrigger("Hit");




            TakeDamage(200f);
        }
        else if (collision.gameObject.CompareTag("Fire"))
        {
            animator.SetTrigger("Hit");




            TakeDamage(200f);
        }
        else if (collision.gameObject.CompareTag("Propeller"))
        {
            animator.SetTrigger("Hit");




            TakeDamage(20f);
        }

        else
        {
        }
    }

    private void Die()
    {
      
        animator.SetBool("death", true);

                rb.bodyType = RigidbodyType2D.Static;
                
       
    }
}



















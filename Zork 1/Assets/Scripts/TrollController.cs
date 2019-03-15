using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Constraints;
using UnityEngine;

public class TrollController : MonoBehaviour
{
     public float moveSpeed;

     private Rigidbody2D trollRB2D;

     private bool isMoving;

     public float timeBetweenMove;

     private float timeBetweenMoveCounter;

     public float timeToMove;

     private float timeToMoveCounter;

     private Vector3 moveDirection;

     public Animator myTrollAnim;

     public float trollHealth;

     public bool trollDeath;

    // Start is called before the first frame update
    void Start()
    {
         int trollKills = PlayerPrefs.GetInt("troll_deaths", 15);
         if (trollKills == 0)
         {
              trollRB2D = GetComponent<Rigidbody2D>();
              trollDeath = false;
              trollHealth = 100;
              //timeBetweenMoveCounter = timeToMove;
              //timeToMoveCounter = timeToMove;

              timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.25f, timeBetweenMove * 1.75f);
              timeToMoveCounter = Random.Range(timeToMove * .025f, timeToMove * 1.75f);
              myTrollAnim.SetBool("trollDeath", false);
         }
         else
         {
              trollDeath = true;
              trollHealth = -100;
              myTrollAnim.SetBool("trollDeath", true);
         }

     }

    // Update is called once per frame
    void Update()
    {
         if (isMoving)
         {
              timeToMoveCounter -= Time.deltaTime;
              trollRB2D.velocity = moveDirection;
              myTrollAnim.SetFloat("moveX", trollRB2D.velocity.x);
              myTrollAnim.SetFloat("moveY", trollRB2D.velocity.y);
              myTrollAnim.SetFloat("lastMoveX", trollRB2D.velocity.x);
              myTrollAnim.SetFloat("lastMoveY", trollRB2D.velocity.y);
              if (trollHealth <= 0f)
              {
                   isMoving = false;
                   myTrollAnim.SetBool("trollDeath", true);
              }
               else if (timeToMoveCounter < 0f)
              {
                   isMoving = false;
                   timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.25f, timeBetweenMove * 1.75f);
                   
               }
         }
         else
         {
              timeBetweenMoveCounter -= Time.deltaTime;
              trollRB2D.velocity = Vector2.zero;
              myTrollAnim.SetFloat("moveX", trollRB2D.velocity.x);
              myTrollAnim.SetFloat("moveY", trollRB2D.velocity.y);
              if (trollHealth <= 0f)
              {
                   isMoving = false;
                   myTrollAnim.SetBool("trollDeath", true);
               }
              else if (timeBetweenMoveCounter < 0f && trollDeath == false)
              {
                   isMoving = true;
                   timeToMoveCounter = Random.Range(timeToMove * .025f, timeToMove * 1.75f);

                    moveDirection = new Vector3(Random.Range(-1f,1f) * moveSpeed, Random.Range(-1f,1f) * moveSpeed,
                        0f);
                   myTrollAnim.SetFloat("moveX", trollRB2D.velocity.x);
                   myTrollAnim.SetFloat("moveY", trollRB2D.velocity.y);
               }
         }
    }

     public void setHealth(int damage)
     {
          trollHealth -= damage;
          if (trollHealth <= 0f)
          {
               trollDeath = true;
               trollHealth = -500;
               isMoving = false;
               myTrollAnim.SetBool("trollDeath", true);
          }
     }
}

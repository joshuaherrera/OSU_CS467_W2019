using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Constraints;
using UnityEngine;

public class BatController : MonoBehaviour
{
     public float moveSpeed;

     private Rigidbody2D batRB2D;

     private bool isMoving;

     public float timeBetweenMove;

     private float timeBetweenMoveCounter;

     public float timeToMove;

     private float timeToMoveCounter;

     private Vector3 moveDirection;
     private Vector3 reversePath;

     public Animator myBatAnim;

     public float batHealth;

     public bool batDeath;

     public Transform wallCheck;
     public float wallCheckRadius;
     public LayerMask whatIsWall;
     public bool hittingWall;

     public Transform playerCheck;
     public float playerCheckRadius;
     public LayerMask whatIsPlayer;
     public bool hittingPlayer;

     // Start is called before the first frame update
     void Start()
     {
          int batKills = PlayerPrefs.GetInt("bat_deaths", 15);
          if (batKills == 0)
          {
               batRB2D = GetComponent<Rigidbody2D>();
               batDeath = false;
               batHealth = 100;
               //timeBetweenMoveCounter = timeToMove;
               //timeToMoveCounter = timeToMove;

               timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.25f, timeBetweenMove * 1.75f);
               timeToMoveCounter = Random.Range(timeToMove * .025f, timeToMove * 1.75f);
               myBatAnim.SetBool("batDeath", false);
          }
          else
          {
               batDeath = true;
               batHealth = -100;
               myBatAnim.SetBool("batDeath", true);
          }

     }

     // Update is called once per frame
     void Update()
     {

          hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
  
          if (hittingWall)
          {
               moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed,
                    0f);
               batRB2D.velocity = moveDirection;
               isMoving = true;
               timeToMoveCounter = Random.Range(timeToMove * .025f, timeToMove * 1.75f);
          }
          if (isMoving)
          {
               timeToMoveCounter -= Time.deltaTime;
               batRB2D.velocity = moveDirection;
               myBatAnim.SetFloat("moveX", batRB2D.velocity.x);
               myBatAnim.SetFloat("moveY", batRB2D.velocity.y);
               myBatAnim.SetFloat("LastMoveX", batRB2D.velocity.x);
               myBatAnim.SetFloat("LastMoveY", batRB2D.velocity.y);
               if (batHealth <= 0f)
               {
                    isMoving = false;
                    myBatAnim.SetBool("batDeath", true);
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
               batRB2D.velocity = Vector2.zero;
               myBatAnim.SetFloat("moveX", batRB2D.velocity.x);
               myBatAnim.SetFloat("moveY", batRB2D.velocity.y);
               if (batHealth <= 0f)
               {
                    isMoving = false;
                    myBatAnim.SetBool("batDeath", true);
               }
               else if (timeBetweenMoveCounter < 0f && batDeath == false)
               {
                    isMoving = true;
                    timeToMoveCounter = Random.Range(timeToMove * .025f, timeToMove * 1.75f);

                    moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed,
                        0f);
                    myBatAnim.SetFloat("moveX", batRB2D.velocity.x);
                    myBatAnim.SetFloat("moveY", batRB2D.velocity.y);
               }
          }
     }

     public void setHealth(int damage)
     {
          batHealth -= damage;
          if (batHealth <= 0f)
          {
               batDeath = true;
               batHealth = -500;
               isMoving = false;
               myBatAnim.SetBool("batDeath", true);
          }
     }
}

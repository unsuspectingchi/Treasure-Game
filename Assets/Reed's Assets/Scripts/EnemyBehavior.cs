using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
  public float attackPercent = 0.2f; // Chance of attack per second
  public float blockPercent = 0.5f; // Chance of block if player attacks
  public float playerHitDamage = 15f; // The amount of damage the player's hit inflicts
  public float health = 100.0f; // The health of the enemy
  public bool isDead = false; // Self explanatory-- is set to true after death
  public Sensor sensor; // The sensor script
  public HitBehavior hitBehavior; // The hit behavior script

  private Vector3 playerDir;

  private Collider player;
  private Animator anim;
  private bool isBlocking;

  void Start()
  {
    anim = GetComponent<Animator>();
  }

  private void FixedUpdate()
  {

    if (player)
    {
      playerDir = (transform.position - player.transform.position).normalized;
      transform.rotation = Quaternion.Euler(new Vector3(0, Quaternion.LookRotation(-playerDir).eulerAngles.y, 0));
    }

    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
    {
      isBlocking = false;
      if (sensor.isAttackable)
      {
        if (true && Random.Range(0f, 1f) < attackPercent / 50) // REPLACE TRUE WITH PLAYER IS IDLE BOOL
        {
          anim.SetTrigger("punch");
        }
        else if (true && Random.Range(0f, 1f) < blockPercent) // REPLACE TRUE WITH PLAYER IS ATTACKING BOOL
        {
          isBlocking = true;
          anim.SetTrigger("block");
        }
      }
    }
  }

  void Update()
  {
    if (sensor.isSeen && !sensor.isAttackable)
    {
      anim.SetBool("isWalking", true);
      player = sensor.detectedObject;
    }
    else
    {
      anim.SetBool("isWalking", false);
    }

    if (hitBehavior.isHit && !isBlocking && true) // REPLACE TRUE WITH PLAYER IS ATTACKING
    {
      hitBehavior.isHit = false;
      if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
      {
        anim.SetBool("isWalking", false);
      }
      anim.SetTrigger("hit");
      health -= playerHitDamage;
      if (health < 0)
      {
        anim.SetBool("isDead", true);
        isDead = true;
      }
    }
  }
}

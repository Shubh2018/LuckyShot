using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float enemySpeed = 0.25f;
    [SerializeField]
    float increasedEnemySpeed = 1.5f;
    Vector3 dir;
    public bool increaseSpeed = false;
    CharacterController2D player;
    [SerializeField]
    GameObject particle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        if (player == null)
            return;
        dir = Vector3.zero - this.transform.position;
    }

    private void Update()
    {
        float speed = increaseSpeed ? increasedEnemySpeed : enemySpeed;

        this.transform.Translate(dir * speed * Time.deltaTime);

        if (transform.position.x > 10 || transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y > 6 || transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Dice"))
        {
            player.RandDie = player.RollDice();
            Destroy(this.gameObject);
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}

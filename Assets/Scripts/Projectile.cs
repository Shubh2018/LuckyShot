﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float bulletSpeed = 10.0f;
    [SerializeField]
    GameObject particle;
    [SerializeField]
    CharacterController2D player;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController2D>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);

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
        if (collision.transform.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            Instantiate(particle, this.transform.position, Quaternion.identity);
            player.Score += 5;
            
        }    

        if(collision.transform.CompareTag("Dice"))
        {
            Destroy(this.gameObject);
            GameManager.Instance.RandDie = GameManager.Instance.RollDice();
            Instantiate(particle, this.transform.position, Quaternion.identity);
        } 
    }
}

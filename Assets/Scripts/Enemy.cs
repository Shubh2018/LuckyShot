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
    [SerializeField]
    GameObject spawnPos;
    float nextTimeToFire = 0.0f;
    [SerializeField]
    float fireRate = 1.0f;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    AudioClip shootClip;
    AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();

        if (player == null)
            return;

        audioSource = GetComponent<AudioSource>();

        if (audioSource)
            audioSource.clip = shootClip;

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

        if (GameManager.Instance.CanEnemyFire)
            EnemyFire();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Dice"))
        {
            GameManager.Instance.RandDie = GameManager.Instance.RollDice();
            Destroy(this.gameObject);
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }

    void EnemyFire()
    {
        Vector3 dir = Vector3.zero - spawnPos.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        spawnPos.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (nextTimeToFire < Time.time)
        {
            nextTimeToFire = Time.time + fireRate;
            Instantiate(projectile, spawnPos.transform.position, Quaternion.Euler(0, 0, angle));
            audioSource.Play();
        }
    }
}

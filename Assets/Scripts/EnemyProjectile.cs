using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    float bulletSpeed = 10.0f;
    [SerializeField]
    GameObject particle;
    [SerializeField]
    Dice dice;

    private void Start()
    {
        dice = GameObject.Find("Dice").GetComponent<Dice>();
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
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Instantiate(particle, this.transform.position, Quaternion.identity);
        }

        if (collision.transform.CompareTag("Dice"))
        {
            Destroy(this.gameObject);
            GameManager.Instance.RandDie = GameManager.Instance.RollDice();
            Instantiate(particle, this.transform.position, Quaternion.identity);
            dice.Health -= 5;
        }
    }
}

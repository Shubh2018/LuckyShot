using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    int health = 100;
    [SerializeField]
    Transform healthSprite;
    [SerializeField]
    Sprite[] diceFaces;
    SpriteRenderer sprite;
    [SerializeField]
    AudioClip powerupClip;
    AudioSource audioSource;

    public int Health
    {
        set
        {
            health = value;
        }

        get
        {
            return health;
        }
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        if(sprite)
            sprite.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);

        if (audioSource)
            audioSource.clip = powerupClip;
    }

    private void Update()
    {
        HealthBar(health);

        sprite.sprite = diceFaces[GameManager.Instance.RandDie];
        

        if(health <= 0)
        {
            Debug.Log("Dead");
            health = 0;
            GameManager.Instance.PlayerDeath();
        }
    }

    public void HealthBar(int health)
    {
        float barScale = (float)health / 100;
        healthSprite.localScale = new Vector3(barScale, healthSprite.transform.localScale.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            health -= 5;
            HealthBar(health);
            audioSource.Play();
        }
    }
}

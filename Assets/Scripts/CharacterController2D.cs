using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField]
    float playerSpeed = 10.0f;
    [SerializeField]
    GameObject[] spawnPos;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    GameObject particle;
    [SerializeField]
    float fireRate = 0.25f;
    float nextTimeToFire = 0.0f;
    [SerializeField]
    float timeToRoll = 5.0f;
    [SerializeField]
    float currentTime = 0.0f;
    [SerializeField]
    int score = 0;
    bool decreasePlayerSpeed = false;
    Dice dice;
    [SerializeField]
    AudioClip shootClip;
    AudioSource audioSource;

    public int Score
    {
        set
        {
            score = value;
        }

        get
        {
            return score;
        }
    }
   

    private void Start()
    {
        dice = GameObject.Find("Dice").GetComponent<Dice>();
        audioSource = GetComponent<AudioSource>();

        if (!dice)
            return;

        if (audioSource)
            audioSource.clip = shootClip;
    }

    private void Update()
    {
        float speed = decreasePlayerSpeed ? playerSpeed / 4 : playerSpeed;
        float hMov = Input.GetAxis("Horizontal") * speed;
        float vMov = Input.GetAxis("Vertical") * speed;

        Vector2 moveDirection = new Vector2(hMov, vMov);

        transform.Translate(moveDirection * Time.deltaTime);

        if(transform.position.x > 9 || transform.position.x < -9)
        {
            float xPos = Mathf.Clamp(transform.position.x, -9f, 9f);
            transform.position = new Vector3(xPos, transform.position.y);
        }

        if (transform.position.y > 5 || transform.position.y < -5)
        {
            float yPos = Mathf.Clamp(transform.position.y, -5f, 5f);
            transform.position = new Vector3(transform.position.x, yPos);
        }

        

        switch (GameManager.Instance.RandDie)
        {
            case 0:
                SingleShot();
                GameManager.Instance.CanEnemyFire = true;
                UIManager.Instance.DisplayText("Enemy Fire");
                break;
            case 1:
                FourShots();
                GameManager.Instance.CanEnemyFire = false;
                UIManager.Instance.DisplayText("Four Direction Shot");
                decreasePlayerSpeed = false;
                GameManager.Instance.increaseSpawnSpeed = false;
                break;
            case 2:
                GameManager.Instance.increaseSpawnSpeed = true;
                GameManager.Instance.CanEnemyFire = false;
                UIManager.Instance.DisplayText("More Enemies");
                decreasePlayerSpeed = false;
                SingleShot();
                break;
            case 3:
                RapidShot();
                GameManager.Instance.increaseSpawnSpeed = false;
                GameManager.Instance.CanEnemyFire = false;
                UIManager.Instance.DisplayText("Rapid Shot");
                decreasePlayerSpeed = false;
                break;
            case 4:
                decreasePlayerSpeed = true;
                UIManager.Instance.DisplayText("Decreased Speed");
                GameManager.Instance.CanEnemyFire = false;
                GameManager.Instance.increaseSpawnSpeed = false;
                SingleShot();
                break;
            case 5:
                EightShots();
                UIManager.Instance.DisplayText("Eight Direction Shot");
                GameManager.Instance.CanEnemyFire = false;
                decreasePlayerSpeed = false;
                break;
        }

        UIManager.Instance.DisplayScore(score);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            GameObject p = Instantiate(particle, this.transform.position, Quaternion.identity);
            Destroy(p, 2.5f);
            score += 5;
        }
    }  

    #region Single Shot
    private void SingleShot()
    { 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - spawnPos[0].transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        spawnPos[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButtonDown(0) && nextTimeToFire < Time.time)
        {
            nextTimeToFire = Time.time + fireRate;
            Instantiate(projectile, spawnPos[0].transform.position, Quaternion.Euler(0, 0, angle));
            audioSource.Play();
        }
    }
    #endregion

    #region Four Shots
    private void FourShots()
    {
        if (Input.GetMouseButtonDown(0) && nextTimeToFire < Time.time)
        {
            nextTimeToFire = Time.time + fireRate;

            for(int i = 1; i < 5; i++)
            {
                Instantiate(projectile, spawnPos[i].transform.position, spawnPos[i].transform.rotation);
                audioSource.Play();
            } 
        }
    }
    #endregion

    #region Rapid Shot
    private void RapidShot()
    {
        for (int i = 1; i < spawnPos.Length; i++)
            spawnPos[i].SetActive(false);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - spawnPos[0].transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        spawnPos[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButton(0))
        {
            Instantiate(projectile, spawnPos[0].transform.position, Quaternion.Euler(0, 0, angle));
            audioSource.Play();
        }
    }
    #endregion

    #region Eight Shots
    private void EightShots()
    {
        if (Input.GetMouseButtonDown(0) && nextTimeToFire < Time.time)
        {
            nextTimeToFire = Time.time + fireRate;

            for (int i = 1; i < 9; i++)
            {
                Instantiate(projectile, spawnPos[i].transform.position, spawnPos[i].transform.rotation);
                audioSource.Play();
            }
        }
    }
    #endregion  
}

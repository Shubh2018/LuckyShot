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
    private int randDie;
    bool decreasePlayerSpeed = false;
    Dice dice;
    
    public int RandDie
    { 
        set
        {
            randDie = value;
        }

        get
        {
            return randDie;
        }
    }

    private void Start()
    {
        randDie = RollDice();

        if (randDie == 0)
            randDie = RollDice();

        dice = GameObject.Find("Dice").GetComponent<Dice>();

        if (!dice)
            return;
    }

    private void Update()
    {
        float speed = decreasePlayerSpeed ? playerSpeed / 2 : playerSpeed;
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

        

        switch (randDie)
        {
            case 0:
                dice.Health -= 10;
                SingleShot();
                Debug.Log(0);
                break;
            case 1:
                FourShots();
                decreasePlayerSpeed = false;
                GameManager.Instance.increaseSpawnSpeed = false;
                break;
            case 2:
                GameManager.Instance.increaseSpawnSpeed = true;
                decreasePlayerSpeed = false;
                SingleShot();
                break;
            case 3:
                RapidShot();
                GameManager.Instance.increaseSpawnSpeed = false;
                decreasePlayerSpeed = false;
                break;
            case 4:
                decreasePlayerSpeed = true;
                GameManager.Instance.increaseSpawnSpeed = false;
                SingleShot();
                break;
            case 5:
                EightShots();
                decreasePlayerSpeed = false;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            GameObject p = Instantiate(particle, this.transform.position, Quaternion.identity);
            Destroy(p, 2.5f);
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
            }
        }
    }
    #endregion

    public int RollDice()
    {
        int ranInt = Random.Range(0, 6);
        return ranInt;
    }
}

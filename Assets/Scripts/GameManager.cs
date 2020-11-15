using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] spawnPos;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    Dice dice;
    [SerializeField]
    GameObject particle;
    public bool increaseSpawnSpeed = false;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        dice = GameObject.Find("Dice").GetComponent<Dice>();
        StartCoroutine(SpawnCoroutine());    
    }

    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            int randomSpawnPosition = Random.Range(0, 4);

            float spawnSpeed = increaseSpawnSpeed ? 0.1f : 0.25f;

            Vector3[] spawnLocation =
            {
                new Vector3(Random.Range(-8, 9), spawnPos[0].transform.position.y),
                new Vector3(Random.Range(-8, 9), spawnPos[1].transform.position.y),
                new Vector3(spawnPos[2].transform.position.x, Random.Range(-4, 5)),
                new Vector3(spawnPos[3].transform.position.x, Random.Range(-4, 5))
            };

            Instantiate(enemyPrefab, spawnLocation[randomSpawnPosition], Quaternion.identity);

            yield return new WaitForSeconds(spawnSpeed);
        }
    }

    public void PlayerDeath()
    {
        Destroy(dice.gameObject);
        Instantiate(particle, dice.transform.position, Quaternion.identity);
    }
}

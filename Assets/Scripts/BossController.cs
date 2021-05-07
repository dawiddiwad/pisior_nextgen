using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update
    public int initialHealth = 100;
    public GameObject dieVFX;
    public GameObject rocket;

    private int health;
    private GameObject player;

    private Timer timer;
    public int getHealth()
    {
        return health;
    }

    private int getRocketSpawnTimeMax()
    {
        return Random.Range(20 / GameController.getDifficultyFactor(), 20);
    }

    private int getRocketSpawnTimeMin()
    {
        return Random.Range(5 / GameController.getDifficultyFactor(), 5);
    }

    IEnumerator SpawnRocket(int from, int to)
    {
        yield return new WaitForSeconds(Random.Range(from, to));
        if (player == null && GameController.playerInstance == null)
        {
            yield break;
        } else
        {
            player = GameController.playerInstance;
        }

        Vector2 directionToPlayer = player.transform.position - transform.position;
        transform.up = -directionToPlayer;

        GameObject rocket = Pool.Instance.Get(Pool.GameObjectType.rocket);
        rocket.transform.position = transform.position;
        _ = StartCoroutine(SpawnRocket(getRocketSpawnTimeMin(), getRocketSpawnTimeMax()));
    }

    public int changeHealthBy(int healthDelta)
    {
        this.health += healthDelta;
        return getHealth();
    }
    public void setHealth(int healthValue)
    {
        health = healthValue;
    }
    void OnEnable()
    {
        health = initialHealth;
        _ = StartCoroutine(SpawnRocket(getRocketSpawnTimeMin(), getRocketSpawnTimeMax()));
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameObject vfx = Pool.Instance.Get(Pool.GameObjectType.vfxEnemyExplosion);
            vfx.transform.position = transform.position;
            Pool.Instance.Return(gameObject);
        }
    }
}

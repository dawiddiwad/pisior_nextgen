using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<Pool.GameObjectType> enemiesTypeList = new List<Pool.GameObjectType>()
    {
        Pool.GameObjectType.enemy1,
        Pool.GameObjectType.enemy2,
        Pool.GameObjectType.enemy3,
        Pool.GameObjectType.enemy4,
        Pool.GameObjectType.enemy5
    };
    public Pool.GameObjectType hitTextType = Pool.GameObjectType.hitText;
    public GameObject player;

    public static GameObject canvas;
    public static GameObject stats;
    public static GameObject playerInstance;
    public static PlayerController playerController;

    public delegate void GameScoreChanged(int newScore);
    public delegate void GamePaused();
    public delegate void GameResumed();
    public delegate void GameEnded(GameResult gameResult);

    public static event GameScoreChanged onScoreChange;
    public static event GamePaused onGamePuase;
    public static event GameResumed onGameResume;
    public static event GameEnded OnGameEnd;

    public int initialEnemiesAmount = 5;
    private static int difficultyFactor = 2;

    public static bool gameEnded = false;
    public static bool gamePaused = true;
    private static int score = 0;

    public enum GameResult
    {
        Won,
        Lost
    }

    public enum Difficulty
    {
        Pussy = 1,
        VeryHard = 2,
        Jedi = 3
    }

    public static int getScore()
    {
        return score;
    }

    public static void SetDifficulty(Difficulty level)
    {
        difficultyFactor = (int)level;
    }

    public static int getDifficultyFactor()
    {
        return difficultyFactor;
    }

    public static GameObject GetObjectCanvas(GameObject source)
    {
        foreach (Transform transform in source.GetComponentsInChildren<Transform>())
        {
            if (transform.GetComponents<Canvas>().Length > 0)
            {
                return transform.gameObject;
            }
        }
        throw new MissingComponentException($"Unable to find Canvas component in {source.name}");
    }

    private int calculateScoreFromDelta(int delta)
    {
        score += delta;
        return score;
    }
    private void ProcessEnemyHitsPlayer(GameObject enemy, Vector2 relativeVelocity,
        Vector3 collisionPosition)
    {
        GameObject vfx = Pool.Instance.Get(Pool.GameObjectType.vfxCollision);
        vfx.transform.position = collisionPosition;

        int hitScore = calculateEnemyHitScore(relativeVelocity.magnitude);
        onScoreChange(calculateScoreFromDelta(hitScore));

        GameObject hitText = Pool.Instance.Get(hitTextType);
        hitText.GetComponent<HitTextController>().Setup(hitScore, collisionPosition, HitTextController.UNIT.SCORE);
        
        _ = enemy.gameObject.GetComponent<BossController>()
            .changeHealthBy(calculateEnemyHealthDelta(relativeVelocity.magnitude));
    }

    private void ProcessRocketHitsPlayer(Vector3 collisionPosition)
    {
        if (gameEnded)
        {
            return;
        }
        GameObject hitText = Pool.Instance.Get(hitTextType);
        hitText.GetComponent<HitTextController>().Setup(-20, collisionPosition, HitTextController.UNIT.HP);

        int score = calculateEnemyHitScore(playerController.GetComponent<Rigidbody2D>().velocity.magnitude) * 10;
        hitText = Pool.Instance.Get(hitTextType);
        hitText.GetComponent<HitTextController>().Setup(score, collisionPosition + new Vector3(0, 0.7f, 0), HitTextController.UNIT.SCORE);

        onScoreChange(calculateScoreFromDelta(score));
    }

    private int calculateEnemyHitScore(float magnitude)
    {
        int score = Mathf.RoundToInt(Mathf.RoundToInt(magnitude) * 100
            * (float)playerController.intialHealth
            / (float)playerController.getHealth())
            ;
        return score > 0 ? score : 100;
    }

    private int calculateEnemyHealthDelta(float magnitude)
    {
        return -Mathf.RoundToInt(magnitude * 2 / difficultyFactor);
    }

    public static IEnumerator ReloadCurrentScene(int timeout)
    {
        if (Time.timeScale != 1 && timeout != 0)
        {
            Debug.LogWarning("Time scale is not equal to 1");
        }
        yield return new WaitForSeconds(timeout);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        _ = GamePauseToggle(forceUnpause: true);
    }
    private void spawnOnAsync(GameObject area, int amount, List<Pool.GameObjectType> enemyList)
    {
        for (int i = 0; amount-- > 0; i++)
        {
            i = i > enemyList.Count - 1 ? 0 : i;

            int randomizeEnemySpawn = Random.Range(0, enemyList.Count);
            GameObject enemyInstance = Pool.Instance.Get(enemyList[randomizeEnemySpawn]);

            Vector2 spawnAreaCenter = area.GetComponent<Renderer>().bounds.center;
            Vector2 spawnAreaBounds = area.GetComponent<Renderer>().bounds.extents;
            Vector2 enemyCollBounds = enemyInstance.GetComponent<Collider2D>().bounds.extents;

            float enemyCollDiagonal = Mathf.Sqrt(
                (enemyCollBounds.x * enemyCollBounds.x)
                + (enemyCollBounds.y * enemyCollBounds.y));

            enemyCollBounds = new Vector2(enemyCollDiagonal, enemyCollDiagonal);
            spawnAreaBounds -= enemyCollBounds;

            if (enemyCollBounds.x > spawnAreaBounds.x
                || enemyCollBounds.y > spawnAreaBounds.y)
            {
                Debug.LogWarning($"collision bounds for {enemyInstance} that have values {enemyCollBounds}" +
                    $" are to big to fit safely into spawn area renderer bounds {spawnAreaBounds}" +
                    $" - item was REMOVED from scene");
                Pool.Instance.Return(enemyInstance);
                continue;
            }

            enemyInstance.transform.position = new Vector2(
                spawnAreaCenter.x + Random.Range(-spawnAreaBounds.x, spawnAreaBounds.x),
                spawnAreaCenter.y + Random.Range(-spawnAreaBounds.y, spawnAreaBounds.y));
        }
    }

    private bool playerWonBasedOn(bool condition)
    {
        if (condition && OnGameEnd != null)
        {
            OnGameEnd(GameResult.Won);
        }
        return condition;
    }

    private bool playerLostBasedOn(bool condition)
    {
        if (condition && OnGameEnd != null)
        {
            OnGameEnd(GameResult.Lost);
        }
        return condition;
    }

    private bool playerIsGone()
    {
        GameObject[] playersOnScene = GameObject.FindGameObjectsWithTag("Player");
        return playersOnScene.Length == 0;
    }

    private bool allNpcAreGone()
    {
        GameObject[] enemiesOnScene = GameObject.FindGameObjectsWithTag("NPC");
        int enemiesCount = enemiesOnScene.Length;
        while (enemiesCount-- > 0)
        {
            if (enemiesOnScene[enemiesCount].GetComponent<BossController>().getHealth() > 0)
            {
                return false;
            }
        }
        return true;
    }

    void Start()
    {
        score = 0;
        gameEnded = false;
    }

    async void OnEnable()
    {
        PlayerController.OnHitEnemyBodyVFX += ProcessEnemyHitsPlayer;
        PlayerController.OnHitEnemyRocket += ProcessRocketHitsPlayer;
        canvas = GameObject.Find("Canvas");
        stats = GameObject.Find("Stats");
        playerInstance = GameObject.FindGameObjectWithTag(player.gameObject.tag);
        playerController = playerInstance.gameObject.GetComponent<PlayerController>();
        while (!Pool.poolReady) await Task.Delay(10);
        spawnOnAsync(GameObject.Find("SpawnArea"), initialEnemiesAmount * difficultyFactor, enemiesTypeList);
        gamePaused = false;
    }

    private void OnDisable()
    {
        PlayerController.OnHitEnemyBodyVFX -= ProcessEnemyHitsPlayer;
        PlayerController.OnHitEnemyRocket -= ProcessRocketHitsPlayer;
    }

    void Update()
    {
        ProcessKeyCommands();
        if (gameEnded || gamePaused)
        {
            return;
        }
        else if (playerWonBasedOn(allNpcAreGone()) || playerLostBasedOn(playerIsGone()))
        {
            gameEnded = true;
            _ = StartCoroutine(ReloadCurrentScene(3));
        }
    }

    private static void ProcessKeyCommands()
    {
        if (Input.GetButtonDown("Pause"))
        {
            _ = GamePauseToggle(forceUnpause: false);
        }
    }

    public static bool GamePauseToggle(bool forceUnpause)
    {
        if (gamePaused || forceUnpause)
        {
            Time.timeScale = 1;
            onGameResume();
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            onGamePuase();
            gamePaused = true;
        }
        return gamePaused;
    }

    public static void GamePauseToggle()
    {
        _ = GamePauseToggle(forceUnpause: false);
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Inside editor - unable to exit game");
        return;
#elif UNITY_WEBGL
        Application.ExternalEval("window.location.href = 'http://www.pisior.space/'");
        return;
#else
        Application.Quit();
#endif
    }
}

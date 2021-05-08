using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static bool poolReady { get; private set; } = false;

    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public GameObject enemy4Prefab;
    public GameObject enemy5Prefab;
    public GameObject rocketPrefab;
    public GameObject pickupHealthPrefab;
    public GameObject vfxEnemyExplosionPrefab;
    public GameObject vfxRocketExplosionPrefab;
    public GameObject vfxCollisionPrefab;
    public GameObject vfxCollisionHealthPickupPrefab;
    public GameObject hitTextPrefab;

    private Dictionary<GameObjectType,  Stack<GameObject>>  stacks =        new Dictionary<GameObjectType,  Stack<GameObject>>();
    private Dictionary<int,             Stack<GameObject>>  stackTrace =    new Dictionary<int,             Stack<GameObject>>();
    private Dictionary<GameObjectType,  GameObject>         typeMap;
    private Dictionary<GameObjectType,  int>                poolSizeMap;

    public enum GameObjectType : int { 
        enemy1, 
        enemy2,
        enemy3,
        enemy4,
        enemy5,
        rocket,
        pickupHealth,
        vfxEnemyExplosion,
        vfxRocketExplosion,
        vfxCollisionHealthPickup,
        vfxCollision,
        hitText
    };
    public static Pool Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        typeMap = new Dictionary<GameObjectType, GameObject>
        {
            {GameObjectType.enemy1,                     enemy1Prefab},
            {GameObjectType.enemy2,                     enemy2Prefab},
            {GameObjectType.enemy3,                     enemy3Prefab},
            {GameObjectType.enemy4,                     enemy4Prefab},
            {GameObjectType.enemy5,                     enemy5Prefab},
            {GameObjectType.rocket,                     rocketPrefab},
            {GameObjectType.hitText,                    hitTextPrefab},
            {GameObjectType.pickupHealth,               pickupHealthPrefab},
            {GameObjectType.vfxEnemyExplosion,          vfxEnemyExplosionPrefab},
            {GameObjectType.vfxRocketExplosion,         vfxRocketExplosionPrefab},
            {GameObjectType.vfxCollisionHealthPickup,   vfxCollisionHealthPickupPrefab},
            {GameObjectType.vfxCollision,               vfxCollisionPrefab}
        };
        poolSizeMap = new Dictionary<GameObjectType, int>
        {
            {GameObjectType.enemy1,                     10},
            {GameObjectType.enemy2,                     10},
            {GameObjectType.enemy3,                     10},
            {GameObjectType.enemy4,                     10},
            {GameObjectType.enemy5,                     10},
            {GameObjectType.rocket,                     10},
            {GameObjectType.hitText,                    10},
            {GameObjectType.pickupHealth,               10},
            {GameObjectType.vfxEnemyExplosion,          10},
            {GameObjectType.vfxRocketExplosion,         10},
            {GameObjectType.vfxCollisionHealthPickup,   10},
            {GameObjectType.vfxCollision,               30}
        };
        poolReady = InitializePools();
    }

    private void CreateStacks()
    {
        if (stacks.Count > 0)
        {
            stacks.Clear();
        }

        foreach (GameObjectType type in (GameObjectType[]) Enum.GetValues(typeof(GameObjectType)))
        {
            stacks.Add(type, new Stack<GameObject>());
        }
    }
    private void InitializePool(Stack<GameObject> stack, GameObject prefab, int size)
    {
        while (size-- > 0)
        {
            GameObject gameObject = Instantiate(prefab);
            PushToStack(stack, gameObject);
        }
    }

    private bool InitializePools()
    {
        {
            CreateStacks();
            foreach (KeyValuePair<GameObjectType, Stack<GameObject>> stack in stacks)
            {
                GameObject gameObject;
                int stackSize;
                if (typeMap.TryGetValue(stack.Key, out gameObject) && poolSizeMap.TryGetValue(stack.Key, out stackSize))
                {
                    InitializePool(stack: stack.Value, prefab: gameObject, size: stackSize);
                }
                else
                {
                    Debug.LogWarning($"unable to setup pool initialization parameters for {stack.Key}");
                }
            }
        }
        return true;

        ////CreateStacks();
        ////return true;
    }

    private GameObject GetFromStack(Stack<GameObject> stack, GameObjectType type)
    {
        GameObject gameObject;
        if (stack.Count > 0)
        {
            gameObject = stack.Pop();
            gameObject.SetActive(true);
            return gameObject;
        }
        else
        {
            int stackSize;
            if (typeMap.TryGetValue(type, out gameObject) && poolSizeMap.TryGetValue(type, out stackSize))
            {
                InitializePool(stack: stack, prefab: gameObject, size: stackSize > 1 ? stackSize / 2 : 1);
            }
            else
            {
                Debug.LogWarning($"unable to setup pool initialization parameters for {type}");
            }
            return GetFromStack(stack, type);
        }

        ////GameObject gameObject;
        ////typeMap.TryGetValue(type, out gameObject);
        ////return GameObject.Instantiate(gameObject);
    }

    private void PushToStack(Stack<GameObject> stack, GameObject gameObject)
    {
        gameObject.SetActive(false);
        stack.Push(gameObject);

        int instanceId = gameObject.GetInstanceID();
        if (!stackTrace.ContainsKey(instanceId))
        {
            stackTrace.Add(instanceId, stack);
        }
    }

    public void Return(GameObject gameObject)
    {
        int instanceId = gameObject.GetInstanceID();
        Stack<GameObject> pool;
        if (stackTrace.TryGetValue(instanceId, out pool))
        {
            PushToStack(pool, gameObject);
        }
        else
        {
            throw new KeyNotFoundException($"unable to get Stack for instanceId {instanceId}");
        }

        ////GameObject.Destroy(gameObject);
    }
    public GameObject Get(GameObjectType type)
    {
        Stack<GameObject> pool;
        if (stacks.TryGetValue(type, out pool))
        {
            return GetFromStack(pool, type);
        } else
        {
            throw new KeyNotFoundException($"unable to get Stack for {type}");
        }
    }
}

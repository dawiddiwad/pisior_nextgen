using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forceFactorOnMove = 10;
    public float forceFactorOnRotate = 10;
    public float maxLinearVelocity = 50;
    public float maxAngularVelocity = 1000;
    public float shotImpulse = 500;
    public int intialHealth = 100;

    public GameObject instantTest;

    public delegate void PlayerHitEnemyBodyVFX(GameObject enemy, Vector2 relativeVelocity,
        Vector3 collisionPosition);
    public delegate void PlayerHitEnemyRocket(Vector3 collisionPosition);
    public delegate void PlayerHealthChanged(int newHealthValue);

    public static event PlayerHitEnemyBodyVFX OnHitEnemyBodyVFX;
    public static event PlayerHitEnemyRocket OnHitEnemyRocket;
    public static event PlayerHealthChanged OnHealthChange;

    private bool shootSignal = false;
    private Vector2 controllerForce;
    private float controllerRotate;
    private Vector3 singleTapPoint;
    private Vector3 singleClickPoint;
    private Vector2 singleTouchPoint;
    private bool mouseClicked;
    private int health = 100;
    private float originalDrag;

    Rigidbody2D player;

    public int reduceHealthBy(int points)
    {
        this.health -= points;
        if (getHealthPrcnt() > 1)
        {
            this.health = intialHealth;
        }
        return getHealth();
    }

    public int increaseHealthBy(int points)
    {
        return reduceHealthBy(-points);
    }

    public int getHealth()
    {
        return this.health;
    }

    public decimal getHealthPrcnt()
    {
        return (decimal)health / (decimal)intialHealth;
    }

    private void processTouchInputs()
    {
        if (Input.touchCount > 0)
        {
            singleTapPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                singleTouchPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
        }
    }

    private void processMouseInputs()
    {
        if (Input.mousePresent && Input.GetMouseButtonDown(0))
        {
            singleClickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseClicked = true;
        }
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            GameObject.Find("PlayerHealth").GetComponent<PlayerHealthDisplayController>().setHealth(0);
            GameObject vfx = Pool.Instance.Get(Pool.GameObjectType.vfxRocketExplosion);
            vfx.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    void setupVariables()
    {
        player = gameObject.GetComponent<Rigidbody2D>();
        health = intialHealth;
        OnHealthChange(getHealth());
        originalDrag = player.drag;
    }

    void processControls()
    {
        if (Input.touchCount > 0)
        {
            Vector2 dir = singleTapPoint - transform.position;
            transform.up = -dir;
            mouseClicked = false;
            shootSignal = true;
        }
        else if (mouseClicked)
        {
            Vector2 dir = singleClickPoint - transform.position;
            transform.up = -dir;
            mouseClicked = false;
            shootSignal = true;
        }
        else
        {
            player.AddForce(controllerForce, ForceMode2D.Force);
            player.AddTorque(getPlayerTorque(controllerRotate) * forceFactorOnRotate * 3,
                ForceMode2D.Force);
            player.AddTorque(player.GetVector(player.velocity).x * forceFactorOnRotate / 3,
                ForceMode2D.Force);

            if (shootSignal)
            {
                player.AddRelativeForce(new Vector2(0f, -shotImpulse));
                shootSignal = false;
            }
        }
    }
    void handleMovementControls()
    {
        controllerForce.x = player.velocity.x < maxLinearVelocity ?
            Input.GetAxis("Horizontal") * forceFactorOnMove / 10 : 0f;
        controllerForce.y = player.velocity.y < maxLinearVelocity ?
            Input.GetAxis("Vertical") * forceFactorOnMove / 10 : 0f;
        controllerRotate = Input.GetAxis("Rotate");
    }

    void handleWeaponsControls()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            shootSignal = true;
        }
    }
    float getPlayerTorque(float rotateCommand)
    {
        rotateCommand *= -1;
        float playerAngularVelocity = player.angularVelocity;
        player.centerOfMass = new Vector2(0, 0.6f * Mathf.Abs(playerAngularVelocity / maxAngularVelocity));

        if (playerAngularVelocity < maxAngularVelocity && playerAngularVelocity > -maxAngularVelocity)
        {
            return rotateCommand;
        }
        else if (playerAngularVelocity > maxAngularVelocity && rotateCommand < 0)
        {
            return rotateCommand;
        }
        else if (playerAngularVelocity < -maxAngularVelocity && rotateCommand > 0)
        {
            return rotateCommand;
        }
        else
        {
            return 0f;
        }
    }

    void Start()
    {
        setupVariables();
    }

    void FixedUpdate()
    {
        if (!GameController.gamePaused)
        {
            processControls();
        }
    }
    void Update()
    {
        if (!GameController.gamePaused)
        {
            handleMovementControls();
            handleWeaponsControls();
            processTouchInputs();
            processMouseInputs();
            checkHealth();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.gameObject;
        string targetTag = target.tag;

        if (target.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
        {
            if (targetTag == "NPC")
            {
                OnHitEnemyBodyVFX?.Invoke(collision.gameObject, collision.relativeVelocity,
                    collision.transform.position);
            }
            else if (targetTag == "Rocket")
            {
                OnHitEnemyRocket?.Invoke(collision.transform.position);
                if (!GameController.gameEnded)
                {
                    OnHealthChange?.Invoke(reduceHealthBy(20));
                }
            }
            else if (targetTag == "pickup_health")
            {
                if (!GameController.gameEnded && !(getHealthPrcnt() == 1))
                {
                    OnHealthChange?.Invoke(increaseHealthBy(20));
                }
            }
        }
    }
}

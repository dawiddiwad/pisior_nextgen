using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketController : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject pickupHealth;

    private BoxCollider2D collider2Ds;
    private GameObject player;
    private Vector3 targetPosition;

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        collider2Ds.enabled = true;
    }

    void SlerpTowards(Quaternion target)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 100);
    }

    void rotateTowards(GameObject target, bool isInstant)
    {
        if (target != null)
        {
            targetPosition =
                targetPosition == null ? new Vector3(0, 0, 0) : target.transform.position;
        }
        Vector3 vectorToTarget = targetPosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.normalized.x, vectorToTarget.normalized.y) * Mathf.Rad2Deg;
        Quaternion targetQuaternion = Quaternion.AngleAxis(angle, Vector3.back);

        if (isInstant)
        {
            transform.rotation = targetQuaternion;
        }
        else
        {
            SlerpTowards(targetQuaternion);
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameController.playerInstance;
        collider2Ds = GetComponent<BoxCollider2D>();
        collider2Ds.enabled = false;
        _ = StartCoroutine(EnableCollider());
        rotateTowards(player, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotateTowards(player, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == tag && GetInstanceID() > collision.gameObject.GetInstanceID())
        {
            GameObject pickupHealth = Pool.Instance.Get(Pool.GameObjectType.pickupHealth);
            pickupHealth.transform.position = collision.transform.position;
            pickupHealth.transform.rotation = transform.rotation;
        }
        else
        {
            //_ = Instantiate(explosionVFX, transform.position, transform.rotation);
            GameObject explosionVFX = Pool.Instance.Get(Pool.GameObjectType.vfxRocketExplosion);
            explosionVFX.transform.position = transform.position;
            explosionVFX.transform.rotation = transform.rotation;
        }
        Pool.Instance.Return(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private Rigidbody Rb
    {
        get
        {
            return rb ?? (rb = gameObject.GetComponent<Rigidbody>());
        }
    }

    public float impulseForce = 10f;
    private Vector3 startPos;

    public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSupperSpeedActive;

    void Awake()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision)
            return;
        if (isSupperSpeedActive)
        {
            if (!collision.transform.GetComponent<LvlFinish>())
            {
                Destroy(collision.transform.parent.gameObject, 0.3f);
            }
        }
        else
        {
            DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
            if (deathPart)
                deathPart.HitDeathPart();
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke("AllowCollision", 0.2f);

        perfectPass = 0;
        isSupperSpeedActive = false;

    }

    private void Update()
    {
        if (perfectPass >= 2 && !isSupperSpeedActive)
        {
            isSupperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }
}

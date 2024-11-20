using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MannequinScript : MonoBehaviour
{
    public Vector3 target;

    [SerializeField] LayerMask obstacleMask;

    public GameObject player;

    Collider2D collider;

    Rigidbody2D rigidBody;

    private AIPather pather;

    public float walk = 4f;

    private float time;
    public float increment = 1 / 3f;
    public bool RemembersPlayer => time <= 1;

    public int currentIndex = 0;
    public float disabledTimer = 20;

    Vector2 facingDirection;

    public float pushRay = 0.5f;
    public float pushRayDistance = 1;

    private bool autoTarget = true;

    public bool active = true;

    public AudioSource audio;

    void Start()
    {
        target = player.transform.position;
        time = 3f;
        increment = 1 / 60f;

        pather = FindObjectOfType<AIPather>();

        collider = this.GetComponent<CapsuleCollider2D>();
        rigidBody = this.GetComponent<Rigidbody2D>();
        audio = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (active)
        {
            if (autoTarget)
               SetTarget(player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target, walk * Time.deltaTime);
            time = 0;
            if (pather != null)
            {
                Vector2 dir = pather.FindPath(transform.position, target, 4);
                Vector2 sideDir = new Vector2(-dir.y, dir.x);
                Vector2 walkDir = dir;
                for (int d = -1; d <= 1; d += 2)
                {
                    if (Physics2D.Raycast(transform.position, dir + sideDir * pushRay * d, pushRayDistance, pather.GetObstacleMask()).collider != null)
                    {
                        walkDir -= sideDir * pushRay * d;
                    }
                }
                walkDir.Normalize();
                transform.position += (Vector3)walkDir * walk * Time.deltaTime;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target, walk * Time.deltaTime);

            }
        }
        else
        {
            if (disabledTimer > 0)
            {
                disabledTimer -= Time.deltaTime;
                audio.Stop();
            }
            else
            {
                active = true;
                audio.Play();
                disabledTimer = 20;
            }
        }

        facingDirection = (target - transform.position).normalized;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FOV"))
        {
            active = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ForceTarget(Vector3 t)
    {
        SetTarget(t);
        autoTarget = false;
    }

    public void EnableAutoTarget() { autoTarget = true; }

    void SetTarget(Vector3 t)
    {
        target = t;
    }
}

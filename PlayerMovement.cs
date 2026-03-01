using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce;
    private Rigidbody2D rb;
    private Vector2 movement;
    public bool canJump;

    public bool atStore = false;
    public GameObject panelStoreUI;
    public Gamemanager manager;

    public string DIRT_TAG;
    public string DIRT2_TAG;
    public string GRASS_TAG;

    public float detectionRadius = 1.5f;


    
void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(panelStoreUI != null){
            if (atStore)
        {
            panelStoreUI.SetActive(true);
        }
        else
        {
            panelStoreUI.SetActive(false);
        }

        }
        
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jump();
        }
        if (Input.GetKey("d"))
        {
            move(Vector2.right);
        }
        if (Input.GetKey("s"))
        {
            move(Vector2.down);
        }
        if (Input.GetKey("a"))
        {
            move(Vector2.left);
        }
    }

    private void move(Vector2 dir)
    {
        rb.AddForce(dir * moveSpeed * Time.deltaTime);
    }

    void jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void EnableAllColliders(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(DIRT_TAG) || other.CompareTag(GRASS_TAG) || other.CompareTag(DIRT2_TAG))
        {
            EnableAllColliders(other.gameObject);
            canJump = true;
        }
        if (other.CompareTag("store"))
        {
            atStore = true;
        }
        if (other.CompareTag("roulet"))
        {
            float num = Random.Range(0, 1);
            if(num > .5)
            {
                manager.money *= 2;
            }
            else
            {
                manager.money = 0;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(DIRT_TAG) || collision.CompareTag(GRASS_TAG) || collision.CompareTag(DIRT2_TAG))
        {
            canJump = false;
        }
        if (collision.CompareTag("store"))
        {
            atStore = false;
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

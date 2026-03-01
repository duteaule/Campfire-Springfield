using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ore : MonoBehaviour
{
    public Gamemanager gamemanager;
    public int oreType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("colelct");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pcolelct");

            gamemanager = FindAnyObjectByType<Gamemanager>();
            gamemanager.gainResources(oreType);

            Destroy(gameObject);
        }


    }
}

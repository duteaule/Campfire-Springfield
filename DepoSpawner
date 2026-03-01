using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepoSpawner : MonoBehaviour
{
    public Vector2 minMaxX;
    public Vector2 maxMaxY;

    public Vector2 minMaxXGold;
    public Vector2 maxMaxYGold;


    public GameObject depoIron;
    public GameObject ironOre;

    public int ironOreQuantity;
    public int spawnQuantityIron;

    [Header("gold")]

    public GameObject depoGold;
    public GameObject GoldOre;

    public int goldOreQuantity;
    public int spawnQuantityGold;
    // Start is called before the first frame update
    private void Start()
    {
        for(int i = 0; i < spawnQuantityIron; i++)
        {
            Instantiate(depoIron, getRandomPose(), Quaternion.identity);
        }
        for (int i = 0; i < ironOreQuantity; i++)
        {
            Instantiate(ironOre, getRandomPose(), Quaternion.identity);
        }

        for (int i = 0; i < spawnQuantityGold; i++)
        {
            Instantiate(depoGold, getRandomPoseGold(), Quaternion.identity);
        }
        for (int i = 0; i < goldOreQuantity; i++)
        {
            Instantiate(GoldOre, getRandomPoseGold(), Quaternion.identity);
        }
    }

    Vector2 getRandomPose()
    {
        float x = Random.Range(minMaxX.x, minMaxX.y);
        float y = Random.Range(maxMaxY.x, maxMaxY.y);

        return new Vector2(x, y);
    }

    Vector2 getRandomPoseGold()
    {
        float x = Random.Range(minMaxXGold.x, minMaxXGold.y);
        float y = Random.Range(maxMaxYGold.x, maxMaxYGold.y);

        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
       
    }
}

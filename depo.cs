using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depo : MonoBehaviour
{
    public int resourceType;
    public int resourcesRemaining;

    // Start is called before the first frame update
    void Start()
    {
        // Randomize rotation on start
        float randomZ = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (!remaining())
        {
            Destroy(gameObject);
        }
    }

    public bool remaining()
    {
        if(resourcesRemaining > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DrainResource()
    {
        if (remaining())
        {
            resourcesRemaining--;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMan : MonoBehaviour
{

    public GameObject extracter;
    public Transform drill;
    public Drill dScript;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            placement(extracter);
        }
    }

    void placement(GameObject target)
    {
        if (dScript)
        {
            GameObject extract = Instantiate(target, dScript.returnDepo(), Quaternion.identity);
            extract.GetComponent<extractor>().sDepo = dScript.depoObj.GetComponent<depo>();
        }
        
    }
}

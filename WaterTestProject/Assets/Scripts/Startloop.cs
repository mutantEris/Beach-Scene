using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startloop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<CamMover>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<CamMover>().enabled == false)
        {
            this.GetComponent<CamMover>().enabled = true;
        }
        
    }
}

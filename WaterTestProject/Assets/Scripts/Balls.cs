using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    public GameObject Ball;
    public int WhichBall;
    public GameObject Proto;
    public PlayerMovement swummingo;

    private void OnTriggerEnter(Collider other)
    {
        if (WhichBall == 1)//bottom ball
        {
            swummingo.swimstate = 1;
        }
        else if (WhichBall == 2)//top ball
        {
            swummingo.swimstate = 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (WhichBall == 1)//bottom ball
        {
            swummingo.swimstate = 0;
        }
        else if (WhichBall == 2)//top ball
        {
            swummingo.swimstate = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        swummingo = Proto.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoccerGoal : MonoBehaviour
{
    ParticleSystem confetti;
    AudioSource chilgren;
    public GameObject ConfettiCannon;
    public bool goalScored;
    // Start is called before the first frame update
    void Start()
    {
        goalScored = false;
        confetti = ConfettiCannon.GetComponent<ParticleSystem>();
        chilgren = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //goal scored
            goalScored = true;
            Debug.Log("score!");
            confetti.Play();
            chilgren.Play();
        }
    }
}

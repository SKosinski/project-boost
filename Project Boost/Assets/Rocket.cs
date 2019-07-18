using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rb;
    AudioSource audioSource;
    LevelLoader levelLoader;

    [SerializeField] float thrustSpeed = 2;
    [SerializeField] float rotationSpeed = 2;

    enum State {  Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        levelLoader = FindObjectOfType<LevelLoader>();

    }

    // Update is called once per frame
    void Update()
    {
        if(state!=State.Dying)
        {
            Thrust();
            Rotate();
        }
    }

    private void Thrust()
    {

        float thrustThisFrame = thrustSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(new Vector3(0, thrustThisFrame, 0));
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }
    }

    private void Rotate()
    {

        rb.freezeRotation = true;

        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 0, -rotationThisFrame));
        }

        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 0, rotationThisFrame));
        }

        rb.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "End":
                state = State.Transcending;
                levelLoader.LoadNextLevel();
                break;
            default:
                Die();
                break;
        }
    }

    private void Die()
    {
        state = State.Dying;
        levelLoader.ReloadLevel();
        rb.freezeRotation = false;
    }
}

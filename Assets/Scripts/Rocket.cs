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

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem[] particles;

    [SerializeField] bool isInvincible = false;

    enum State {  Alive, Dying, Transcending, Invincible }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        levelLoader = FindObjectOfType<LevelLoader>();
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Dying)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            levelLoader.ApplyLoadNextLevel();
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (state != State.Invincible)
            {
                state = State.Invincible;
                isInvincible = true;
            }
            else if (state == State.Invincible)
            {
                state = State.Alive;
                isInvincible = false;
            }
        }
    }

    private void RespondToThrustInput()
    {
        float thrustThisFrame = thrustSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
                particles[0].Stop();
            }
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rb.AddRelativeForce(new Vector3(0, thrustThisFrame, 0));
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        particles[0].Play();
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            rb.freezeRotation = true;
            transform.Rotate(new Vector3(0, 0, -rotationThisFrame));
            rb.freezeRotation = false;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            rb.freezeRotation = true;
            transform.Rotate(new Vector3(0, 0, rotationThisFrame));
            rb.freezeRotation = false;
        }
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
            case "Success":
                Success();
                break;
            default:
                Die();
                break;
        }
    }

    private void Success()
    {
        state = State.Transcending;
        //audioSource.Stop();
        audioSource.PlayOneShot(success);
        particles[2].Play();
        levelLoader.LoadNextLevel();
    }

    private void Die()
    {
        state = State.Dying;
        if (particles[0].isPlaying)
        {
            particles[0].Stop();
        }
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        particles[1].Play();
        levelLoader.ReloadLevel();
        rb.freezeRotation = false;
    }
}

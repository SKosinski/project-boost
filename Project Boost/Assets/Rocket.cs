using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    public Rigidbody rb;
    public AudioSource audioSource;
    [SerializeField] float thrustSpeed = 2;
    [SerializeField] float rotationSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
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
        if (collision.gameObject.tag == "Friendly")
        {
            print("Safe");
        }
        else
        {
            print("Dead");
        }
    }
}

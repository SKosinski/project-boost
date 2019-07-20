using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    float cycles = 0;

    //todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; //0 not moved, 1 fully moved

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period != 0)
        {
            cycles = Time.time / period; //grows from 0
        }

        const float tau = Mathf.PI * 2; //6.28
        float rawSinWave = Mathf.Sin(cycles * tau); //between -1 and 1 

        movementFactor = rawSinWave / 2f + 0.5f; //between 0 and 1

        Vector3 offset = movementVector * movementFactor;

        transform.position = startingPos + offset;
    }
}

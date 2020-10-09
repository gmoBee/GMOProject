using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliding : MonoBehaviour
{
    Rigidbody rig;
    CapsuleCollider colider;

    float originalHeight;
    float reduceHeight;
    public float slideSpeed = 7f;
    bool isSliding;

    // Start is called before the first frame update
    void Start()
    {
        colider = GetComponent<CapsuleCollider>();
        rig = GetComponent<Rigidbody>();
        originalHeight = colider.height; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
        {
            Sliding();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StandUp();
        }

    }

    private void Sliding()
    {
        colider.height = reduceHeight;
        rig.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
    }

    private void StandUp()
    {
        colider.height = originalHeight;
    }
}


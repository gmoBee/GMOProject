using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    // setting variables for movement code. Default speed is 5, but i can be changed in unity 
    [SerializeField]
    private CharacterController controller = null;
    public float _speed = 5f;
    public float jumpSpeed = 5f;
    private float movement = 0f;

    private Rigidbody rigidBody;
    public Transform groundCheck;
    public float GroundDistance =0.5f;
    public LayerMask ground;
    bool TouchGround;
    //private Animator animation;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
       // animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchGround = Physics.CheckSphere(groundCheck.position, GroundDistance, ground);
        movement = Input.GetAxis("Horizontal");
        if (movement > 0f)
        {
            rigidBody.velocity = new Vector3(movement * _speed, rigidBody.velocity.y);
        }
        else if (movement < 0f)
        {
            rigidBody.velocity = new Vector3(movement * _speed, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y);
        }
        if (Input.GetButtonDown("Jump") && TouchGround)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpSpeed);
        }
    }

    
}

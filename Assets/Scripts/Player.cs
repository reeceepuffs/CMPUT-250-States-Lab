using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : AnimatedEntity
{
    //Current Velocity
    private Vector3 velocity, acceleration, prevPosition;
    //Minimum Velocity
    private float minVelocity = 0.2f;
    //Facing direction
    private Vector3 direction;

    [Header("Movement Settings")]
    //Acceleration and max velocity
    public float accelerationDelta = 3.5f;
    public float maxAcceleration = 5f;
    public float maxVelocity = 5;
    public float inertia = 10f;

    [Header("Animation Settings")] 
    public List<Sprite> idle;
    public List<Sprite> runCycle; // Delete this

    // Start is called before the first frame update
    void Start()
    {
        AnimationSetup();
    }

    // Update is called once per frame
    void Update()
    {
        //Animation Handling?
        AnimationCycle = idle;

        //Flip the sprite according to movement direction
        if (direction == Vector3.left)
        {
            sr.flipX = true;
            AnimationCycle = runCycle;
        }
        else if (direction == Vector3.right)
        {
            sr.flipX = false;
            AnimationCycle = runCycle;
        }
        AnimationUpdate(); //Animate the character!
    }

    private void FixedUpdate()
    {
        bool buttonPressed = false;
        direction = Vector3.zero;
        //Going Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
            buttonPressed = true;
        }
        //Going Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector3.right;
            buttonPressed = true;
        }

        if (!buttonPressed)
        {
            //Decrease velocity if not actively pressed
            if (velocity.magnitude > 0)
            {
                velocity -= velocity.normalized * (inertia * Time.deltaTime);
            }

            if (velocity.magnitude < minVelocity)
            {
                velocity = Vector3.zero;
            }

            acceleration = Vector3.zero;
        }
        else
        {
            if ((direction.x > 0f && velocity.x < 0f) || (direction.x < 0f && velocity.x > 0f))
            {
                velocity.x = 0f;
                acceleration.x = 0f;
            }
            acceleration += direction * (accelerationDelta * Time.deltaTime);

            if (acceleration.magnitude > maxAcceleration)
            {
                acceleration.Normalize();
                acceleration *= maxAcceleration;
            }

            //Increase Velocity according to acceleration
            velocity += acceleration * Time.deltaTime;
        }

        //Ensure we stay within maxVelocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity.Normalize();
            velocity *= maxVelocity;
        }

        Vector3 newPosition = transform.position + Time.deltaTime * velocity;

        //Set position
        transform.position = newPosition;
    }

}


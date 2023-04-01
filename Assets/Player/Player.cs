using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    int Orientation = 1;

    [SerializeField]
    bool Grounded;

    [SerializeField]
    bool Walled;

    [SerializeField]
    Rigidbody playerRigidbody;

    [SerializeField]
    float shiftCooldown;

    [SerializeField]
    Transform targetDisplay;

    public float Speed;
    public float JumpPower;

    public float Gravity;

    Vector3 force;
    float ForceX;
    float ForceXActive;


    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RaycastHit hitground;
        if (Physics.SphereCast(transform.position, 0.75f, -transform.up * Orientation, out hitground, 0.4f))
        {
            Debug.Log("Ground found!");
            targetDisplay.position = hitground.point;
            Grounded = true;
        }
        else
        {
            
            else
            {
                RaycastHit hitwallback;
                if (Physics.SphereCast(transform.position, 0.2f, -transform.right, out hitwallback, 0.5f))
                {
                    Debug.Log("Wall found behind!");
                    targetDisplay.position = hitwallback.point;
                    Walled = true;
                }
                else
                {
                    Debug.Log("Airborn");
                    targetDisplay.position = new Vector3(0, 100, 0);
                    Grounded = false;
                    Walled = false;
                }
                
            }
                
        }

        if (Grounded)
        {
            ForceX = ForceXActive;
        }
        else
        {
            ForceX = ForceXActive*0.3f;
        }


        if (transform.position.y > 0)
        {
            force = new Vector3(ForceX, Orientation*Gravity, 0);
            playerRigidbody.AddForce(force);
        }
        else
        {
            force = new Vector3(ForceX, Orientation*Gravity, 0);
            playerRigidbody.AddForce(force);
        }

        if (timer < shiftCooldown)
        {
            timer += Time.deltaTime;
        }

        if (ForceXActive > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (ForceXActive < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }


    private void OnMove(InputValue input)
    {
        //Debug.Log("Move X: " + input.Get<Vector2>().x + "Move Y: " + input.Get<Vector2>().y);
        ForceXActive = input.Get<Vector2>().x * Speed;
    }


    private void OnJump(InputValue input)
    {
        //Debug.Log("Jump: " + input.Get<float>());

        RaycastHit hit;
        if (Grounded)
        {
            playerRigidbody.AddForce(new Vector3(0, JumpPower*Orientation,0));
        }
    }


    private void OnUse(InputValue input)
    {
        //Debug.Log("Use: " + input.Get<float>());
    }


    private void OnShift(InputValue input)
    {
        //Debug.Log("Shift: " + input.Get<float>());
        if (timer >= shiftCooldown)
        {
            if (Orientation == 1)
            {
                Orientation = -1;
                transform.position = new Vector3(transform.position.x, -Mathf.Abs(transform.position.y), transform.position.z);
                //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
            else
            {
                Orientation = 1;
                transform.position = new Vector3(transform.position.x, Mathf.Abs(transform.position.y), transform.position.z);
                //playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, -playerRigidbody.velocity.y, playerRigidbody.velocity.z);
            }
            
            //transform.localScale = new Vector3(transform.localScale.x, Orientation*transform.localScale.y, transform.localScale.z);
            timer = 0;
        }    
    }

}

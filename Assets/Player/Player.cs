using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool Alive;

    bool ExitAnim;

    [SerializeField]
    GameObject WarpSpritePrefab;


    [SerializeField]
    int Orientation = 1;

    [SerializeField]
    bool Grounded;

    [SerializeField]
    bool WalledRight;

    [SerializeField]
    bool WalledLeft;

    [SerializeField]
    bool Roofied;

    [SerializeField]
    Rigidbody playerRigidbody;

    [SerializeField]
    public Animator animator;

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
        ExitAnim = true;
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
            //Debug.Log("Ground found!");
            targetDisplay.position = hitground.point + new Vector3(0, 0, -5);
            Grounded = true;
            WalledRight = false;
            WalledLeft = false;
            Roofied = false;
        }
        else
        {
            RaycastHit hitroof;
            if (Physics.SphereCast(transform.position, 0.75f, transform.up * Orientation, out hitroof, 0.4f))
            {
                //Debug.Log("Roof found!");
                targetDisplay.position = hitroof.point + new Vector3(0, 0, -5);
                Roofied = true;
            }
            else
            {
                RaycastHit hitwallback;
                RaycastHit hitwallfront;
                if (Physics.SphereCast(transform.position, 0.2f, Vector3.right, out hitwallback, 0.5f))
                {
                    //Debug.Log("Wall found behind!");
                    targetDisplay.position = hitwallback.point+new Vector3(0,0,-5);
                    WalledRight = true;
                    Grounded = false;
                    WalledLeft = false;
                    Roofied = false;
                }
                else if (Physics.SphereCast(transform.position, 0.2f, -Vector3.right, out hitwallfront, 0.5f))
                {
                    //Debug.Log("Wall found in front!");
                    targetDisplay.position = hitwallfront.point + new Vector3(0, 0, -5);
                    WalledLeft = true;
                    Grounded = false;
                    WalledRight = false;
                    Roofied = false;
                }
                else
                {
                    //Debug.Log("Airborn");
                    targetDisplay.position = new Vector3(0, 100, 0);
                    Grounded = false;
                    WalledRight = false;
                    WalledLeft = false;
                    Roofied = false;
                }
                
            }
                
        }

        if (Grounded || Roofied)
        {
            ForceX = ForceXActive;
        }
        else
        {
            ForceX = ForceXActive*0.2f;
        }



        if (Roofied)
        {
            force = new Vector3(ForceX, -Orientation*Gravity, 0);
        }
        else
        {
            force = new Vector3(ForceX, Orientation * Gravity, 0);
        }
            
        playerRigidbody.AddForce(force);


        if (timer < shiftCooldown)
        {
            timer += Time.deltaTime;

            if (timer > 0.2f && ExitAnim)
            {
                GameObject newSprite = GameObject.Instantiate(WarpSpritePrefab);
                newSprite.transform.position = transform.position;
                ExitAnim = false;
            }
        }


        if (ForceXActive > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (ForceXActive < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        animator.SetFloat("YVel", playerRigidbody.velocity.y * Orientation);
        animator.SetBool("Grounded", Grounded);
        animator.SetBool("Roofied", Roofied);
        animator.SetBool("Walled", WalledLeft || WalledRight);
    }


    private void OnMove(InputValue input)
    {
        //Debug.Log("Move X: " + input.Get<Vector2>().x + "Move Y: " + input.Get<Vector2>().y);
        ForceXActive = input.Get<Vector2>().x * Speed;
    }


    private void OnJump(InputValue input)
    {
        //Debug.Log("Jump: " + input.Get<float>());
        if (Grounded)
        {
            playerRigidbody.AddForce(new Vector3(0, JumpPower*Orientation,0));
        }
        else if (WalledRight)
        {
            if (ForceXActive < 0)
            {
                playerRigidbody.AddForce(-Vector3.right * JumpPower + new Vector3(0, JumpPower * Orientation, 0));
            }
            else
            {
                playerRigidbody.AddForce(-Vector3.right * JumpPower*0.25f + new Vector3(0, JumpPower * Orientation, 0));
            }
        }
        else if (WalledLeft)
        {
            if (ForceXActive > 0)
            {
                playerRigidbody.AddForce(Vector3.right * JumpPower + new Vector3(0, JumpPower * Orientation, 0));
            }
            else
            {
                playerRigidbody.AddForce(Vector3.right * JumpPower * 0.2f + new Vector3(0, JumpPower * Orientation, 0));
            }
        }  
        else if (Roofied)
        {
            playerRigidbody.AddForce(new Vector3(0, -JumpPower * Orientation/2, 0));
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
            GameObject newSprite = GameObject.Instantiate(WarpSpritePrefab);
            newSprite.transform.position = transform.position;
            ExitAnim = true;
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

            transform.localScale = new Vector3(1, Orientation,1);
            timer = 0;
        }
    }

}

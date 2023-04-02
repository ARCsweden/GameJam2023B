using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float RespawnTime;

    public Color color;

    [SerializeField]
    float maxSpeed;

    public bool Alive;

    bool exploded;

    [SerializeField]
    GameObject SplatEffect;
    [SerializeField]
    GameObject SplatEffectBlood;

    public Material playerMat;

    [SerializeField]
    SpriteRenderer playerSprite;

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
    Transform mainCameraTransform;

    CheckSpawn spawnchecker;

    public bool IsInitialized { get; private set; }

    int score = 0;
    public PlayerScorePrefab playerScorePrefab;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = FindAnyObjectByType<Camera>().transform;
        color = Random.ColorHSV(0, 1, 0.5f, 1, 0.75f, 1, 1, 1);
        playerMat = playerSprite.material;

        spawnchecker = GameObject.FindAnyObjectByType<CheckSpawn>();
        Vector3 spawnpoint = spawnchecker.getSpawnPoint();

        if (spawnpoint != new Vector3(0, 0, 0))
        {
            SetupPlayer(spawnpoint.x, spawnpoint.y);
        }
        else
        {
            Alive = false;
            timer = RespawnTime / 3;
        }
        IsInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetupPlayer(float xPoint,float atPoint)
    {
        Alive = true;
        ExitAnim = true;
        exploded = false;
        transform.position = new Vector3(xPoint, atPoint, 0);
        Orientation = -1;
        transform.localScale = new Vector3(1, Orientation, 1);
        timer = shiftCooldown;
        playerMat.SetColor("_Color", color);
    }

    private void FixedUpdate()
    {
        if (Alive)
        {
            RaycastHit hitground;
            if (Physics.SphereCast(transform.position, 0.4f, -transform.up * Orientation, out hitground, 1))
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
                if (Physics.SphereCast(transform.position, 0.2f, transform.up * Orientation, out hitroof, 0.9f))
                {
                    //Debug.Log("Roof found!");
                    targetDisplay.position = hitroof.point + new Vector3(0, 0, -5);
                    Roofied = true;
                }
                else
                {
                    RaycastHit hitwallback;
                    RaycastHit hitwallfront;
                    if (Physics.SphereCast(transform.position, 0.4f, Vector3.right, out hitwallback, 0.3f))
                    {
                        //Debug.Log("Wall found behind!");
                        targetDisplay.position = hitwallback.point + new Vector3(0, 0, -5);
                        WalledRight = true;
                        Grounded = false;
                        WalledLeft = false;
                        Roofied = false;
                    }
                    else if (Physics.SphereCast(transform.position, 0.4f, -Vector3.right, out hitwallfront, 0.3f))
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
                ForceX = ForceXActive * 0.2f;
            }



            if (Roofied)
            {
                force = new Vector3(ForceX, -Orientation * Gravity, 0);
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
        else
        {
            if (!exploded)
            {
                GameObject DeathSplatEffect = GameObject.Instantiate(SplatEffect,FindAnyObjectByType<Camera>().transform);
                DeathSplatEffect.transform.position = transform.position + new Vector3(0, 1, 0);
                playerMat.SetColor("_Color", new Vector4(0,0,0,0));
                playerRigidbody.velocity = new Vector3(0,0,0);
                exploded = true;
                timer = 0;
            }

            timer += Time.deltaTime;
            if (timer > RespawnTime)
            {
                Vector3 SpawnPoint = GameObject.FindAnyObjectByType<CheckSpawn>().getSpawnPoint();
                if (SpawnPoint != new Vector3(0,0,0))
                {
                    SetupPlayer(SpawnPoint.x,SpawnPoint.y);
                }
                else
                {
                    timer = RespawnTime / 3;
                }
            }
        }

        if (playerRigidbody.velocity.magnitude > maxSpeed) 
        {
            playerRigidbody.velocity = new Vector3(Mathf.Min(playerRigidbody.velocity.x, maxSpeed), Mathf.Min(playerRigidbody.velocity.y, maxSpeed), 0);
        }
    }


    private void OnMove(InputValue input)
    {
        //Debug.Log("Move X: " + input.Get<Vector2>().x + "Move Y: " + input.Get<Vector2>().y);
        ForceXActive = input.Get<Vector2>().x * Speed;
    }


    private void OnJump(InputValue input)
    {
        if (Alive)
        {
            //Debug.Log("Jump: " + input.Get<float>());
            if (Grounded)
            {
                playerRigidbody.AddForce(new Vector3(0, JumpPower * Orientation, 0));
            }
            else if (WalledRight)
            {
                if (ForceXActive < 0)
                {
                    playerRigidbody.AddForce(-Vector3.right * JumpPower + new Vector3(0, JumpPower * Orientation, 0));
                }
                else
                {
                    playerRigidbody.AddForce(-Vector3.right * JumpPower * 0.25f + new Vector3(0, JumpPower * Orientation * 1.2f, 0));
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
                    playerRigidbody.AddForce(Vector3.right * JumpPower * 0.2f + new Vector3(0, JumpPower * Orientation * 1.2f, 0));
                }
            }
            else if (Roofied)
            {
                playerRigidbody.AddForce(new Vector3(0, -JumpPower * Orientation / 2, 0));
            }
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
            Vector3 checkposfrom = new Vector3(transform.position.x, -(transform.position.y-(0.35f*Orientation)), 0);
            Vector3 checkposto = checkposfrom - new Vector3(0, 0.8f, 0) * Orientation;
            Debug.Log("Player pos: " + transform.position + "  checkposfrom: " + checkposfrom + "  checkposto: " + checkposto);
            int layermask = ~LayerMask.GetMask("Human");

            if (Physics.OverlapCapsule(checkposfrom, checkposto, 0.5f, layermask).Length == 0)
            {
                Debug.DrawLine(checkposfrom+new Vector3(0,0,-10), checkposto + new Vector3(0, 0, -10), Color.green,3);
                Debug.Log("Found Shift position");
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

                transform.localScale = new Vector3(1, Orientation, 1);
                timer = 0;
            }
            else
            {
                Debug.Log("Can't shift into (" + Physics.OverlapCapsule(checkposfrom, checkposto, 0.5f).Length + "): " + Physics.OverlapCapsule(checkposfrom, checkposto, 0.5f)[0].name);
                Debug.DrawLine(checkposfrom + new Vector3(0, 0, -10), checkposto + new Vector3(0, 0, -10), Color.red, 3);
                GameObject newSprite = GameObject.Instantiate(WarpSpritePrefab);
                newSprite.transform.position = transform.position;
                ExitAnim = true;
                timer = 0;
            }
        }
    }

    private void OnRespawn(InputValue input)
    {
        Alive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<HumanPrefab>())
        {
            GameObject DeathSplatEffectBlood = GameObject.Instantiate(SplatEffectBlood, FindAnyObjectByType<Camera>().transform);
            DeathSplatEffectBlood.transform.position = collision.transform.position + new Vector3(0, 1, 0);
            Destroy(collision.collider.gameObject);
            playerRigidbody.AddForce(new Vector3(Speed*50,0,0));
            animator.SetTrigger("Chomp");
            // Increment score and update UI
            score++;
            string scoreString = score.ToString();
            playerScorePrefab.SetText(scoreString);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPrefab : MonoBehaviour
{
    public Animator animator;

    public Rigidbody rigidBody;

    public float Speed;

    bool Panic = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Panic)
        {
            // TODO: away from player
            rigidBody.velocity = new Vector3(Speed, rigidBody.velocity.y, 0);
        } else
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            animator.SetBool("Panic", true);
            Panic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            animator.SetBool("Panic", false);
            Panic = false;
        }
    }
}

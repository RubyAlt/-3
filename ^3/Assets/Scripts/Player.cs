using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject GameObject;
    public ParticleSystem PartDestroy;
    public BoxCollider BoxCollider;
    [Space(10)]
    public float x;
    public float y;
    [Space(10)]
    public float JumpForce = 10;
    public float Gravity = -9.81f;
    public float GravityScale = 1;
    float Velocity;
    public bool IsGrounded = false;
    public float DistToCheck = 0.5f;
    public float Speed = 2f;

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if(Physics.Raycast(transform.position, Vector3.down, DistToCheck))
            IsGrounded = true;
        else
            IsGrounded=false;

        if (!IsGrounded)
            Velocity += Gravity * GravityScale * Time.deltaTime;

        if (Input.GetAxis("Jump") > 0 && IsGrounded)
            Velocity = JumpForce;

        transform.Translate(new Vector3(0, Velocity, 0)*Time.deltaTime);

        if (Speed < 10 && x != 0)
            Speed *= 1.25f;
        else if (Speed > 2 && x == 0)
            Speed *= 0.5f;
        else if (Speed > 10)
            Speed = 10f;
        else if (Speed < 2)
            Speed = 2f;
    }

    private void FixedUpdate()
    {
        Debug.Log(Speed);
        rb.velocity = new Vector3(Speed*x, 0, 0);
        if (y < 0)
        {
            GameObject.transform.localScale = Vector3.Lerp(GameObject.transform.localScale, new Vector3(1.5f, 0.5f, 1), 0.1f);
            DistToCheck = 0.25f;
            GravityScale = 0.9f;
        }
        else if (y == 0)
        {
            GameObject.transform.localScale = Vector3.Lerp(GameObject.transform.localScale, new Vector3(1, 1, 1), 0.1f);
            DistToCheck = 0.5f;
            GravityScale = 1f;
        }
        else
        {
            GameObject.transform.localScale = Vector3.Lerp(GameObject.transform.localScale, new Vector3(0.5f, 1.5f, 1), 0.1f);
            DistToCheck = 0.75f;
            GravityScale = 1.1f;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Velocity = 0;
            Debug.Log("ntm");
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Wait(2));
        }
    }

    IEnumerator Wait(int i)
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        PartDestroy.Play();
        yield return new WaitForSeconds(i);
        Destroy(gameObject, 0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
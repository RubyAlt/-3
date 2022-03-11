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
    public int jump;

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        rb.AddForce(transform.up * Input.GetAxis("Jump") * 5000, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(5*x, -10, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            jump = 1;
        }
        if (col.gameObject.CompareTag("Platform"))
        {
            ContactPoint2D contact = col.contacts[0];
            if (Vector2.Dot(contact.normal, Vector2.up) > 0.5)
            {
                transform.parent = col.transform;
                jump = 1;
            }
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Wait(2));
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            jump = 0;
        }
        if (col.gameObject.CompareTag("Platform"))
        {
            transform.parent = null;
            jump = 0;
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
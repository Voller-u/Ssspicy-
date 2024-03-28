using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Body : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W)) transform.Translate((Vector3)Vector2.up);
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    rb.velocity = new Vector2(-1, 0);
        //}
        //if (Input.GetKeyDown(KeyCode.S)) transform.Translate((Vector3)Vector2.down);
        //if (Input.GetKeyDown(KeyCode.D)) transform.Translate((Vector3)Vector2.right);
        transform.position = new Vector3(transform.position.x - 0.01f, transform.position.y, 0);
    }
}

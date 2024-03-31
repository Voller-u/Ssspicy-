using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [Header("飞行状态")]
    public bool flying;
    public Vector3 flyDir;
    [Header("身体部位贴图")]
    public List<Sprite> sprites;
    [Header("身体转角贴图")]
    public List<Sprite> cornerSprites;
    [Header("尾巴贴图")]
    public List<Sprite> tailSprites;
    public LayerMask detectLayer;
    public LayerMask ground;
    public Rigidbody2D rb;
    public Collider2D coll;
    public bool isTail;
    [Header("灰尘预制体")]
    public GameObject dust;
    void Start()
    {
        Init();

    }

     void Init()
    {
        isTail = false;
        var sp = GetComponent<SpriteRenderer>().sprite;
        foreach(var sprite in tailSprites)
        {
            if(sp == sprite) isTail = true;
        }
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(flying)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, flyDir, 0.7f, detectLayer);
            if (!hit) return;
            if (hit.collider.tag.Equals("Stone"))
            {
                GameManager.instance.SetFlyEnd();
            }
        }
    }
    private enum TailDir { right,down,left,up}
    public void UpdateSprite(Vector3 left,Vector3 right)
    {
        
        if(isTail == true)
        {
            if (left.x < transform.position.x && left.y == transform.position.y)
                GetComponent<SpriteRenderer>().sprite = tailSprites[(int)TailDir.right];

            if (left.x > transform.position.x && left.y == transform.position.y)
                GetComponent<SpriteRenderer>().sprite = tailSprites[(int)TailDir.left];

            if (left.x == transform.position.x && left.y > transform.position.y)
                GetComponent<SpriteRenderer>().sprite = tailSprites[(int)TailDir.down];

            if (left.x == transform.position.x && left.y < transform.position.y)
                GetComponent<SpriteRenderer>().sprite = tailSprites[(int)TailDir.up];
        }
        else
        {
            Vector3 pos = transform.position;
            Vector3 vec = (left - pos) + (right - pos);
             
            //Debug.Log("vec:" + vec);
            if(vec.x > 0.5f && vec.y >0.5f)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[2];
            }
            if (vec.x > 0.5f && vec.y < 0.5f)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[0];
            }
            if (vec.x < 0.5f && vec.y > 0.5f)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[3];
            }
            if (vec.x < 0.5f && vec.y < 0.5f)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[1];
            }

            if ((left.x < pos.x && right.x > pos.x) || (left.x > pos.x && right.x < pos.x))
                GetComponent<SpriteRenderer>().sprite = sprites[0];
            if ((left.y < pos.y && right.y > pos.y) || (left.y > pos.y && right.y < pos.y))
                GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(flying)
        {
            if (other.collider.tag.Equals("Stone"))
            {
                GameManager.instance.SetFlyEnd();
            }
            else if (other.collider.tag.Equals("Collectable"))
            {
                GameManager.instance.flyingObjects.Add(other.gameObject);
                other.gameObject.GetComponent<Collectable>().flying = true;
                other.gameObject.GetComponent<Collectable>().flyDir = flyDir;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Collectable") && flying)
        {
            GameManager.instance.flyingObjects.Add(collision.gameObject);
            collision.gameObject.GetComponent<Collectable>().flying = true;
            collision.gameObject.GetComponent<Collectable>().flyDir = flyDir;
        }
    }

    public void GenerateDust(Vector3 ori)
    {
        Debug.Log("Dust:" + transform.localPosition);
        int num = Random.Range(2, 5);
        for(int i=0;i<num;i++)
        {
            GameObject d = Instantiate(dust);
            d.transform.position = ori +  new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),0);
        }
        
    }
}

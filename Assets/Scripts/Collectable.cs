using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool collectable;
    public LayerMask detectLayer;
    public bool flying;
    public bool falling;
    public Rigidbody2D rb;
    public float flySpeed;
    public float fallingSpeed; 
    public Vector3 flyDir;
    public LayerMask ground;
    private Collider2D coll;
    public GameObject shadow;

    private void Awake()
    {
        Debug.Log("collectable 生成了");
    }
    private void Start()
    {
        if(collectable)
            GameManager.instance.CollectableNum++;
        Debug.Log(this.name + "++");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Fly();
        Fall();
        CooperationRectify();
    }

    void CooperationRectify()
    {
        if (flying || falling) return;
        transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f,
           Mathf.Floor(transform.position.y) + 0.5f, transform.position.z);
    }

    /// <summary>
    /// 判断收集物是否可以推动，如果可以则推动，否则吃掉第一个收集物
    /// </summary>
    /// <param name="moveDir"></param>
    /// <param name="isOrigin"></param>
    /// <returns></returns>
    public bool CanMoveDir(Player player, Vector3 moveDir,bool isOrigin)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + 1f * moveDir, moveDir, 0.2f, detectLayer);
        //Debug.Log("hit");
        if (!hit)
        {
            if(collectable)
                transform.parent.transform.Translate(moveDir);
            else 
                transform.Translate(moveDir);
            return true;
        }
        else
        {
            Debug.Log("tag :" + hit.collider.tag);
            if(hit.collider.tag.Equals("Body") && hit.collider.GetComponent<Body>().isTail)
            {
                Debug.Log("打到尾巴，可以走");
                if (collectable)
                    transform.parent.transform.Translate(moveDir);
                else
                    transform.Translate(moveDir);
                return true;
            }
            if (hit.collider.GetComponent<Collectable>() != null)
            {
                if (hit.collider.GetComponent<Collectable>().CanMoveDir(player,moveDir, false))
                {
                    
                    if (collectable)
                        transform.parent.transform.Translate(moveDir);
                    else
                        transform.Translate(moveDir);
                    return true;
                }
                //这时候就应该被吃掉
                else if (isOrigin)
                {
                    if (collectable)
                    {
                        Eaten(player);
                        return true;
                    }
                    else return false;
                }
            }
            else if (isOrigin)
            {
                if(collectable)
                {
                    Eaten(player);
                    return true;
                }
                else return false;
            }
        }
        return false;
    }

    public virtual void Eaten(Player player)
    {
        GameManager.instance.CollectableNum--;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!flying)
            return;
        else
        {
            if (other.collider.tag.Equals("Stone"))
            {
                GameManager.instance.SetFlyEnd();
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

    void Fly()
    {
        if (flying)
        {
            Debug.Log("飞起来咯" + transform.position + flyDir * flySpeed);
            if(transform.parent.gameObject.layer == 7)
                transform.parent.position = new Vector3(transform.parent.position.x + flyDir.x * flySpeed,
                transform.parent.position.y + flySpeed * flyDir.y, 0f);
            else
                transform.position = new Vector3(transform.position.x + flyDir.x * flySpeed,
                transform.position.y + flySpeed * flyDir.y, 0f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + 0.5f * flyDir, flyDir, 0.2f, detectLayer);
            if (!hit) return;
            if(hit.collider.tag.Equals("Stone"))
            {
                Debug.Log("飞完力");
                GameManager.instance.SetFlyEnd();
            }
        }
    }
    void Fall()
    {
        if(flying) return; 
        if(!falling)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left, 0.001f, ground);
            if (!hit)
            {
                falling = true;
                coll.enabled = false;
                GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                GetComponent<SpriteRenderer>().sortingOrder = -10;
                if (collectable) 
                    shadow.SetActive(false);
            }
        }
        else
        {
            if (collectable)
            {
                Vector3 vector3 = transform.parent.transform.position;
                transform.parent.transform.position = new Vector3(vector3.x,
                    vector3.y - Time.deltaTime * fallingSpeed, 0f);
                GameManager.instance.collectableFall = true;
            }
            else
            {
                Vector3 vector3 = transform.position;
                transform.position = new Vector3(vector3.x,
                    vector3.y - Time.deltaTime * fallingSpeed, 0f);
            }
        }
    }
}

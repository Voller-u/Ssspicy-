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
        Debug.Log("collectable ������");
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
    /// �ж��ռ����Ƿ�����ƶ�������������ƶ�������Ե���һ���ռ���
    /// </summary>
    /// <param name="moveDir"></param>
    /// <param name="isOrigin"></param>
    /// <returns></returns>
    public bool CanMoveDir(Player player, Vector3 moveDir,bool isOrigin)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + 1f * moveDir, moveDir, 0.2f, detectLayer);
        Debug.Log("hit");
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
                //��ʱ���Ӧ�ñ��Ե�
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

    void Fly()
    {
        if (flying)
        {
            transform.position = new Vector3(transform.position.x + flyDir.x * flySpeed,
                transform.position.y + flySpeed * flyDir.y, 0f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + 0.5f * flyDir, flyDir, 0.2f, detectLayer);
            if (!hit) return;
            if(hit.collider.tag.Equals("Stone"))
            {
                GameManager.instance.SetFlyEnd();
            }
        }
    }
    void Fall()
    {
        if(flying) return; 
        if(!falling)
        {
            if(!coll.IsTouchingLayers(ground))
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

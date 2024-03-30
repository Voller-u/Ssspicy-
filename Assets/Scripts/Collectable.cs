using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public LayerMask detectLayer;
    public bool flying;
    public Rigidbody2D rb;
    public float flySpeed;
    public Vector3 flyDir;

    private void Start()
    {
        GameManager.instance.CollectableNum++;
        Debug.Log(this.name + "++");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Fly();
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
        Debug.Log("hit");
        if (!hit)
        {
            transform.Translate(moveDir);
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Collectable>() != null)
            {
                if (hit.collider.GetComponent<Collectable>().CanMoveDir(player,moveDir, false))
                {
                    transform.Translate(moveDir);
                    return true;
                }
                //这时候就应该被吃掉
                else if (isOrigin)
                {
                    Eaten(player);
                    return true;
                }
            }
            else if (isOrigin)
            {
                Eaten(player);
                return true;
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

}

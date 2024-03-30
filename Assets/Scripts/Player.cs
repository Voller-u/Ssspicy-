using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("飞行属性")]
    public bool flying;
    public Vector3 flyDir;
    public float flySpeed;
    [Header("身体部位")]
    public List<Body> bodies = new List<Body>();
    [Header("爬行头部贴图")]
    public List<Sprite> sprites;
    [Header("辣辣头部贴图")]
    public List<Sprite> spicySprites;
    [Header("坠落头部贴图")]
    public List<Sprite> fallingSprites;
    [Header("开心头部贴图")]
    public List<Sprite> happySprites;
    [Header("身体预制体")]
    public GameObject bodyPrefab;
    public GameObject fire;
    //头部朝向
    public Vector3 orient;
    private Rigidbody2D rb;
    public LayerMask detectLayer;
    Vector3 moveDir = Vector3.zero;

    [Header("状态位置")]
    public List<Vector3> status = new List<Vector3>();

    private void Awake()
    {
        if (bodies.Count > 0) bodies[^1].isTail = true;


    }
    // Start is called before the first frame update
    void Start()
    {
        InitSprite();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        Fly();
        UpdateTail();
        UpdateBody();
        CooperationRectify();   
    }

    void UpdateTail()
    {
        for(int i=0;i<bodies.Count;i++)
        {
            bodies[i].isTail = false;
            if (i == bodies.Count - 1) bodies[i].isTail = true;
        }
    }

    /// <summary>
    /// 初始化蛇的头部朝向
    /// </summary>
    void InitSprite()
    {
        int index = 0; 
        for(int i =0;i<sprites.Count;i++)
        {
            if(GetComponent<SpriteRenderer>().sprite == sprites[i])
            {
                index = i;
                break;
            }
        }
        switch(index)
        {
            case 0: orient = Vector3.right; break;
            case 1: orient = Vector3.down; break;
            case 2: orient = Vector3.left; break;
            case 3: orient = Vector3.up; break;
        }
    }
    /// <summary>
    /// 根据蛇的移动方向dir调整蛇头的贴图
    /// </summary>
    /// <param name="dir">移动方向</param>
     public void UpdateSprite(Vector3 dir)
    {
        if(flying) return;
        //if (dir == orient) 
        //    return;
        if(dir == Vector3.right)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        if (dir == Vector3.down)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        if (dir == Vector3.left)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        if (dir == Vector3.up)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[3];
        }

        orient = dir;

        
    }

    void UpdateBody()
    {
        //更新身体的贴图
        List<Vector3> ts = new List<Vector3>();
        ts.Add(transform.position);
        foreach (var b in bodies) ts.Add(b.transform.position);
        ts.Add(Vector3.zero);
        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].UpdateSprite(ts[i], ts[i + 2]);
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    string s = "";
        //    foreach (var v in ts)
        //    {
        //        s += v.ToString();
        //        s += "\n";
        //    }
        //    Debug.Log(s);
        //}
    }

    void InputKey()
    {
        Vector3 tempMoveDir = Vector3.zero;
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W");
            if ((Vector3.up + orient).x == 0 && (Vector3.up + orient).y == 0) return;
            tempMoveDir = Vector3.up;
        }

        else if(Input.GetKeyDown(KeyCode.S))
        {
            if ((Vector3.down + orient).x == 0 && (Vector3.down + orient).y == 0) return;
            tempMoveDir = Vector3.down;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            if ((Vector3.right + orient).x == 0 && (Vector3.right + orient).y == 0) return;
            tempMoveDir = Vector3.right;
        }

        else if( Input.GetKeyDown(KeyCode.A)) 
        {
            if ((Vector3.left + orient).x == 0 && (Vector3.left + orient).y == 0) return;
            tempMoveDir = Vector3.left;
        }
        
        //如果输入了方向键
        if(tempMoveDir != Vector3.zero && !flying)
        {
            Debug.Log(tempMoveDir);
            moveDir = tempMoveDir;
            if (CanMoveDir(tempMoveDir))
            {
                Debug.Log("CanMove");
                Move(tempMoveDir);
                
            }
        }
    }

    void CooperationRectify()
    {
        if (flying) return;
    
        transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f,
           Mathf.Floor(transform.position.y) + 0.5f, transform.position.z);

        foreach(var b in bodies)
        {
            b.transform.localPosition = new Vector3(Mathf.Round(b.transform.localPosition.x),
                Mathf.Round(b.transform.localPosition.y), b.transform.localPosition.z);
        }
    }

    void Fly()
    {
        if(flying)
        {
            transform.position = new Vector3(transform.position.x + flyDir.x*flySpeed,
                transform.position.y + flySpeed * flyDir.y, 0f);
            foreach(var b in bodies) b.flyDir = flyDir;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, flyDir, 0.7f, detectLayer);
            if (!hit) return;
            if (hit.collider.tag.Equals("Stone"))
            {
                GameManager.instance.SetFlyEnd();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!flying) return;
        if(other.collider.tag.Equals("Stone"))
        {
            GameManager.instance.SetFlyEnd();

        }
        else if(other.collider.tag.Equals("Collectable"))
        {
            GameManager.instance.flyingObjects.Add(other.gameObject);
            other.gameObject.GetComponent<Collectable>().flying = true;
            other.gameObject.GetComponent<Collectable>().flyDir = flyDir;
        }
    }

    private bool CanMoveDir(Vector3 moveDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 1f, detectLayer);

        if (!hit)
            return true;
        else
        {
            if (hit.collider.tag.Equals("Body") && hit.collider.GetComponent<Body>().isTail)
                return true;
            if (hit.collider.GetComponent<Collectable>() != null)
            {
                return hit.collider.GetComponent<Collectable>().CanMoveDir(this,moveDir,true);
            }
            //这时候就应该吃掉
            else return false;
        }
    }

    /// <summary>
    /// 向特定方向移动
    /// </summary>
    /// <param name="dir">方向向量</param>
    void Move(Vector3 dir)
    {
        //头部和身体各个部位的位置
        List<Vector3> poses = new List<Vector3>();
        poses.Add(transform.position);
        foreach(Body b in bodies)
        {
            poses.Add(b.transform.position);
        }
        transform.Translate(dir);
        for(int i=0;i<bodies.Count;i++) 
        {
            Vector3 targetPos = poses[i];
            bodies[i].transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        }
        if(status.Count > 0)
        {
            //吃到了香蕉
            EatBanana();
            status.Clear();
        }
        UpdateSprite(dir);
    }

    public void EatPepper()
    {
        flying = true;
        flyDir = -moveDir;
        GameManager.instance.flyingObjects.Add(this.gameObject);
        foreach (Body b in bodies)
        {
            b.flying = true;
            b.flyDir = flyDir;
            GameManager.instance.flyingObjects.Add(b.gameObject);
        }
        


        if (moveDir.x > 0.5f)
        {
            fire.transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("好辣 x->");
            GetComponent<SpriteRenderer>().sprite = spicySprites[0];
            Debug.Log("sprite:" + spicySprites[0]);
        }
        if (moveDir.x < -0.5f)
        {
            fire.transform.rotation = Quaternion.Euler(0, 0, 180);
            GetComponent<SpriteRenderer>().sprite = spicySprites[2];
        }
        if (moveDir.y > 0.5f)
        {
            fire.transform.rotation = Quaternion.Euler(0, 0, 90);
            GetComponent<SpriteRenderer>().sprite = spicySprites[3];
        }
        if (moveDir.y < -0.5f)
        {
            fire.transform.rotation = Quaternion.Euler(0, 0, 270);
            GetComponent<SpriteRenderer>().sprite = spicySprites[1];
        }
        fire.transform.localPosition = new Vector3(moveDir.x, moveDir.y, moveDir.z);
        Debug.Log("localPosition:" + fire.transform.localPosition);
        fire.SetActive(true);

         
    }

    public void EatBanana()
    {
        GameObject bd = Instantiate(bodyPrefab, transform);
        Vector3 pos = status[^1];
        bd.transform.position = new Vector3(pos.x, pos.y, pos.z);
        bodies.Add(bd.GetComponent<Body>());
    }

    public void RecordPos()
    {
        status.Add(transform.position);
        foreach (var body in bodies)
        {
            status.Add(body.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Hole") && collision.GetComponent<Hole>().on)
        {
            //胜利
            Debug.Log("Victory");
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            GetComponent<SpriteRenderer>().sortingOrder = -10;
            StartCoroutine(Victory());
        }
    }

    IEnumerator Victory()
    {
        int num = bodies.Count;
        for (int i = 0; i < num; i++)
        {
            Move(orient);
            bodies[i].GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            bodies[i].GetComponent<SpriteRenderer>().sortingOrder = -10;
            yield return new WaitForSeconds(0.15f);
            //TODO加载下一关
        }
        
    }
}

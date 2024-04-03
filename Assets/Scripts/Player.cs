using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("��������")]
    public bool flying;
    public Vector3 flyDir;
    public float flySpeed;
    [Header("��������")]
    public bool falling;
    public float fallingSpeed;
    [Header("���岿λ")]
    public List<Body> bodies = new List<Body>();
    [Header("����ͷ����ͼ")]
    public List<Sprite> sprites;
    [Header("����ͷ����ͼ")]
    public List<Sprite> spicySprites;
    [Header("׹��ͷ����ͼ")]
    public List<Sprite> fallingSprites;
    [Header("����ͷ����ͼ")]
    public List<Sprite> happySprites;
    [Header("����ͷ����ͼ")]
    public List<Sprite> crySprites;
    [Header("����Ԥ����")]
    public GameObject bodyPrefab;
    public GameObject fire;
    //ͷ������
    [Header("ͷ������")]
    public Vector3 orient;
    private Rigidbody2D rb;
    private Collider2D coll;
    [Header("���߼��Ĳ�")]
    public LayerMask detectLayer;
    public LayerMask moveLayer;
    public LayerMask ground;
    Vector3 moveDir = Vector3.zero;

    [Header("״̬λ��")]
    public List<Vector3> status = new List<Vector3>();

    [Header("չʾ����")]
    public bool onShow;
    public float showTime;

    [Header("��ʾ����")]
    public GameObject leftKey;
    public GameObject upKey;
    public GameObject rightKey;
    public GameObject downKey;
    [Header("��ʾʱ��")]
    public float tipTime;
    public float tipTimer;
    private void Awake()
    {
        GameManager.instance.gameOver = false;
        GameManager.instance.gamePass = false;
        if (bodies.Count > 0) bodies[^1].isTail = true;

        Debug.Log("player������");
    }
    // Start is called before the first frame update
    void Start()
    {
        
        InitSprite();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        Fly();
        UpdateTail();
        UpdateBody();
        CooperationRectify();
        FallJudge();
        UpdateTip();
        DetectGame();
    }

    //������ʾ��Ϣ
    void UpdateTip()
    {
        tipTimer += Time.deltaTime;
        if(tipTimer >= tipTime)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.left, 1f, moveLayer);
            if (!hit)
                leftKey.SetActive(true);
            hit = Physics2D.Raycast(transform.position, Vector3.right, 1f, moveLayer);
            if(!hit)
                rightKey.SetActive(true);
            hit = Physics2D.Raycast(transform.position, Vector3.up, 1f, moveLayer);
            if (!hit)
                upKey.SetActive(true);
            hit = Physics2D.Raycast(transform.position, Vector3.down, 1f, moveLayer);
            if (!hit)
                downKey.SetActive(true);
        }
        else
        {
            leftKey.SetActive(false);
            rightKey.SetActive(false);
            upKey.SetActive(false);
            downKey.SetActive(false);
        }
    }

    void DetectGame()
    {
        if(transform.position.x <-15 || transform.position.x > 15
            || transform.position.y <-10 || transform.position.y > 10)
        {
            GameManager.instance.gameOver = true;
            Debug.Log("�ߵ���ȥ��");
            onShow = true;
        }
        if(GameManager.instance.collectableFall)
        {
            StartCoroutine(CFall());
        }
    }

    IEnumerator CFall()
    {
        GameManager.instance.collectableFall = false;
        if (orient == Vector3.right)
        {
            GetComponent<SpriteRenderer>().sprite = crySprites[0];
        }
        if (orient == Vector3.down)
        {
            GetComponent<SpriteRenderer>().sprite = crySprites[1];
        }
        if (orient == Vector3.left)
        {
            GetComponent<SpriteRenderer>().sprite = crySprites[2];
        }
        if (orient == Vector3.up)
        {
            GetComponent<SpriteRenderer>().sprite = crySprites[3];
        }

        onShow = true;
        yield return new WaitForSeconds(1f);
        Debug.Log("�������ȥ��");
        GameManager.instance.gameOver = true;
        
    }

    void UpdateTail()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].isTail = false;
            if (i == bodies.Count - 1) bodies[i].isTail = true;
        }
    }

    void FallJudge()
    {
        if (falling)
        {
            Fall();
        }
        if (!flying)
        {
            bool fall = true;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.001f, ground);
            if(hit)
                fall = false;
            //if (coll.IsTouchingLayers(ground))
            //{
                
            //    fall = false;
            //}
            foreach (var b in bodies)
            {
                RaycastHit2D hhit = Physics2D.Raycast(b.transform.position, moveDir, 0.001f, ground);
                if (hhit)
                    fall = false;
            }
            if (fall)
            {
//#if UNITY_EDITOR
//                UnityEditor.EditorApplication.isPaused = true;

//#endif
                falling = true;
                if (orient == Vector3.right)
                    GetComponent<SpriteRenderer>().sprite = fallingSprites[0];
                else if (orient == Vector3.down)
                    GetComponent<SpriteRenderer>().sprite = fallingSprites[1];
                else if (orient == Vector3.left)
                    GetComponent<SpriteRenderer>().sprite = fallingSprites[2];
                else if (orient == Vector3.up)
                    GetComponent<SpriteRenderer>().sprite = fallingSprites[3];

                coll.enabled = false;
                foreach (var b in bodies) b.coll.enabled = false;
                GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                GetComponent<SpriteRenderer>().sortingOrder = -10;
                foreach (var b in bodies)
                {
                    b.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    b.GetComponent<SpriteRenderer>().sortingOrder = -10;
                }
                    Fall();
            }

        }
    }



    void Fall()
    {
        

        transform.position = new Vector3(transform.position.x,
            transform.position.y - Time.deltaTime * fallingSpeed, transform.position.z);
    }

    /// <summary>
    /// ��ʼ���ߵ�ͷ������
    /// </summary>
    void InitSprite()
    {
        int index = 0;
        for (int i = 0; i < sprites.Count; i++)
        {
            if (GetComponent<SpriteRenderer>().sprite == sprites[i])
            {
                index = i;
                break;
            }
        }
        switch (index)
        {
            case 0: orient = Vector3.right; break;
            case 1: orient = Vector3.down; break;
            case 2: orient = Vector3.left; break;
            case 3: orient = Vector3.up; break;
        }
    }
    /// <summary>
    /// �����ߵ��ƶ�����dir������ͷ����ͼ
    /// </summary>
    /// <param name="dir">�ƶ�����</param>
    public void UpdateSprite(Vector3 dir)
    {
        if (flying || falling || onShow) return;
        //if (dir == orient) 
        //    return;
        if (dir == Vector3.right)
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

        


    }

    void UpdateBody()
    {
        //�����������ͼ
        List<Vector3> ts = new List<Vector3>();
        ts.Add(transform.position);
        foreach (var b in bodies) ts.Add(b.transform.position);
        ts.Add(Vector3.zero);
        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].UpdateSprite(ts[i], ts[i + 2]);
        }
    }

    void InputKey()
    {
        Vector3 tempMoveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            if ((Vector3.up + orient).x == 0 && (Vector3.up + orient).y == 0) return;
            tempMoveDir = Vector3.up;
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            if ((Vector3.down + orient).x == 0 && (Vector3.down + orient).y == 0) return;
            tempMoveDir = Vector3.down;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            if ((Vector3.right + orient).x == 0 && (Vector3.right + orient).y == 0) return;
            tempMoveDir = Vector3.right;
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            if ((Vector3.left + orient).x == 0 && (Vector3.left + orient).y == 0) return;
            tempMoveDir = Vector3.left;
        }

        if (tempMoveDir != Vector3.zero)
            tipTimer = 0;

        //��������˷����
        if (tempMoveDir != Vector3.zero && !flying && !falling && !onShow)
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
        if (flying || falling) return;
    
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
            //��ʱ���Ӧ�óԵ�
            else return false;
        }
    }

    /// <summary>
    /// ���ض������ƶ�
    /// </summary>
    /// <param name="dir">��������</param>
    void Move(Vector3 dir)
    {
        if (bodies.Count > 0)
        {
            bodies[^1].GenerateDust(bodies[^1].transform.position);
        }
        //ͷ�������������λ��λ��
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
        UpdateSprite(dir);
        orient = dir;
        if (status.Count > 0)
        {
            //�Ե����㽶
            EatBanana();
            status.Clear();
        }
        
        
    }

    public void EatPepper()
    {
        StartCoroutine(EEatPepper());
    }

    IEnumerator EEatPepper()
    {
        Debug.Log("Э�̿�ʼ");
        onShow = true;
        if (moveDir.x > 0.5f)
        {
            fire.transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("���� x->");
            GetComponent<SpriteRenderer>().sprite = spicySprites[0];
            //Debug.Log("sprite:" + spicySprites[0]);
        }
        if (moveDir.x < -0.5f)
        {
            Debug.Log("���� x<-");
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
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Э�̼���ִ��");
        flying = true;
        onShow = false;
        flyDir = -moveDir;
        GameManager.instance.flyingObjects.Add(this.gameObject);
        foreach (Body b in bodies)
        {
            b.flying = true;
            b.flyDir = flyDir;
            GameManager.instance.flyingObjects.Add(b.gameObject);
        }
        fire.SetActive(true);

    }

    public void EatBanana()
    {
        GameObject bd = Instantiate(bodyPrefab, transform);
        Vector3 pos = status[^1];
        bd.transform.position = new Vector3(pos.x, pos.y, pos.z);
        bodies.Add(bd.GetComponent<Body>());
        if (orient == Vector3.right)
            GetComponent<SpriteRenderer>().sprite = happySprites[0];
        else if (orient == Vector3.down)
            GetComponent<SpriteRenderer>().sprite = happySprites[1];
        else if (orient == Vector3.left)
            GetComponent<SpriteRenderer>().sprite = happySprites[2];
        else if (orient == Vector3.up)
            GetComponent<SpriteRenderer>().sprite = happySprites[3];

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
            //ʤ��
            Debug.Log("Victory");
            StartCoroutine(IntoHole(true));
        }
        else if(collision.tag.Equals("SandPit"))
        {
            if(!flying)
                StartCoroutine(IntoHole(false));
        }
        else if (collision.tag.Equals("Collectable") && flying)
        {
            Debug.Log("ײ�����ռ�����");
            GameManager.instance.flyingObjects.Add(collision.gameObject);
            collision.gameObject.GetComponent<Collectable>().flying = true;
            collision.gameObject.GetComponent<Collectable>().flyDir = flyDir;
        }
    }

    IEnumerator IntoHole(bool isHole)
    {
        if(!isHole)
        {
            if (orient == Vector3.right)
                GetComponent<SpriteRenderer>().sprite = fallingSprites[0];
            else if (orient == Vector3.down)
                GetComponent<SpriteRenderer>().sprite = fallingSprites[1];
            else if (orient == Vector3.left)
                GetComponent<SpriteRenderer>().sprite = fallingSprites[2];
            else if (orient == Vector3.up)
                GetComponent<SpriteRenderer>().sprite = fallingSprites[3];
            yield return new WaitForSeconds(0.5f);
        }
        GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        GetComponent<SpriteRenderer>().sortingOrder = -1000;
        int num = bodies.Count;
        for (int i = 0; i < num; i++)
        {
            Move(orient);
            bodies[i].GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            bodies[i].GetComponent<SpriteRenderer>().sortingOrder = -1000;
            yield return new WaitForSeconds(0.15f);
        }
        if(isHole)
        {

            GameManager.instance.gamePass = true;
        }
        else
        {
            Debug.Log("��ɳ����");
            GameManager.instance.gameOver = true;
        }
        onShow = true;
    }
}

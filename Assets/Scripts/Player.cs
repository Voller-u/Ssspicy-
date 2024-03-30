using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("��������")]
    public bool flying;
    [Header("���岿λ")]
    public List<Body> bodies = new List<Body>();
    [Header("ͷ����ͼ")]
    public List<Sprite> sprites;
    //ͷ������
    public Vector3 orient;
    private Rigidbody2D rb;
    public LayerMask detectLayer;
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
    }

    /// <summary>
    /// ��ʼ���ߵ�ͷ������
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
    /// �����ߵ��ƶ�����dir������ͷ����ͼ
    /// </summary>
    /// <param name="dir">�ƶ�����</param>
    void UpdateSprite(Vector3 dir)
    {
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

        //�����������ͼ
        List<Vector3> ts = new List<Vector3>();
        ts.Add(transform.position);
        foreach(var b in bodies) ts.Add(b.transform.position);
        ts.Add(Vector3.zero);
        for(int i=0;i<bodies.Count;i++)
        {
            bodies[i].UpdateSprite(ts[i], ts[i + 2]);
        }
    }

    void InputKey()
    {
        Vector3 moveDir = Vector3.zero;
        if(Input.GetKeyDown(KeyCode.W))
        {
            if ((Vector3.up + orient).x == 0 && (Vector3.up + orient).y == 0) return;
            moveDir = Vector3.up;
        }

        else if(Input.GetKeyDown(KeyCode.S))
        {
            if ((Vector3.down + orient).x == 0 && (Vector3.down + orient).y == 0) return;
            moveDir=Vector3.down;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            if ((Vector3.right + orient).x == 0 && (Vector3.right + orient).y == 0) return;
            moveDir= Vector3.right;
        }

        else if( Input.GetKeyDown(KeyCode.A)) 
        {
            if ((Vector3.left + orient).x == 0 && (Vector3.left + orient).y == 0) return;
            moveDir = Vector3.left;
        }

        //��������˷����
        if(moveDir != Vector3.zero)
        {
            if(CanMoveDir(moveDir))
            {
                Move(moveDir);
            }
        }
    }

    private bool CanMoveDir(Vector3 moveDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 1f, detectLayer);

        if (!hit)
            return true;
        else
        {
            if (hit.collider.GetComponent<Collectable>() != null)
            {
                return hit.collider.GetComponent<Collectable>().CanMoveDir(moveDir);
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
    }
}

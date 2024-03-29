using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [Header("飞行状态")]
    public bool flying;
    [Header("身体部位贴图")]
    public List<Sprite> sprites;
    [Header("身体转角贴图")]
    public List<Sprite> cornerSprites;
    [Header("尾巴贴图")]
    public List<Sprite> tailSprites;

    public bool isTail;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
             
            Debug.Log(vec);
            if(vec.x > 0 && vec.y >0)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[2];
            }
            if (vec.x > 0 && vec.y < 0)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[0];
            }
            if (vec.x < 0 && vec.y > 0)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[3];
            }
            if (vec.x < 0 && vec.y < 0)
            {
                GetComponent<SpriteRenderer>().sprite = cornerSprites[1];
            }

            if ((left.x < pos.x && right.x > pos.x) || (left.x > pos.x && right.x < pos.x))
                GetComponent<SpriteRenderer>().sprite = sprites[0];
            if ((left.y < pos.y && right.y > pos.y) || (left.y > pos.y && right.y < pos.y))
                GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }
}

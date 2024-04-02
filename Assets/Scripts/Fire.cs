using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Vector3 generatePoint;
    public Vector3 orient;
    public GameObject firePrefab;
    [Header("生成火焰秒")]
    public float maxTime;
    public float timer;//生成火焰计时器
    [Header("火焰存在时间")]
    public float fireTime;
    [Header("火焰速度")]
    public float fireSpeed;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
        GenerateFire();
    }

    void GenerateFire()
    {
        if(transform.parent.GetComponent<Player>() != null)
            orient = transform.parent.GetComponent<Player>().orient;
        generatePoint = transform.position + orient * 0.4f;
        timer += Time.deltaTime;
        if(timer > maxTime)
        {
            timer = 0;
            GameObject fire = Instantiate(firePrefab, transform);
            fire.transform.parent = transform;
            fire.transform.position = generatePoint;
            fire.transform.rotation = transform.rotation;

            float angle = Random.Range(-45f, 45f);
            float baseAngle = 0;
            if (orient == Vector3.up) baseAngle = 90;
            else if (orient == Vector3.down) baseAngle = 270;
            else if (orient == Vector3.left) baseAngle = 180;
            else if (orient == Vector3.right) baseAngle = 0;
            fire.transform.Rotate(new Vector3(0, 0, angle));

            Vector3 dir = Vector3.zero;
            dir.x = Mathf.Cos((baseAngle + angle) * 2 * Mathf.PI /360);
            dir.y = Mathf.Sin((baseAngle + angle) * 2 * Mathf.PI / 360);

            //Debug.Log("dir = " + dir);

            fire.GetComponent<Rigidbody2D>().velocity = dir * fireSpeed;

            Destroy(fire.gameObject, 1f);
        }
    }

}

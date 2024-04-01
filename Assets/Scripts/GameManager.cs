using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> flyingObjects;

    public int CollectableNum;

    public bool gameOver;

    public bool intoLevelChoose;

    public GameObject playerPrefab;
    public GameObject bodyPrefab;
    public GameObject pepperPrefab;
    public GameObject bananaPrefab;
    public GameObject icePrefab;

    [Serializable]
    public struct Items
    {
        public int bodyNum;
        public List<Vector3> snakePosition;

        public int pepperNum;
        public List<Vector3> pepperPosition;

        public int bananaNum;
        public List<Vector3> bananaPosition;

        public int iceNum;
        public List<Vector3> icePosition;
    }

    public List<Items> items;

    void Awake()
    {
        if(GameManager.instance == null)
        {
            GameManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFlyEnd()
    {
        Debug.Log("飞行时间结束了");
        foreach(GameObject obj in flyingObjects)
        {
            if(obj.GetComponent<Player>() != null)
            {
                obj.GetComponent<Player>().flying = false;
                obj.GetComponent<Player>().fire.SetActive(false);
                obj.GetComponent<Player>().UpdateSprite(-obj.GetComponent<Player>().flyDir);
            }
            else if(obj.GetComponent<Body>() != null)
            {
                obj.GetComponent<Body>().flying = false;
            }
            else if(obj.GetComponent<Collectable>() != null)
            {
                obj.GetComponent <Collectable>().flying = false;
            }
        }
        flyingObjects.Clear();
    }

    public void GameOver()
    {

    }

    public void GenerateItems(int index)
    {
        //生成蛇
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = items[index].snakePosition[0];
        for(int i = 1; i <= items[index].bodyNum;i++)
        {
            GameObject body = Instantiate(bodyPrefab,player.transform);
            player.GetComponent<Player>().bodies.Add(body.GetComponent<Body>());    
            body.transform.localPosition = items[index].snakePosition[i];
        }

        //生成辣椒
        for(int i = 0; i < items[index].pepperNum;i++)
        {
            GameObject pepper = Instantiate(pepperPrefab);
            pepper.transform.position = items[index].pepperPosition[i];
        }

        //生成香蕉
        for (int i = 0; i < items[index].bananaNum; i++)
        {
            GameObject banana = Instantiate(bananaPrefab);
            banana.transform.position = items[index].bananaPosition[i];
        }

        //生成冰块
        for (int i = 0; i < items[index].iceNum; i++)
        {
            GameObject ice = Instantiate(icePrefab);
            ice.transform.position = items[index].icePosition[i];
        }

    }
}

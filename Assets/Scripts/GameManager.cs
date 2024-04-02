using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> flyingObjects;

    public int CollectableNum;
    public int gameLevel;
    public int maxLevelId;

    public bool gameOver;
    public bool gamePass;
    //掉落物掉下去了
    public bool collectableFall;

    public bool intoLevelChoose;

    
    

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

    public List<TextAsset> levelMaps;

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

    public void InitGame()
    {
        GameManager.instance.CollectableNum = 0;
        GameManager.instance.collectableFall = false;
        GameManager.instance.gameOver = false;
        GameManager.instance.gamePass  = false;
    }

}

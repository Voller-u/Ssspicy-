using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> flyingObjects;

    public int CollectableNum;
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
        foreach(GameObject obj in flyingObjects)
        {
            if(obj.GetComponent<Player>() != null)
            {
                obj.GetComponent<Player>().flying = false;
                obj.GetComponent<Player>().fire.SetActive(false);
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
}

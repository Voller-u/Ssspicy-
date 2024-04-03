using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class MapManager : MonoBehaviour
{
    public Tile groundTile;
    public Tilemap groundMap;

    public GameObject playerPrefab;
    public GameObject bodyPrefab;
    public GameObject pepperPrefab;
    public GameObject bananaPrefab;
    public GameObject icePrefab;
    public GameObject stonePrefab;
    public GameObject woodPrefab;
    public GameObject holePrefab;
    public GameObject sandPitPrefab;
    public GameObject stoneBotmPrefab;

    public List<TextAsset> maps;
    //public List<GameObject> objects;

    private void Start()
    {
        StartCoroutine(GenerateMap(GameManager.instance.gameLevel));
    }

    IEnumerator GenerateMap(int index)
    {
        index = index - 1;
        
        string[] map = maps[index].text.Split('\n');
        for (int i = 0; i < map.Length; i++)
            map[i] = map[i].Replace(" ", "").Replace("\r", "");

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] != '*')
                {
                    Debug.Log("i = " + i + " j = " + j);
                    groundMap.SetTile(new Vector3Int(j - 14, 7 - i, 0), groundTile);
                }
            }
        }
        yield return new WaitForEndOfFrame();
        GameObject player = Instantiate(playerPrefab);
        int headOrient = map[^1][0] - '0';
        player.GetComponent<SpriteRenderer>().sprite = player.GetComponent<Player>().sprites[headOrient];
        GameObject tail = null;
        for (int i = 0; i < map.Length-1; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {

                Vector3 pos = new Vector3(j - 13.5f, 7.5f - i, 0);

                if (map[i][j] == 'T')
                {
                    GameObject stone = Instantiate(stonePrefab);
                    stone.transform.position = pos;
                }
                else if (map[i][j] == 'D')
                {
                    GameObject stoneBtm = Instantiate(stoneBotmPrefab);
                    stoneBtm.transform.position = pos;
                }
                else if (map[i][j] == 'S')
                {
                    player.transform.position = pos;
                }
                else if (map[i][j] == 'B')
                {
                    GameObject body = Instantiate(bodyPrefab,player.transform);
                    player.GetComponent<Player>().bodies.Add(body.GetComponent<Body>());
                    body.transform.position = pos;
                }
                else if (map[i][j] == 'L')
                {
                    tail = Instantiate(bodyPrefab, player.transform);
                    tail.transform.position = pos;
                }
                else if (map[i][j] == 'P')
                {
                    GameObject pepper = Instantiate(pepperPrefab);
                    pepper.transform.position = pos;
                    
                }
                else if (map[i][j] == 'A')
                {
                    GameObject banana = Instantiate(bananaPrefab);
                    banana.transform.position = pos;
                }
                else if (map[i][j] == 'H')
                {
                    GameObject hole = Instantiate(holePrefab);
                    hole.transform.position = pos;
                }
                else if (map[i][j] == 'I')
                {
                    GameObject ice = Instantiate(icePrefab);
                    ice.transform.position = pos;
                }
                else if (map[i][j] == 'W')
                {
                    GameObject woodWall = Instantiate(woodPrefab);
                    woodWall.transform.position = pos;
                }
                else if (map[i][j] == 'N')
                {
                    GameObject sandPit = Instantiate(sandPitPrefab);
                    sandPit.transform.position = pos;
                }

            }
        }
        player.GetComponent<Player>().bodies.Add(tail.GetComponent<Body>());
    }
}

/*
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
 */
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    GameObject player;

    int MAX_SIZE;
    [Range(2,10)]
    public int mapSize;

    [Range(0,10)]
    public int trapNum;

    int posX = 0;
    int posY = 0;
    int wallSize = 4;

    public GameObject[] blocks;
    public GameObject[] props;
    public GameObject start;
    public GameObject end;

    void Awake()
    {
        MAX_SIZE = mapSize * mapSize;

        player = GameManager.Instance.Player.gameObject;
    }

    void Start()
    {
        createmap();
        createPoint();
        createProps();
    }

    void createmap()
    {
        int count = 0;
        for(int i = 0; i < mapSize; i++)
        {
            posX = 0;
            for(int j = 0; j < mapSize; j++)
            {
                int randomWall = Random.Range(0, blocks.Length);

                GameObject obj = Instantiate(blocks[randomWall], this.transform);
                obj.name = $"{++count}";
                obj.transform.position = new Vector3(posX, 0, posY);
                posX += wallSize;
            }
            posY += wallSize;
        }
    }

    void createPoint()
    {
        int startpoint = Random.Range(0, MAX_SIZE);
        int endpoint = Random.Range(0, MAX_SIZE);

        GameObject startobj = Instantiate(start, this.transform);
        startobj.transform.position = new Vector3((int)startpoint / mapSize * wallSize, 0, startpoint % mapSize * wallSize);
        player.transform.position = new Vector3((int)startpoint / mapSize * wallSize, 0, startpoint % mapSize * wallSize);

        GameObject endobj = Instantiate(end, this.transform);
        endobj.transform.position = new Vector3((int)endpoint / mapSize * wallSize, 0, endpoint % mapSize * wallSize);
    }

    void createProps()
    {
        int count = 0;
        while(++count <= trapNum)
        {
            int propPoint = Random.Range(0, MAX_SIZE);
            int propNum = Random.Range(0, props.Length);

            GameObject obj = Instantiate(props[propNum], this.transform);
            obj.transform.position = new Vector3((int)propPoint / mapSize * wallSize, 0, propPoint % mapSize * wallSize);
        }
    }
}

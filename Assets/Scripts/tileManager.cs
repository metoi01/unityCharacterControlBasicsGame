using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    public GameObject[] tiles;
    public float zSpawn = 0;
    public float tileLength = 49;
    public Transform player;
    private List<GameObject> _activeTiles = new List<GameObject>();
    private GameObject _object;
    private int _counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            spawnTile(Random.Range(0, tiles.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (zSpawn - player.transform.position.z < tiles.Length * tileLength)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                spawnTile(Random.Range(0, tiles.Length));
                if (_counter > 15)
                {
                    deleteTile();
                    _counter -= 5;
                }
            }
        }
    }
    public void spawnTile(int tileIndex)
    {
        _object = Instantiate(tiles[tileIndex], transform.forward * zSpawn, transform.rotation);
        _activeTiles.Add(_object);
        _counter++;
        zSpawn += tileLength;
        Debug.Log(zSpawn);
    }
    public void deleteTile()
    {
        for (int i = 5; i > 0; i--)
        {
            Destroy(_activeTiles[0]);
            _activeTiles.RemoveAt(0);
        }
    }
}

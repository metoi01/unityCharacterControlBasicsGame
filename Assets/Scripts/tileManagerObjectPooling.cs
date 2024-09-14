using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManagerObjectPooling : MonoBehaviour
{
    public GameObject[] tiles;
    public float zSpawn = 0;
    public float tileLength = 49;
    public Transform player;
    private List<GameObject> _activeTiles = new List<GameObject>();
    private List<GameObject> _oldTiles = new List<GameObject>();
    private GameObject _object;
    private int _counter = 0;
    private int _counterHelper;
    private int _tilesToSpawn;

    void Start()
    {
        _tilesToSpawn = tiles.Length * 2 + 2;
        for (int i = 0; i < _tilesToSpawn; i++)
        {
            SpawnTile(Random.Range(0, tiles.Length));
        }
    }

    void Update()
    {
        ReArrangeTiles();
    }
    public void SpawnTile(int tileIndex)
    {
        _object = Instantiate(tiles[tileIndex], transform.forward * zSpawn, transform.rotation);
        _activeTiles.Add(_object);
        zSpawn += tileLength;
    }
    public void ReArrangeTiles()
    {
        if (player.transform.position.z > _activeTiles[_counter].transform.position.z + tileLength)
        {
            _activeTiles[_counter].SetActive(false);
            _activeTiles[_counter].transform.position = new Vector3(_activeTiles[_counter].transform.position.x, _activeTiles[_counter].transform.position.y, zSpawn);
            _counterHelper = _counter;
            zSpawn += tileLength;
            if (_counter < tiles.Length * 2 + 1)
            {
                _counter++;
            }
            else
            {
                _counter = 0;
            }
            _activeTiles[_counterHelper].SetActive(true);
        }
    }
}

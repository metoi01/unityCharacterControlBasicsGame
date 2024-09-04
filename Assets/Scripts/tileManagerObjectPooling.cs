using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManagerObjectPooling : MonoBehaviour
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


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tiles.Length * 2 + 2; i++)
        {
            spawnTile(Random.Range(0, tiles.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        reArrangeTiles();
    }
    public void spawnTile(int tileIndex)
    {
        _object = Instantiate(tiles[tileIndex], transform.forward * zSpawn, transform.rotation);
        _activeTiles.Add(_object);
        zSpawn += tileLength;
        Debug.Log(zSpawn);
    }
    public void reArrangeTiles()
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

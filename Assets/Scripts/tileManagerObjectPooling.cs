using System.Collections.Generic;
using UnityEngine;

public class TileManagerObjectPooling : MonoBehaviour
{
    public GameObject[] tiles;
    public float zSpawn = 0;
    public float tileLength = 49;
    public Transform playerTransform;
    private List<GameObject> _activeTiles = new List<GameObject>();
    private GameObject _object;
    private GameObject _swap;
    private int _randomNum;
    private int _counter = 0;
    private int _counterHelper;
    private int _tilesToSpawn;
    private bool _isGameStarted = true;
    private bool _didCharacterMove = false;

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
        if (playerTransform.transform.position.z > 4)
        {
            _didCharacterMove = true;
        }
        else if (!(playerTransform.transform.position.z > 4) && _didCharacterMove)
        {
            RestartGame();
            _didCharacterMove = false;
        }
        if (_isGameStarted)
        {
            ReArrangeTiles();
        }
    }
    public void SpawnTile(int tileIndex)
    {
        _object = Instantiate(tiles[tileIndex], transform.forward * zSpawn, transform.rotation);
        _activeTiles.Add(_object);
        zSpawn += tileLength;
    }
    private void ReArrangeTiles()
    {
        if (playerTransform.transform.position.z > _activeTiles[_counter].transform.position.z + tileLength)
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
    private void RestartGame()
    {
        zSpawn = 0;
        _counter = 0;
        for (int i = 0; i < _tilesToSpawn; i++)
        {
            _randomNum = Random.Range(i, _tilesToSpawn);
            _activeTiles[_counter].SetActive(false);
            _swap = _activeTiles[_randomNum];
            _activeTiles[_randomNum] = _activeTiles[_counter];
            _activeTiles[_counter] = _swap;
            _activeTiles[_counter].transform.position = new Vector3(_activeTiles[_counter].transform.position.x, _activeTiles[_counter].transform.position.y, zSpawn);
            zSpawn += tileLength;
            _activeTiles[_counter].SetActive(true);
            _counter++;
        }
        _isGameStarted = true;
        _counter = 0;
    }
}

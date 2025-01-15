using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManagerObjectPooling : MonoBehaviour
{
    public static Action<GameObject> TileArranged;
    
    public GameObject[] tiles;
    public GameObject startGround;
    
    public Transform playerTransform;
    
    public float zSpawn = 0;
    public float tileLength = 19;
    
    private List<GameObject> _activeTiles = new List<GameObject>();
    
    private GameObject _object;
    private GameObject _startingGround;
    private GameObject _swap;
    
    private int _randomNum;
    private int _counter = 0;
    private int _counterHelper;
    private int _tilesToSpawn;
    
    private void OnEnable()
    {
        GameLogic.PrepareScene += RestartGame;
    }
    
    void Start()
    {
        _startingGround = Instantiate(startGround, transform.forward * zSpawn, transform.rotation);
        zSpawn += tileLength;
        _tilesToSpawn = tiles.Length;
        
        List<int> tileIndices = new List<int>(tiles.Length);
        for (int i = 0; i < tiles.Length; i++)
        {
            tileIndices.Add(i);
        }
        tileIndices.Sort((a, b) => Random.Range(-1, 2));

        
        for (int i = 0; i < _tilesToSpawn; i++)
        {
            SpawnTile(tileIndices[i]);
        }
    }
    private void OnDisable()
    {
        GameLogic.PrepareScene -= RestartGame;
    }

    void Update()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            ReArrangeTiles();
        }
    }
    private void SpawnTile(int tileIndex)
    {
        _object = Instantiate(tiles[tileIndex], transform.forward * zSpawn, transform.rotation);
        _activeTiles.Add(_object);
        zSpawn += tileLength;
    }
    private void ReArrangeTiles()
    {
        if(playerTransform.transform.position.z > _activeTiles[_counter].transform.position.z + tileLength)
        {
            _activeTiles[_counter].GetComponent<Renderer>().enabled = false;
            _activeTiles[_counter].transform.position = new Vector3(_activeTiles[_counter].transform.position.x, _activeTiles[_counter].transform.position.y, zSpawn);
            TileArranged?.Invoke(_activeTiles[_counter]);
            _counterHelper = _counter;
            zSpawn += tileLength;
            if (_counter < _activeTiles.Count - 1)
            {
                _counter++;
            }
            else
            {
                _counter = 0;
            }
            _activeTiles[_counterHelper].GetComponent<Renderer>().enabled = true;
        }
    }
    private void RestartGame()
    {
        zSpawn = 19;
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
        _counter = 0;
    }
}

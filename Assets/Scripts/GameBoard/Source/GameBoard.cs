using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour, IGameBoard, ISpawnable
{
    public GameObject gameTilePrefab;
    private Dictionary<Vector3, IGameTileController> gameTiles = new Dictionary<Vector3, IGameTileController>();

    private delegate void OnGenerated();

    CallbackHandler onCreationCompleteHandler;

    public void GenerateBoard(List<Vector3> points)
    {
        StartCoroutine(SpawnTiles(points, 0.1f));
    }

    public void GenerateBoard(List<Vector3> points,  float maxSpawnDelay)
    {
        StartCoroutine(SpawnTiles(points, maxSpawnDelay));
    }

    public IEnumerator SpawnTiles(List<Vector3> points, float maxSpawnDelay)
    {
        foreach (Vector3 point in points)
        {
            if (!gameTiles.ContainsKey(point))
            {
                GameObject tile = Instantiate<GameObject>(gameTilePrefab, point, Quaternion.identity);
                tile.transform.parent = transform;
                gameTiles.Add(point, new GameTileController(tile.GetComponent<IGameTile>()));
                if (!Mathf.Approximately(Mathf.Max(0f, maxSpawnDelay), 0.0f))
                {
                    yield return new WaitForSeconds(maxSpawnDelay);
                }
            }
            else
            {
                Debug.LogWarning("Duplicate point, " + point);
            }
        }
        float waitTime = 0f;
        if (GetTileCount() > 0)
        {
            // just get the first, they should all have the same spawn times
            waitTime = gameTiles[points[0]].GetSpawnTime();
        }
        yield return new WaitForSeconds(waitTime);
        if (onCreationCompleteHandler != null)
        {
            onCreationCompleteHandler.Invoke();
        }
    }

    public void TearDownBoard()
    {
        foreach (KeyValuePair<Vector3,IGameTileController> pair in gameTiles)
        {
            pair.Value.DespawnTile();
        }
        gameTiles = new Dictionary<Vector3, IGameTileController>();
    }

    public void SetTileState(Vector3 position, ITileState newTileState)
    {
        if (gameTiles.ContainsKey(position))
        {
            gameTiles[position].SetTileState(newTileState);
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }

    public void SetOnCreationCompleteHandler(CallbackHandler callback)
    {
        onCreationCompleteHandler = callback;
    }


    public int GetTileCount()
    {
        return gameTiles.Count;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour, IGameBoard
{

    // test variables
    public List<Vector3> injectedTileCoordinates;


    public GameObject gameTilePrefab;
    private List<Vector3> tileCoordinates;
    private Dictionary<Vector3, IGameTileController> gameTiles = new Dictionary<Vector3, IGameTileController>();

    IEnumerator Start()
    {
        yield return GenerateBoard(injectedTileCoordinates,0.1f);
    }

    public IEnumerator GenerateBoard(List<Vector3> points)
    {
        yield return GenerateBoard(points, 0.1f);
    }

    public IEnumerator GenerateBoard(List<Vector3> points, float maxSpawnDelay)
    {
        foreach (Vector3 point in points)
        {
            if (!gameTiles.ContainsKey(point))
            {
                gameTiles.Add(point, new GameTileController(Instantiate<GameObject>(gameTilePrefab, point, Quaternion.identity).gameObject.AddComponent<GameTile>()));
                if (!Mathf.Approximately(Mathf.Max(0f, maxSpawnDelay), 0.0f))
                {
                    yield return new WaitForSeconds(Random.Range(0f, maxSpawnDelay));
                }
            }
            else
            {
                Debug.LogWarning("Duplicate point, " + point);
            }
        }

        tileCoordinates = points;
        yield return null;
    }

    public void TearDownBoard()
    {
        tileCoordinates = new List<Vector3>();
        foreach (KeyValuePair<Vector3,IGameTileController> pair in gameTiles)
        {
            pair.Value.DespawnTile();
        }
    }

    public List<Vector3> GetAllTileCoordinates()
    {
        return new List<Vector3>(tileCoordinates);
    }

    void SetTileColor(Vector3 position, Color color)
    {
        if (gameTiles.ContainsKey(position))
        {
            gameTiles[position].SetTileColor(color);
        }
        else
        {
            Debug.LogWarning("Attempted to change the color of tile at invalid position: " + position + ", color: " + color);
        }
    }

    void SetAllTileColor(Color color)
    {
        foreach (Vector3 point in tileCoordinates)
        {
            SetTileColor(point, color);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    public enum GameControllerState
    {
        CREATED, // created
        IDLE, // ready to get input
        BUSY, // busy processing input, prevent any more input from occuring
		WIN,
		LOSE
    }

    // game objects
    public GameObject gameTilePrefab;
    public GameObject playerPrefab;

    public List<Vector3> playArea;
    private List<GameObject> gameTiles = new List<GameObject>();
    private GameObject playerCube;

    // player objects
    private IPlayer player;
    private IPlayerController playerController;
    private GameObject playerGameObject;

    // controller state
    private GameControllerState state = GameControllerState.CREATED;
    
    // Use this for initialization
	IEnumerator Start () {

        yield return SpawnTiles();

        playerGameObject = Instantiate<GameObject>(playerPrefab, Vector3.zero, Quaternion.identity);
        player = playerGameObject.AddComponent<Player>();
        playerController = new PlayerController(player);

        TransitionToState(GameControllerState.IDLE);

	}
	
    private IEnumerator SpawnTiles()
    {

        foreach (Vector3 point in playArea)
        {
            Debug.Log("Generating a tile at " + point.ToString());
            GameObject a = Instantiate<GameObject>(gameTilePrefab, point, Quaternion.identity);
            a.transform.parent = gameObject.transform;
            gameTiles.Add(a);
            yield return new WaitForSeconds(Random.Range(0f, 0.1f));
        }
    }

	// Update is called once per frame
	void Update () {

        switch (state)
        {
        case GameControllerState.CREATED:
            // do nothing
            break;
        case GameControllerState.IDLE:
            // in idle we check inputs
            CheckInputs();
            break;
        case GameControllerState.BUSY:
            break;
		case GameControllerState.LOSE:
			break;
		case GameControllerState.WIN:
			break;
        }

    }

    private bool CheckInputs()
    {
        TumblingDirection dir = TumblingDirection.NONE;

        if (Input.GetButton("Up"))
        {
            dir = TumblingDirection.UP;
        }
        else if (Input.GetButton("Down"))
        {
            dir = TumblingDirection.DOWN;
        }
        else if (Input.GetButton("Left"))
        {
            dir = TumblingDirection.LEFT;
        }
        else if (Input.GetButton("Right"))
        {
            dir = TumblingDirection.RIGHT;
        }
        else
        {
            return false;
        }
			
        playerController.StartMovePlayer(dir, OnPlayerTumbleFinish);
        TransitionToState(GameControllerState.BUSY);


        return true;
    }

    void OnPlayerTumbleFinish(EventCallbackError errorCode)
    {
        if (errorCode == EventCallbackError.NOERROR)
        {
            TransitionToState(GameControllerState.IDLE);
            Vector3 tilePosition = playerController.GetPlayerCurrentPosition();
            if (CheckBounds(tilePosition))
            {
                CheckWin();
            }
            else
            {
                TransitionToState(GameControllerState.LOSE);
            }
        }


    }

	private bool CheckBounds(Vector3 playerPosition)
	{
		int nearby = 0;
		foreach (Vector3 point in playArea)
		{
			if ((playerPosition-point).sqrMagnitude <= 0.1)
			{
				nearby += 1;
			}
		}

		if (nearby == 1) 
		{
			return true;
		}
		else
		{
			return false;
		}
	}

    private void CheckWin()
    {
		int tileCount = 0;
		foreach (GameObject tile in gameTiles)
		{
			if (tile.GetComponent<GameTileController>().state == GameTileController.GameTileControllerState.ACTIVE)
			{
				tileCount += 1;
			}
		}

		if (tileCount == playArea.Count) 
		{
			TransitionToState(GameControllerState.WIN);
		}
    }

    private void TransitionToState(GameControllerState newState)
    {
        Debug.Log("Transitioning state (" + state.ToString() + " -> " + newState.ToString() + ")", gameObject);
        switch (newState)
        {
            case GameControllerState.CREATED:
                // do nothing, we cant transition back to this state
                break;
            case GameControllerState.IDLE:
                if (state == GameControllerState.CREATED || state == GameControllerState.BUSY)
                {
                    state = GameControllerState.IDLE;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
            case GameControllerState.BUSY:
                if (state == GameControllerState.IDLE)
                {
                    state = GameControllerState.BUSY;
                }
                else
                {
                    // do nothing, will get caught
                }
                break;
		case GameControllerState.LOSE:
			if (state == GameControllerState.IDLE) 
			{
				state = GameControllerState.LOSE;
			}
			break;
		case GameControllerState.WIN:
			if (state == GameControllerState.IDLE) 
			{
				state = GameControllerState.WIN;
			}
			break;
        }
        if (state != newState)
        {
            Debug.LogError("Incorrect transition. ", gameObject);
        }
    }
}

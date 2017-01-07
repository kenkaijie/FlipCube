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
    private Dictionary<Vector3, IGameTileController> gameTiles = new Dictionary<Vector3, IGameTileController>();

    // player objects
    private IPlayer player;
    private IPlayerController playerController;
    private GameObject playerGameObject;

    // game tile objects
    private IGameTile gameTile;
    private GameObject gameTileGameObject;
    private IGameTileController gameTileController;

    // controller state
    private GameControllerState state = GameControllerState.CREATED;
    
    // Use this for initialization
	IEnumerator Start () {
        Debug.logger.logEnabled = true;
        yield return SpawnTiles();

        playerGameObject = Instantiate<GameObject>(playerPrefab, Vector3.zero, Quaternion.identity);
        player = playerGameObject.AddComponent<Player>();
        playerController = new PlayerController(player);

        gameTiles[Vector3.zero].ActivateTile();

        TransitionToState(GameControllerState.IDLE);

	}
	
    private IEnumerator SpawnTiles()
    {

        foreach (Vector3 point in playArea)
        {
            gameTileGameObject = Instantiate<GameObject>(gameTilePrefab, point, Quaternion.identity);
            gameTile = gameTileGameObject.AddComponent<GameTile>();
            gameTiles.Add(point, (IGameTileController) new GameTileController(gameTile));
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

            if (gameTiles.ContainsKey(tilePosition))
            {
                IGameTileController tileController = gameTiles[tilePosition];
                if (tileController.IsActivated())
                {
                    tileController.DeactivateTile();
                }
                else
                {
                    tileController.ActivateTile();
                }
            }

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
		foreach (KeyValuePair<Vector3,IGameTileController> gameTileKeyValuePair in gameTiles)
		{
            IGameTileController tileController = gameTileKeyValuePair.Value;

            if (tileController.IsActivated())
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

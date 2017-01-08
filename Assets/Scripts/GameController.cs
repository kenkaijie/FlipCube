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

    private Dictionary<string, ITileState> tileStatesRegister = new Dictionary<string, ITileState>();

    // game objects
    public GameObject gameTilePrefab;
    public GameObject playerPrefab;
    public GameObject gameBoardPrefab;

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

    // game board objects
    private IGameBoardController gameBoardController;

    // controller state
    private GameControllerState state = GameControllerState.CREATED;
    
    // Use this for initialization
	void Start () {
        Debug.logger.logEnabled = true;

        tileStatesRegister.Add("Active", new TileState("Active", new Color(0.882f,0.129f,0.129f,0.502f)));
        tileStatesRegister.Add("Inactive", new TileState("Inactive", new Color(0.525f, 0.957f, 0.549f, 0.502f)));

        gameBoardController = new GameBoardController(Instantiate<GameObject>(gameBoardPrefab).GetComponent<IGameBoard>(),playArea, tileStatesRegister["Active"]);

        gameBoardController.GenerateBoard();

        foreach (Vector3 point in playArea)
        {
            gameBoardController.SetTileState(point, tileStatesRegister["Inactive"]);
        }

        playerGameObject = Instantiate<GameObject>(playerPrefab, 0.5f*Vector3.up, Quaternion.identity);
        player = playerGameObject.AddComponent<Player>();
        playerController = new PlayerController(player);
        TransitionToState(GameControllerState.IDLE);

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
                if (tileController.GetTileState().Equals(tileStatesRegister["Active"]))
                {
                    tileController.SetTileState(tileStatesRegister["Active"]);
                }
                else
                {
                    tileController.SetTileState(tileStatesRegister["Inactive"]);
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

            if (tileController.GetTileState().Equals(tileStatesRegister["Active"]))
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

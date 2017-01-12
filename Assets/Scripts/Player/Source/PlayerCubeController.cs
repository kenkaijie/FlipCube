using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IPlayerCubeStateName
{
	UNINIT,
	IDLE,
	TUMBLING,
	SPAWNING,
	DESPAWNING,
	TELEPORTING
}

public class PlayerCubeController : MonoBehaviour , IPlayerCubeController
{



	private Vector3 targetPlayerPosition;
	private Vector3 lastSuccessfulPlayerPosition;

	private EventCallback nextCallback;

	private IPlayerCubeState currentState;
	private IPlayerCubeState nextState;
	private Dictionary<IPlayerCubeStateName, IPlayerCubeState> playerStateMap = new Dictionary<IPlayerCubeStateName, IPlayerCubeState>();

	IEnumerator Start () {

		//generating all the states here

		playerStateMap.Add(IPlayerCubeStateName.IDLE,GetComponentInChildren<PlayerCubeStateIdle>());
		playerStateMap.Add(IPlayerCubeStateName.TUMBLING,GetComponentInChildren<PlayerCubeStateTumbling>());

		currentState = playerStateMap [IPlayerCubeStateName.IDLE];





	}

	private void Update () {
		currentState.UpdateState ();
	}

	private void FixedUpdate()
	{
		currentState.FixedUpdateState ();
	}

	private void OnStateTransition(EventCallbackError error)
	{
		if (error == EventCallbackError.NOERROR)
		{
			lastSuccessfulPlayerPosition = targetPlayerPosition;
			currentState = nextState;
		}

		nextCallback.Invoke (error);
	}

	public void StartTumbling(Vector3 position, EventCallback callback)
	{
		targetPlayerPosition = position;
		currentState.StartTumble (position, OnStateTransition);	
	}
	public void StartTeleporting (Vector3 position, EventCallback callback)
	{
		targetPlayerPosition = position;
		currentState.StartTeleport (position, OnStateTransition);	
	}

	// creation/deletion
	public void StartSpawn(Vector3 position, EventCallback callback)
	{
		targetPlayerPosition = position;
		currentState.StartSpawn (position, OnStateTransition);	
	}
	public void StartDespawn(EventCallback callback)
	{
		targetPlayerPosition = Vector3.zero;
		currentState.StartDespawn (OnStateTransition);	
	}

	public void SetNextState(IPlayerCubeStateName newState)
	{
		nextState = playerStateMap[newState];
	}

	// parameters
	public bool IsSpawned()
	{
		return true;
	}

	public Vector3 GetPlayerPosition()
	{
		return lastSuccessfulPlayerPosition;
	}
}

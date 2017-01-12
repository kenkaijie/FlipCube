using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IPlayerCubeStateName
{
    IDLE, TUMBLING, ERROR
}

public class PlayerCubeController : MonoBehaviour , IPlayerCubeController
{

	private Vector3 targetPlayerPosition;
	private Vector3 lastSuccessfulPlayerPosition;

	private EventCallback nextCallback;

	private IPlayerCubeState currentState;
	private IPlayerCubeState nextState;
	private Dictionary<IPlayerCubeStateName, IPlayerCubeState> playerStateMap = new Dictionary<IPlayerCubeStateName, IPlayerCubeState>();

	void Start () {

		//generating all the states here
		playerStateMap.Add(IPlayerCubeStateName.IDLE,GetComponentInChildren<PlayerCubeStateIdle>());
		playerStateMap.Add(IPlayerCubeStateName.TUMBLING,GetComponentInChildren<PlayerCubeStateTumbling>());

        currentState = playerStateMap[IPlayerCubeStateName.IDLE];

        StartTumble(Vector3.forward, OnDone);

	}

    private void OnDone(EventCallbackError error)
    {
    }

	private void Update () {
        TransitionToState(currentState.UpdateState ());
	}

	private void FixedUpdate()
	{
        TransitionToState(currentState.FixedUpdateState ());
	}


	public void StartTumble(Vector3 position, EventCallback callback)
	{
        targetPlayerPosition = position;
        TransitionToState(currentState.StartTumble (position));	
	}

	private void TransitionToState(StateTransition<IPlayerCubeState> stateInfo)
    {
        currentState = stateInfo.nextState;
        currentState.OnStateEntry(targetPlayerPosition);
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

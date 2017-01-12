using System;
using UnityEngine;

public class PlayerCubeStateIdle: MonoBehaviour, IPlayerCubeState
{
    private EventCallback lastEventCallback = null;

	public StateTransition<IPlayerCubeState> StartTumble (Vector3 direction)
	{
        return new StateTransition<IPlayerCubeState>(GetComponent<PlayerCubeStateTumbling>(), TransitionStatus.NO_ERROR);
    }


	public StateTransition<IPlayerCubeState> UpdateState ()
	{
        return new StateTransition<IPlayerCubeState>(this, TransitionStatus.NO_ERROR);
    }

	public StateTransition<IPlayerCubeState> FixedUpdateState()
	{
        return new StateTransition<IPlayerCubeState>(this, TransitionStatus.NO_ERROR);
    }

    public void OnStateEntry(Vector3 position)
    {
    
    }
}

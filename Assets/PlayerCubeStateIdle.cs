using UnityEngine;
using System;

public class PlayerCubeStateIdle: MonoBehaviour, IPlayerCubeState
{
	private IPlayerCubeController context;
	private EventCallback callback;

	void Start()
	{
		context = GetComponentInParent<IPlayerCubeController> ();

	}

	public void StartTumble (Vector3 direction, EventCallback callback)
	{
		
	}
	public void StartTeleport (Vector3 position, EventCallback callback)
	{
		callback.Invoke (EventCallbackError.ERROR_GENERAL);
	}
	// creation and deletion
	public void StartSpawn (Vector3 position, EventCallback callback)
	{
		callback.Invoke (EventCallbackError.ERROR_GENERAL);
	}
		
	public void StartDespawn (EventCallback callback)
	{
		callback.Invoke (EventCallbackError.ERROR_GENERAL);
	}

	public void UpdateState ()
	{
		// nothing to update in idle state
	}

	public void FixedUpdateState()
	{

	}

}

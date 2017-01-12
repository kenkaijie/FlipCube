using System;
using UnityEngine;

public interface IPlayerCubeState
{
	void StartTumble (Vector3 direction, EventCallback callback);
	void StartTeleport (Vector3 position, EventCallback callback);
	// creation and deletion
	void StartSpawn (Vector3 position, EventCallback callback);
	void StartDespawn (EventCallback callback);

	void UpdateState ();
	void FixedUpdateState ();
}

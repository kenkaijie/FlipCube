using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCubeController
{
	// movements
	void StartTumbling(Vector3 position, EventCallback callback);
	void StartTeleporting (Vector3 position, EventCallback callback);

	// creation/deletion
	void StartSpawn(Vector3 position, EventCallback callback);
	void StartDespawn(EventCallback callback);

	// parameters
	bool IsSpawned();
	Vector3 GetPlayerPosition();

	void SetNextState (IPlayerCubeStateName newState);

}
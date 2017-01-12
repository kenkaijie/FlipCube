using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player actor
public interface IPlayerCubeController
{
	// movements
	void StartTumble(Vector3 position, EventCallback callback);
	//void StartTeleporting (Vector3 position, EventCallback callback);

	// creation/deletion
	//void StartSpawn(Vector3 position, EventCallback callback);
	//void StartDespawn(EventCallback callback);

	// parameters
	bool IsSpawned();
	Vector3 GetPlayerPosition();

}
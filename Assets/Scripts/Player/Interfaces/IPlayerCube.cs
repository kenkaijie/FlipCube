using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCube
{
	// movements
	void StartTumbling (Vector3 direction, EventCallback callback);
	void StartTeleport (Vector3 position, EventCallback callback);
	// creation and deletion
	void StartSpawn (Vector3 position, EventCallback callback);
	void StartDespawn (EventCallback callback);

	//GetParams
	Vector3 GetPlayerPosition ();
	Quaternion GetPlayerRotation ();
}

using UnityEngine;

public class PlayerCubeStateTumbling: MonoBehaviour, IPlayerCubeState
{

	private IPlayerCubeController context;

	public float baseForceMagnitude;
	public Vector3 baseForceVector;

	// private variables for calubating
	private float rotationModifier = 0f;
	private Quaternion appliedRotation;
	private Vector3 appliedVector;

	private Vector3 targetPosition;
	private Quaternion targetRotation;

	private Vector3 forceVector;

	//
	private Rigidbody rb;

	private EventCallback lastEventCallback = null;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		context = GetComponentInParent<IPlayerCubeController> ();
	}

	public void StartTumble(Vector3 direction, EventCallback callback)
	{
		if (direction.Equals (Vector3.forward))
		{
			rotationModifier = 0f;
			appliedRotation = Quaternion.AngleAxis (90f, Vector3.right);
		} 
		else if (direction.Equals (Vector3.back))
		{
			rotationModifier = 180f;
			appliedVector = Vector3.back;
			appliedRotation = Quaternion.AngleAxis (-90f, Vector3.right);
		} 
		else if (direction.Equals (Vector3.left))
		{
			rotationModifier = 270f;
			appliedVector = Vector3.left;
			appliedRotation = Quaternion.AngleAxis (-90f, Vector3.forward);
		} 
		else if (direction.Equals (Vector3.right))
		{
			rotationModifier = 90f;
			appliedVector = Vector3.right;
			appliedRotation = Quaternion.AngleAxis (90f, Vector3.forward);
		}
		else
		{
			callback.Invoke (EventCallbackError.ERROR_GENERAL);
			return;
		}
		targetPosition = context.GetPlayerPosition() + appliedVector;
		targetRotation = rb.rotation * appliedRotation;

		forceVector = Quaternion.Euler (0f, rotationModifier, 0f) * baseForceVector;

		lastEventCallback = callback;
	}

	public void StartTeleport (Vector3 position, EventCallback callback)
	{
		callback.Invoke (EventCallbackError.ERROR_INCORRECT_PLAYER_STATE);
	}

	// creation and deletion
	public void StartSpawn (Vector3 position, EventCallback callback)
	{
		callback.Invoke (EventCallbackError.ERROR_INCORRECT_PLAYER_STATE);
	}

	public void StartDespawn (EventCallback callback)
	{
		callback.Invoke (EventCallbackError.ERROR_INCORRECT_PLAYER_STATE);
	}

	public void UpdateState()
	{

	}

	public void FixedUpdateState()
	{
		rb.AddForce(baseForceMagnitude * forceVector);
		if ((rb.position - targetPosition).sqrMagnitude <= 0.1)
		{

			FreezePlayer();

			rb.position = targetPosition;
			rb.rotation = targetRotation;

			Debug.Log("Done!");

			if (lastEventCallback != null)
			{
				lastEventCallback.Invoke(EventCallbackError.NOERROR);
				lastEventCallback = null;
			}

		}
	}

	private void FreezePlayer()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

}


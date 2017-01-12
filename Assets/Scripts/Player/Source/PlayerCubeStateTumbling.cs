using UnityEngine;

public class PlayerCubeStateTumbling: MonoBehaviour, IPlayerCubeState
{

    public Vector3 direction;
	public float baseForceMagnitude;
	public Vector3 baseForceVector;

	// private variables for calubating
	private float rotationModifier = 0f;
	private Quaternion appliedRotation;
	private Vector3 appliedVector;

	private Vector3 targetPosition;
	private Quaternion targetRotation = Quaternion.identity;

	private Vector3 forceVector;

	//
	private Rigidbody rb;

	private EventCallback lastEventCallback = null;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();

	}

    public void OnStateEntry(Vector3 direction)
    {
        if (direction.Equals(Vector3.forward))
        {
            rotationModifier = 0f;
            appliedRotation = Quaternion.AngleAxis(90f, Vector3.right);
        }
        else if (direction.Equals(Vector3.back))
        {
            rotationModifier = 180f;
            appliedVector = Vector3.back;
            appliedRotation = Quaternion.AngleAxis(-90f, Vector3.right);
        }
        else if (direction.Equals(Vector3.left))
        {
            rotationModifier = 270f;
            appliedVector = Vector3.left;
            appliedRotation = Quaternion.AngleAxis(-90f, Vector3.forward);
        }
        else if (direction.Equals(Vector3.right))
        {
            rotationModifier = 90f;
            appliedVector = Vector3.right;
            appliedRotation = Quaternion.AngleAxis(90f, Vector3.forward);
        }
        else
        {
            forceVector = Vector3.zero;
            return;
        }
        targetPosition = rb.position + appliedVector;
        targetRotation = rb.rotation * appliedRotation;

        forceVector = Quaternion.Euler(0f, rotationModifier, 0f) * baseForceVector;

    }

    public StateTransition<IPlayerCubeState> StartTumble(Vector3 direction)
	{
        return new StateTransition<IPlayerCubeState>(this, TransitionStatus.NO_ERROR);
    }


	public StateTransition<IPlayerCubeState> UpdateState()
	{
        return new StateTransition<IPlayerCubeState>(this, TransitionStatus.NO_ERROR);
    }

	public StateTransition<IPlayerCubeState> FixedUpdateState()
	{
        if (forceVector.Equals(Vector3.zero))
        {
            return new StateTransition<IPlayerCubeState>(GetComponent<PlayerCubeStateIdle>(), TransitionStatus.ERROR);
        }

		rb.AddForce(baseForceMagnitude * forceVector);
		if ((rb.position - targetPosition).sqrMagnitude <= 0.1)
		{

			FreezePlayer();

			rb.position = targetPosition;
			rb.rotation = targetRotation;

			if (lastEventCallback != null)
			{
				lastEventCallback.Invoke(EventCallbackError.NOERROR);
				lastEventCallback = null;
			}

            return new StateTransition<IPlayerCubeState>(GetComponent<PlayerCubeStateIdle>(), TransitionStatus.NO_ERROR);

        }

        return new StateTransition<IPlayerCubeState>(this, TransitionStatus.NO_ERROR);


    }

	private void FreezePlayer()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

}


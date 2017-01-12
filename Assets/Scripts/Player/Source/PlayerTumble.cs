using UnityEngine;

public class PlayerTumble : MonoBehaviour
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

    EventCallback callback;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartTumble(Vector3 direction, EventCallback cb)
    {
        this.direction = direction;
        callback = cb;
        PrepareActions();
    }

    private void PrepareActions()
    {
        if (direction.Equals(Vector3.forward))
        {
            rotationModifier = 0f;
            appliedRotation = Quaternion.identity;
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

    // TEST START
    private bool pressed = false;

    public void Update()
    {
        if (!pressed)
        {
            if (Input.GetButton("Up"))
            {
                pressed = true;
                StartTumble(Vector3.forward, OnFinish);
            }
            else if (Input.GetButton("Down"))
            {
                pressed = true;
                StartTumble(Vector3.back, OnFinish);
            }
            else if (Input.GetButton("Left"))
            {
                pressed = true;
                StartTumble(Vector3.left, OnFinish);
            }
            else if (Input.GetButton("Right"))
            {
                pressed = true;
                StartTumble(Vector3.right, OnFinish);
            }

        }
    }

    private void OnFinish(EventCallbackError er)
    {
        pressed = false;
    }

    // TEST START

    public void FixedUpdate()
    {
        if (!forceVector.Equals(Vector3.zero))
        {
            rb.AddForce(baseForceMagnitude * forceVector);
            if ((rb.position - targetPosition).sqrMagnitude <= 0.1)
            {

                FreezePlayer();

                rb.position = targetPosition;
                rb.rotation = targetRotation;

                if (callback != null)
                {
                    callback.Invoke(EventCallbackError.NOERROR);
                }

                forceVector = Vector3.zero;

            }

        }

    }

    private void FreezePlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

}


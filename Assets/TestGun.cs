using UnityEngine;

public class TestGun : MonoBehaviour
{
    public GameObject grappleBlock;
    public InputSystem_Actions inputActions;
    private HingeJoint2D hinge;
    private bool isHooked = false;
    public float pullSpeed = 2f;
    public float minRopeLength = 2f;
    private Rigidbody2D rb;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        hinge = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (inputActions.Player.Jump.WasPressedThisFrame() && !isHooked)
        {
            AttachHook();
        }

        if (isHooked && inputActions.Player.Jump.WasReleasedThisFrame())
        {
            ReleaseHook();
        }

        if (isHooked && inputActions.Player.Sprint.IsPressed())
        {
            PullHook();
        }
    }

    private void AttachHook()
    {
        isHooked = true;
        hinge.connectedBody = grappleBlock.GetComponent<Rigidbody2D>();
        hinge.enabled = true;

        rb.freezeRotation = false;

        // Rotate towards the point connected to
        Vector3 direction = grappleBlock.transform.position - transform.position;
        transform.up = direction;

        // Set the Anchor.y to the distance
        float distance = Vector2.Distance(transform.position, grappleBlock.transform.position);
        Vector2 anchorVector = new Vector2(0, distance);
        hinge.anchor = anchorVector;
    }

    private void ReleaseHook()
    {
        isHooked = false;
        hinge.enabled = false;
        hinge.connectedBody = null;

        transform.up = Vector3.up;
        rb.freezeRotation = true;
    }

    private void PullHook()
    {
        // Slowly decrease the hinge.anchor.y
        // TODO: Test put how its like if the pulling was exponential instead
        if (hinge.anchor.y >= minRopeLength)
        {
            hinge.anchor -= new Vector2(0, pullSpeed) * Time.deltaTime;
        }
    }
}

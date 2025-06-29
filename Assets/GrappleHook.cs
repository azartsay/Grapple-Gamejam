using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    private HingeJoint2D hinge;
    private bool isHooked = false;
    public float pullSpeed = 2f;
    private float currentPullSpeed;
    public float minRopeLength = 2f;
    private Rigidbody2D rb;
    public LayerMask grappableLayer;
    public GrappleRope rope;

    [HideInInspector] public Vector2 grapplePoint;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        hinge = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        currentPullSpeed = pullSpeed;
        rope.enabled = false;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (inputActions.Player.Grapple.WasPressedThisFrame() && !isHooked)
        {
            // Raycast from mouse pos to check if Grappable was hit
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, Mathf.Infinity, grappableLayer);
            if(hit.collider != null)
            {
                AttachHook(hit.collider.gameObject);
                rope.enabled = true;
            }
            
        }

        if (isHooked && inputActions.Player.Grapple.WasReleasedThisFrame())
        {
            ReleaseHook();
            currentPullSpeed = pullSpeed;
            rope.enabled = false;
        }

        if (isHooked && inputActions.Player.Sprint.IsPressed())
        {
            PullHook();
        }

        if(isHooked && inputActions.Player.Sprint.WasReleasedThisFrame())
        {
            currentPullSpeed = pullSpeed;
        }
    }

    private void AttachHook(GameObject grappable)
    {
        isHooked = true;
        hinge.connectedBody = grappable.GetComponent<Rigidbody2D>();
        hinge.enabled = true;
        grapplePoint = grappable.transform.position;

        rb.freezeRotation = false;

        // Rotate towards the point connected to
        Vector3 direction = grappable.transform.position - transform.position;
        transform.up = direction;

        // Set the Anchor.y to the distance
        float distance = Vector2.Distance(transform.position, grapplePoint);
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
            hinge.anchor -= new Vector2(0, currentPullSpeed) * Time.deltaTime;
        }
    }

    public Vector2 GetRopeLength()
    {
        return grapplePoint - (Vector2)transform.position;
    }
}

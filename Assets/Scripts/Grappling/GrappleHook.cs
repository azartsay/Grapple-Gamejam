using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GrappleHook : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    private HingeJoint2D hinge;
    public bool isHooked = false;
    public float pullSpeed = 2f;
    private float currentPullSpeed;
    public float minRopeLength = 2f;
    private Rigidbody2D rb;
    public LayerMask grappableLayer;
    public GrappleRope rope;
    public float groundCheckRadius;
    public LayerMask ground;
    public float grappleRange;
    public float unHookDelay = 0.2f;

    public float spinStrength = 10;

    private Vector2 moveInput;

    [SerializeField] private PauseManager pauseManager;

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
        
        if (pauseManager.isPaused) return;

        if (inputActions.Player.Grapple.WasPressedThisFrame() && !isHooked)
        {
            // Raycast player in direction of mouse for a range to look for grappable
            Vector2 direction = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grappleRange, grappableLayer);
            if(hit.collider != null)
            {
                AttachHook(hit.collider.gameObject);
                rope.enabled = true;
            }
            
        }

        if (inputActions.Player.Grapple.WasReleasedThisFrame())
        {
            ReleaseHook();
        }

        if (isHooked && inputActions.Player.Sprint.IsPressed())
        {
            PullHook();
        }

        if(isHooked && inputActions.Player.Sprint.WasReleasedThisFrame())
        {
            currentPullSpeed = pullSpeed;
        }

        // Checks if the player touches the ground -> releases hook
        if(Physics2D.OverlapCircle(transform.position, groundCheckRadius, ground))
        {
            ReleaseHook();
        }

        if (isHooked && inputActions.Player.Move.IsPressed()) {
            MoveWhileHooked();
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

    public void ReleaseHook()
    {
        if (!isHooked)
            return;
        
        hinge.enabled = false;
        hinge.connectedBody = null;

        transform.up = Vector3.up;
        rb.freezeRotation = true;

        currentPullSpeed = pullSpeed;
        rope.enabled = false;

        StartCoroutine(DelayedUnHook(unHookDelay));      
    }

    private void PullHook()
    {
        // Slowly decrease the hinge.anchor.y
        // TODO: Test put how its like if the pulling was exponential instead
        if (hinge.anchor.y >= minRopeLength)
        {
            hinge.autoConfigureConnectedAnchor = false;
            hinge.anchor -= new Vector2(0, currentPullSpeed) * Time.deltaTime;
        }
    }

    public Vector2 GetRopeLength()
    {
        return grapplePoint - (Vector2)transform.position;
    }

    public void MoveWhileHooked() {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        rb.AddTorque(moveInput.x * spinStrength);

    }

    private IEnumerator DelayedUnHook(float delay)
    {
        Debug.Log("Releasing after: " + delay);
        yield return new WaitForSeconds(delay);
        isHooked = false;
    }
}

using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        animator.SetBool("IsRunning", Mathf.Abs(movement.RB.linearVelocityX) >= 0.1f);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class windSound : MonoBehaviour
{
    private PlayerMovement movement;
    private AudioSource windSource;
    private float maxSpeed = 12f;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        windSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!movement.CanJump())
        {
            float speed = movement.RB.linearVelocity.magnitude;
            float normalized = Mathf.Clamp01(Mathf.Abs(speed) / maxSpeed);
            windSource.volume = normalized;
        }
        else
        {
            windSource.volume = 0;
        }
        
    }
}

using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] footsteps;
    private AudioSource footSource;
    public AudioSource hookSource;
    public PlayerMovement movement;

    private void Start()
    {
        footSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if(movement.CanJump())
        {
            int len = footsteps.Length;
            footSource.clip = footsteps[Random.Range(0, len)];
            footSource.Play();
        }
        
    }

    public void PlayHookSound()
    {
        hookSource.Play();
    }
}

using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] footsteps;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        int len = footsteps.Length;
        source.clip = footsteps[Random.Range(0, len)];
        source.Play();
    }
}

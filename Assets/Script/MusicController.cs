using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource; // Audio Source yang memutar musik
    private bool isPlaying = true; // Status musik

    public void ToggleMusic()
    {
        if (isPlaying)
        {
            musicSource.Pause(); // Pause musik
            isPlaying = false;
        }
        else
        {
            musicSource.Play(); // Mainkan musik
            isPlaying = true;
        }
    }
}

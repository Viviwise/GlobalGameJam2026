using UnityEngine;
namespace Script.Choice.Listener
{
    public class SoundListener : MonoBehaviour
    {
        public AudioSource audioSource;

        public void PlaySound()
        {
            if (audioSource != null)
                audioSource.Play();
        }
    }

}
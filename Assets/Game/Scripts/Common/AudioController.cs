using UnityEngine;

namespace SpaceInvaders
{
    public class AudioController : MonoBehaviour
    {
        public AudioSource SoundSource;

        [Space]
        public AudioClip PlayerLaserSound;
        public AudioClip EnemyLaserSound;
        public AudioClip ExplosionSound;
        public AudioClip GameOverSound;
        public AudioClip NewWaveSound;

        [Space]
        public AudioClip ButtonPressSound;
        public AudioClip ButtonReleaseSound;

        public void PlaySound(AudioClip clip, float volume = 1f)
        {
            SoundSource.PlayOneShot(clip, volume);
        }
    }
}
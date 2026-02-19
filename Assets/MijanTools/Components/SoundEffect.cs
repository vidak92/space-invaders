using MijanTools.Common;
using System;
using UnityEngine;

namespace MijanTools.Components
{
    [Serializable]
    public class SoundEffect
    {
        public float MinVolume = 0f;
        public float MaxVolume = 1f;
        public AudioClip[] Clips;

        private int _lastPlayedClipIndex;

        public void PlayRandomSound(AudioSource soundSource, float intensity)
        {
            var clip = Clips.GetRandomElement();
            if (clip != null)
            {
                PlayClip(soundSource, clip, intensity);
            }
        }

        public void PlayRandomSound(AudioSource soundSource)
        {
            PlayRandomSound(soundSource, 1f);
        }

        public void PlayNonRepeatingRandomSound(AudioSource soundSource, float intensity)
        {
            if (!Clips.IsNullOrEmpty())
            {
                _lastPlayedClipIndex = Clips.GetRandomIndexExcluding(_lastPlayedClipIndex);
                var clip = Clips[_lastPlayedClipIndex];
                PlayClip(soundSource, clip, intensity);
            }
        }

        public void PlayNonRepeatingRandomSound(AudioSource soundSource)
        {
            PlayNonRepeatingRandomSound(soundSource, 1f);
        }

        private void PlayClip(AudioSource soundSource, AudioClip clip, float intensity)
        {
            intensity = Mathf.Clamp01(intensity);
            var volume = Mathf.Lerp(MinVolume, MaxVolume, intensity);
            soundSource.PlayOneShot(clip, volume);
        }

        public void PlayFirstClipWithoutOneShot(AudioSource soundSource)
        {
            var volume = Mathf.Lerp(MinVolume, MaxVolume, 1f);
            soundSource.clip = Clips[0];
            soundSource.volume = volume;
            soundSource.Play();
        }
    }
}
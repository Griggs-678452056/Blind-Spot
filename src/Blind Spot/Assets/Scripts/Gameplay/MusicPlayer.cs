using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlindSpot
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip[] _musicClips;

        private int _currentClipIndex = -1;
        private bool _isStopped;

        private void Awake()
        {
            if (_musicSource == null)
            {
                _musicSource = GetComponent<AudioSource>();
            }

            _musicSource.playOnAwake = false;
            _musicSource.loop = false;
        }

        private void Start()
        {
            PlayRandomTrack();
        }

        private void Update()
        {
            if (_isStopped)
            {
                return;
            }

            if (!_musicSource.isPlaying)
            {
                PlayRandomTrack();
            }
        }

        private void PlayRandomTrack()
        {
            if (_musicClips == null || _musicClips.Length == 0)
            {
                return;
            }

            int newTrackIndex;

            if (_musicClips.Length == 1)
            {
                newTrackIndex = 0;
            }
            else
            {
                do
                {
                    newTrackIndex = Random.Range(0, _musicClips.Length);
                }
                while (newTrackIndex == _currentClipIndex);
            }

            _currentClipIndex = newTrackIndex;

            _musicSource.clip = _musicClips[_currentClipIndex];
            _musicSource.Play();
        }

        public void StopMusic()
        {
            _isStopped = true;
            _musicSource.Stop();
        }
    }
}

using System;
using UnityEngine;

// THIS WORKS AND IDK HOW PLS HALP
public class AudioManager : MonoBehaviour {
    [SerializeField] private Audio[] _audioArray;
    [SerializeField] private float masterVolume;

    private static AudioManager _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (Audio audio in _audioArray) {
            audio.Source = gameObject.AddComponent<AudioSource>();
            audio.Source.clip = audio.Clip;
            audio.Source.outputAudioMixerGroup = audio.AudioMixerGroup;

            audio.Source.volume = audio.Volume;
            audio.Source.loop = audio.Loop;
        }

        Play("BackgroundMusic");
    }

    public void Play(string name) {
        FindAudio(name).Source.Play();
    }

    public void PlayDelayed(string name, float delay) {
        if (delay < 0) {
            throw new ArgumentException("Param delay in PlayDelayed() cannot be negative");
        }

        FindAudio(name).Source.PlayDelayed(delay);
    }

    public void Stop(string name) {
        FindAudio(name).Source.Stop();
    }

    public void Pause(string name) {
        FindAudio(name).Source.Pause();
    }

    public AudioClip GetAudioClip(string name) {
        return FindAudio(name).Source.clip;
    }

    private Audio FindAudio(string name) {
        Audio audio = Array.Find(_audioArray, audio => audio.Name == name);

        if (audio == null) {
            throw new ArgumentException("Could not find Audio with name \"" + name + "\"");
        }

        return audio;
    }

    public static AudioManager Instance {
        get {
            // return instance if it exists
            if (_instance != null) {
                return _instance;
            }

            // look in scene for it
            _instance = FindObjectOfType<AudioManager>();

            // return it if found
            if (_instance != null) {
                return _instance;
            }

            // else create new one
            _instance = new GameObject(nameof(AudioManager), typeof(AudioManager)).AddComponent<AudioManager>();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<AudioManager>();
            return _instance;
        }
    }
}

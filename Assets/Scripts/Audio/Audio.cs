using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Audio {
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] [Range(0f, 1f)] private float _volume;
    [SerializeField] private bool _loop;

    private AudioSource _source;

    public AudioSource Source {
        get { return _source; }
        set { _source = value; }
    }

    public string Name {
        get { return _name; }
    }

    public AudioClip Clip {
        get { return _clip; }
    }

    public AudioMixerGroup AudioMixerGroup {
        get { return _audioMixerGroup; }
    }

    public float Volume {
        get { return _volume; }
    }

    public bool Loop {
        get { return _loop; }
    }
}

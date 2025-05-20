using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    private class Sound
    {
        public string Name;
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
    }

    [SerializeField] private List<Sound> _sounds;
    [SerializeField] private int _poolSize = 10;

    private Dictionary<string, Sound> _soundMap;
    private List<AudioSource> _audioSourcePool;
    private Transform _poolContainer;
    private float _masterVolume = 1f;
    [SerializeField] private AudioSource _loopingSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        DontDestroyOnLoad(gameObject);

        _soundMap = new Dictionary<string, Sound>();
        foreach (var sound in _sounds)
            _soundMap[sound.Name] = sound;

        CreateAudioSourcePool();
    }

    private void CreateAudioSourcePool()
    {
        _poolContainer = new GameObject("AudioSourcePool").transform;
        _poolContainer.parent = transform;

        _audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject go = new GameObject($"PooledAudioSource_{i}");
            go.transform.parent = _poolContainer;
            AudioSource src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 1f; 
            _audioSourcePool.Add(src);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var src in _audioSourcePool)
        {
            if (!src.isPlaying)
                return src;
        }

        GameObject go = new GameObject("ExtraAudioSource");
        go.transform.parent = _poolContainer;
        AudioSource newSource = go.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.spatialBlend = 1f;
        _audioSourcePool.Add(newSource);
        return newSource;
    }

    public void Play(string soundName)
    {
        PlayAtPosition(soundName, Camera.main.transform.position);
    }

    public void PlayAtPosition(string soundName, Vector3 position)
    {
        if (!_soundMap.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound '{soundName}' pas trouvé");
            return;
        }

        Sound sound = _soundMap[soundName];
        AudioSource src = GetAvailableAudioSource();
        src.clip = sound.Clip;
        src.volume = sound.Volume * _masterVolume;
        src.transform.position = position;
        src.Play();
    }

    public void SetVolume(float volume)
    {
        _masterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();

        Debug.Log("Volume réglé à : " + volume);
    }

    public float GetVolume()
    {
        return _masterVolume;
    }

    public void PlayLoop(string soundName, Transform followTarget = null)
    {
        if (!_soundMap.TryGetValue(soundName, out var sound))
        {
            Debug.LogWarning($"Sound '{soundName}' pas trouvé");
            return;
        }

        if (_loopingSource == null)
        {
            GameObject go = new GameObject("LoopingAudioSource");
            _loopingSource = go.AddComponent<AudioSource>();
        }

		_loopingSource.spatialBlend = 1f;
		_loopingSource.rolloffMode = AudioRolloffMode.Linear;
		_loopingSource.maxDistance = 20f;

		if (followTarget != null)
        {
            _loopingSource.transform.SetParent(followTarget);
            _loopingSource.transform.localPosition = Vector3.zero;
        }

        if (_loopingSource.isPlaying && _loopingSource.clip == sound.Clip)
            return;

        _loopingSource.clip = sound.Clip;
        _loopingSource.volume = sound.Volume * _masterVolume;
        _loopingSource.loop = true;
        _loopingSource.Play();
    }

    public void StopLoop()
    {
        if (_loopingSource != null && _loopingSource.isPlaying)
            _loopingSource.Stop();
    }
}

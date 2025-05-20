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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
        src.volume = sound.Volume;
        src.transform.position = position;
        src.Play();
    }
}

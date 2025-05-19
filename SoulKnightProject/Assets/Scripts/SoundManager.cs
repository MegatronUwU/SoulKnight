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

    private Dictionary<string, Sound> _soundMap;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _soundMap = new Dictionary<string, Sound>();

        foreach (var sound in _sounds)
            _soundMap[sound.Name] = sound;
    }

    public void Play(string soundName)
    {
        if (!_soundMap.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound '{soundName}' pas trouvé");
            return;
        }

        Sound sound = _soundMap[soundName];
        _audioSource.PlayOneShot(sound.Clip, sound.Volume);
    }
}
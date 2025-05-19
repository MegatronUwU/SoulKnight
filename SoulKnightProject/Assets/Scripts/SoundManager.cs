using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private List<Sound> sounds;

    private Dictionary<string, Sound> soundMap;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        soundMap = new Dictionary<string, Sound>();

        foreach (var sound in sounds)
            soundMap[sound.name] = sound;
    }

    public void Play(string soundName)
    {
        if (!soundMap.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound '{soundName}' pas trouvé");
            return;
        }

        Sound sound = soundMap[soundName];
        audioSource.PlayOneShot(sound.clip, sound.volume);
    }
}
using System;
using UnityEngine;

[Serializable]
public class MusicByIdDictionary : SerializedDictionary<MusicId, AudioClip> { }
[Serializable]
public class SoundByIdDictionary : SerializedDictionary<SoundId, AudioClip> { }

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private MusicByIdDictionary musicById;
    [SerializeField] private SoundByIdDictionary soundById;

    public void Initialize()
    {
        ToggleAudioSource();
    }

    public void ToggleAudioSource()
    {
        musicSource.enabled = GameData.Instance.IsMusicEnabled;
        soundSource.enabled = GameData.Instance.IsSoundEnabled;
    }

    public void PlayMusic(MusicId id)
    {
        musicSource.clip = musicById[id];
        musicSource.Play();
    }

    public void PlaySound(SoundId id)
    {
        if (soundSource.loop)
        {
            soundSource.loop = false;
            soundSource.Stop();
        }

        soundSource.PlayOneShot(soundById[id]);
    }

    public void PlaySound(SoundId id, float duration)
    {
        soundSource.loop = true;
        soundSource.clip = soundById[id];
        soundSource.Play();

        ScheduleUtils.DelayTask(duration, () =>
        {
            if (soundSource.loop)
            {
                soundSource.loop = false;
                soundSource.Stop();
            }
        });
    }
}

public enum MusicId
{
    [HideInInspector]
    None = -1,
    Home = 0,
    Game = 1,
}

public enum SoundId
{
    [HideInInspector]
    None = -1,
    Click = 0,
}

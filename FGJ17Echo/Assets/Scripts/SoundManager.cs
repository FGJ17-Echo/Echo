using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private List<AudioSource> _audioSourcePool;
    
    [SerializeField]
    private float _minIntervalBetweenSameSounds = 0.3f;

    private List<AudioClip> _neutralPings;

    private List<AudioClip> _dangerPings;

    private List<AudioClip> _damageClips;

    private List<AudioClip> _bonusPings;

    private List<AudioClip> _positiveClips;

    public enum SoundEffect
    {
        Undefined,
        NeutralPing,
        DangerPing,
        BonusPing,
        Damage,
        PositiveFeedback
    }

    private Dictionary<SoundEffect, float> _soundTimes = new Dictionary<SoundEffect, float>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(SoundEffect effect, Vector3 position)
    {
        var lastTime = _soundTimes.ContainsKey(effect) ? _soundTimes[effect] : -100;

        if (lastTime + _minIntervalBetweenSameSounds < Time.time) return;

        AudioSource source = _audioSourcePool.FirstOrDefault(x => !x.isPlaying);
        
        if (source != null)
        {
            source.clip = GetClip(effect);
            source.transform.position = position;
            source.Play();
        }
    }

    AudioClip GetClip(SoundEffect effect)
    {
        List<AudioClip> list = null;

        switch (effect)
        {
            case SoundEffect.BonusPing:
                list = _bonusPings; break;
            case SoundEffect.Damage:
                list = _damageClips; break;
            case SoundEffect.DangerPing:
                list = _dangerPings; break;
            case SoundEffect.NeutralPing:
                list = _neutralPings; break;
            case SoundEffect.PositiveFeedback:
                list = _positiveClips; break;
        }

        if (list == null) return null;

        return list[Random.Range(0, list.Count - 1)];
    }
}

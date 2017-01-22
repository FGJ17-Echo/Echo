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

    [SerializeField]
    private List<AudioClip> _neutralPings;

    [SerializeField]
    private List<AudioClip> _dangerPings;

    [SerializeField]
    private List<AudioClip> _damageClips;

    [SerializeField]
    private List<AudioClip> _bonusPings;

    [SerializeField]
    private List<AudioClip> _positiveClips;

    [SerializeField]
    private List<AudioClip> _pickupClips;

    [SerializeField]
    private List<AudioClip> _waspPings;

    public enum SoundEffect
    {
        Undefined,
        NeutralPing,
        DangerPing,
        BonusPing,
        Damage,
        PositiveFeedback,
        Pickup,
        WaspPing
    }

    private Dictionary<SoundEffect, float> _soundTimes = new Dictionary<SoundEffect, float>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(SoundEffect effect, Vector3 position)
    {
        var lastTime = _soundTimes.ContainsKey(effect) ? _soundTimes[effect] : -100;

        if (lastTime + _minIntervalBetweenSameSounds > Time.time) return;

        AudioSource source = _audioSourcePool.FirstOrDefault(x => !x.isPlaying);
        
        if (source != null)
        {
            var clip = GetClip(effect);

            if (clip != null)
            {
                source.clip = GetClip(effect);
                source.transform.position = position;
                source.Play();

                _soundTimes[effect] = Time.time;
            }
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
            case SoundEffect.Pickup:
                list = _pickupClips; break;
            case SoundEffect.WaspPing:
                list = _waspPings; break;
        }

        if (list == null || list.Count == 0) return null;

        var rnd = Mathf.Clamp(Random.Range(0, list.Count - 2), 0, list.Count - 1);

        return list[rnd];
    }
}

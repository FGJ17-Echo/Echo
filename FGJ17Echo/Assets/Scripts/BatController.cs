using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class BatController : MonoBehaviour, IDamageReceiver
{
    [SerializeField]
    private float _maxEnergy = 100;
    public float MaxEnergy
    {
        get
        {
            return _maxEnergy;
        }
    }

    public bool HasTheKey { get; private set; }

    [SerializeField]
    private EchoLocator _echoLocator;

    [SerializeField]
    private float _minVelocityForDamage = 1f;
    [SerializeField]
    private float _damagePerVelocity = 1f;
    [SerializeField]
    private float _maxDamageFromCollision = 10f;
    [SerializeField]
    private AudioSource _eatSound;

    [SerializeField]
    private float _deadzone = 0.08f;

    [SerializeField]
    private BatVision _batVision;

    [SerializeField]
    private ParticleSystem _eatParticles;

    public static event System.Action<EnergyChangedEventArgs> EnergyChanged;
   
    public float CurrentEnergy { get; private set; }

    public bool IsDead { get; private set; }

    private MoveController _moveController;

    [SerializeField]
    private LayerMask _collisionDamageLayerMask;

    [SerializeField]
    private LayerMask _collectableLayerMask;

    [SerializeField]
    private Transform _respawnPosition;

    void Awake ()
    {
        _moveController = GetComponent<MoveController>();
	}

    void Start()
    {
        Init();
    }

    public void Init()
    {
        CurrentEnergy = _maxEnergy;
        if (EnergyChanged != null)
        {
            EnergyChanged(new EnergyChangedEventArgs()
            {
                Bat = this,
                Delta = 0,
                CanDie = false,
                RemainingEnergy = CurrentEnergy,
                Source = null
            });
        }

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        var bounceSize = LeanTween.scale(gameObject, Vector3.one * 1.2f, 2);
        bounceSize.setEaseInOutBounce();
        bounceSize.setOnComplete(() => {
            IsDead = false;
            LeanTween.scale(gameObject, Vector3.one, 0.5f);
        });
    }

    public void UseEnegy(float amount, bool canDie = false, object source = null)
    {
        if (amount > 0)
        {
            ChangeEnergy(-amount, canDie, source);

            if (canDie) SoundManager.Instance.PlaySound(SoundManager.SoundEffect.Damage, transform.position);
        }
    }

    public void GainEnegy(float amount, bool canDie = false, object source = null)
    {
        if (amount > 0)
        {
            ChangeEnergy(amount, canDie, source);
        }
    }

    internal void CollectKey()
    {
        HasTheKey = true;
     }

    internal void DropKey()
    {
        HasTheKey = false;
    }

    public void Die()
    {
        IsDead = true;
        transform.position = _respawnPosition.position;
        Init();
    }

    private void ChangeEnergy(float amount, bool canDie = false, object source = null)
    {
        var newAmount = Mathf.Clamp(CurrentEnergy + amount, 0, _maxEnergy);

        if (newAmount != CurrentEnergy)
        {
            var delta = newAmount - CurrentEnergy;
            CurrentEnergy = newAmount;

            if (CurrentEnergy <= 0) Die();

            if (EnergyChanged != null)
            {
                EnergyChanged(new EnergyChangedEventArgs() {
                    Bat = this,
                    Delta = delta,
                    CanDie = canDie,
                    RemainingEnergy = CurrentEnergy,
                    Source = source
                });
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("Echo")) Echo();
    }

    void FixedUpdate ()
    {
        var xMove = Input.GetAxis("Horizontal");
        var yMove = Input.GetAxis("Vertical");

        if (Mathf.Abs(xMove) < _deadzone && Mathf.Abs(yMove) < _deadzone)
        {
            xMove = 0;
            yMove = 0;
        }

        if (!IsDead) _moveController.Move(new Vector2(xMove, yMove));        
	}

    public void Echo()
    {
        if (CurrentEnergy >= _echoLocator.EnergyUsage)
        {
            _echoLocator.Echo();
            UseEnegy(_echoLocator.EnergyUsage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var layermask = _collisionDamageLayerMask;
        var layer = collision.gameObject.layer;

        if (layermask == (layermask | (1 << layer)))
        {
            var magnitudeSqr = collision.relativeVelocity.sqrMagnitude;

            var excessSpeed = magnitudeSqr - _minVelocityForDamage;
            if (excessSpeed > 0)
            {
                var damage = Mathf.Clamp(excessSpeed * _damagePerVelocity, 0, _maxDamageFromCollision);
                UseEnegy(damage, true, collision);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var layermask = _collectableLayerMask;
        var layer = collider.gameObject.layer;

        if (LayerMask.LayerToName(collider.gameObject.layer) == "Checkpoint")
        {
            if (_respawnPosition != collider.gameObject.transform)
            {
                _respawnPosition = collider.gameObject.transform;
                SoundManager.Instance.PlaySound(SoundManager.SoundEffect.PositiveFeedback, _respawnPosition.position);
            }
        }
        else if (layermask == (layermask | (1 << layer)))
        {
            var go = collider.attachedRigidbody ? collider.attachedRigidbody.gameObject : collider.gameObject;

            var energySource = go.GetComponent<CollectableEnergySource>();

            if (energySource != null)
            {
                var energy = energySource.Collect(this);

                if (energy > 0)
                {
                    GainEnegy(energy);

                    _eatParticles.Play();
                    _eatSound.Play();
                }
            }
        }
    }

    public void EnhanceVision()
    {
        _batVision.IncreaseVision();
    }

    public void TakeDamage(float amount, object source)
    {
        UseEnegy(amount, true, source);
    }

    public class EnergyChangedEventArgs
    {
        public BatController Bat { get; set; }
        public float RemainingEnergy { get; set; }
        public float Delta { get; set; }
        public bool CanDie { get; set; }
        public object Source { get; set; }
    }
}

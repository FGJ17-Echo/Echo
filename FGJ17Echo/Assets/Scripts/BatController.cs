using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField]
    private float _maxEnergy = 100;
    private float MaxEnergy
    {
        get
        {
            return _maxEnergy;
        }
    }
    [SerializeField]
	private float moveSpeed = 15.0f;
	[SerializeField]
	private float maxVelocity = 100f;
    [SerializeField]
    private EchoLocator _echoLocator;

	private Rigidbody2D rb2d;

    [SerializeField]
    private float _deadzone = 0.08f;

    public static event System.Action<EnergyChangedEventArgs> EnergyChanged;
   
    public float CurrentEnergy { get; private set; }

	void Awake ()
    {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
	}

    void Start()
    {
        Init();
    }

    public void Init()
    {
        CurrentEnergy = _maxEnergy;
    }

    public void UseEnegy(float amount, bool canDie = false, object source = null)
    {
        if (amount > 0)
        {
            ChangeEnergy(-amount, canDie, source);
        }
    }

    public void GainEnegy(float amount, bool canDie = false, object source = null)
    {
        if (amount > 0)
        {
            ChangeEnergy(amount, canDie, source);
        }
    }

    private void ChangeEnergy(float amount, bool canDie = false, object source = null)
    {
        var newAmount = Mathf.Clamp(CurrentEnergy + amount, 0, _maxEnergy);

        if (newAmount != CurrentEnergy)
        {
            var delta = newAmount - CurrentEnergy;
            CurrentEnergy = newAmount;

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
        if (Input.GetButtonUp("Jump")) Echo();
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

        Move(new Vector2(xMove, yMove));        
	}

    public void Echo()
    {
        if (CurrentEnergy >= _echoLocator.EnergyUsage)
        {
            _echoLocator.Echo();
            UseEnegy(_echoLocator.EnergyUsage);
        }
    }

    public void Move(Vector2 direction)
    {
        if (rb2d.velocity.magnitude < maxVelocity)
        {
            rb2d.AddForce(Vector2.right * direction.x * moveSpeed * Time.deltaTime);
            rb2d.AddForce(Vector2.up * direction.y * moveSpeed * Time.deltaTime);
        }
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

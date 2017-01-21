using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _fill;

    [SerializeField]
    private Image _flashImage;

    [SerializeField]
    private Color _neutralFlashColor = Color.white;

    [SerializeField]
    private Color _positiveFlashColor = Color.blue;

    [SerializeField]
    private Color _negativeFlashColor = Color.red;

    [SerializeField]
    private float _flashInTime = 0.25f;

    private Color _flashColor;

    private float _remainingFlashTime = 0;

    private void OnEnable()
    {
        BatController.EnergyChanged += BatController_EnergyChanged;
    }

    private void OnDisable()
    {
        BatController.EnergyChanged -= BatController_EnergyChanged;
    }

    void Start()
    {
        _fill.anchorMax = new Vector2(1, 1);
    }

    void Update()
    {
        UpdateFlash();
    }

    void UpdateFlash()
    {
        if (_remainingFlashTime <= 0)
        {
            _flashImage.CrossFadeColor(_neutralFlashColor, _flashInTime, false, false);
        }
        else
        {
            _remainingFlashTime -= Time.deltaTime;

            _flashImage.CrossFadeColor(_flashColor, _flashInTime, false, false);
        }
    }

    private void BatController_EnergyChanged(BatController.EnergyChangedEventArgs args)
    {
        var fillAmount = args.RemainingEnergy / args.Bat.MaxEnergy;

        _fill.anchorMax = new Vector2(fillAmount, 1);

        if (fillAmount <= 0)
        {
            _fill.gameObject.SetActive(false);
        }
        else if (!_fill.gameObject.activeInHierarchy)
        {
            _fill.gameObject.SetActive(true);
        }

        if (args.Delta > 0)
        {
            FlashPositive();
        }
        else if (args.Delta < 0)
        {
            if (args.CanDie) FlashNegativeSevere();
            else FlashNegative();
        }
    }

    public void FlashPositive()
    {
        _flashColor = _positiveFlashColor;
        _remainingFlashTime = 0.5f;
    }

    public void FlashNegative()
    {
        _flashColor = _negativeFlashColor;
        _remainingFlashTime = 0.25f;
    }

    public void FlashNegativeSevere()
    {
        _flashColor = _negativeFlashColor;
        _remainingFlashTime = 1f;
    }
}

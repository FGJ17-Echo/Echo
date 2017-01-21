using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _fill;

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
    }
}

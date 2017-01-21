using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatVision : MonoBehaviour
{
    [SerializeField]
    private float _minVision = 1;

    [SerializeField]
    private float _maxVision = 1.2f;

    [SerializeField]
    private float _visionIncreaseTime = 20f;

    [SerializeField]
    private float _visionIncreaseTransitionTime = 0.5f;
    
    public bool IsEnhanced { get; set; }

    public void IncreaseVision()
    {
        if (!IsEnhanced)
        {
            Debug.Log("Enhanced!");
            IsEnhanced = true;

            var tween = LeanTween.scale(gameObject, Vector3.one * _maxVision, _visionIncreaseTransitionTime);
            tween.setEaseOutElastic();

            var outTween = LeanTween.scale(gameObject, Vector3.one * _minVision, _visionIncreaseTransitionTime);
            outTween.setEaseInQuad();
            outTween.delay = _visionIncreaseTime;
            outTween.setOnComplete(() => {
                IsEnhanced = false;
            });
        }
    }
}

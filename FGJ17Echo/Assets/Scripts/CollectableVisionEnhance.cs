using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableVisionEnhance : CollectableEnergySource
{
    public override float Collect(BatController bat)
    {
        bat.EnhanceVision();
        return base.Collect(bat);
    }
}

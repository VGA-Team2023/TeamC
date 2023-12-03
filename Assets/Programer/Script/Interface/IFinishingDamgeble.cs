using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFinishingDamgeble 
{
    void StartFinishing();

    void StopFinishing();

    void EndFinishing(MagickType attackHitTyp);

}

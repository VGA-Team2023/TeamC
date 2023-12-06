using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopCall : MonoBehaviour
{
    private float _time = 0;

    IEnumerator _hitStopCall;

    void Start()
    {

    }


    public void HitStopCalld(float time)
    {
        if (_hitStopCall != HitStop(time))
        {
            _hitStopCall = HitStop(time);
        }

        GameManager.Instance.SlowManager.OnOffSlow(true);
        StartCoroutine(_hitStopCall);
    }

    public IEnumerator HitStop(float time)
    {
        yield return new WaitForSeconds(time);

        GameManager.Instance.SlowManager.OnOffSlow(false);
    }

}

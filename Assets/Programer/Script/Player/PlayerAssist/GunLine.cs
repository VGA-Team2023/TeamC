using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private Transform _firstPos;

    [SerializeField] private float _time = 0.2f;

    public void ReflectBulletLine(Transform[] enemys)
    {
        //StartCoroutine(SetLine(enemys));
    }

    public void BulletLine(Transform player)
    {
       // StartCoroutine(Set(player));
    }


    private IEnumerator Set(Transform player)
    {
        _lineRenderer.positionCount = 2;
        Vector3 dir = player.forward;
        dir.y = 0;

        _lineRenderer.SetPosition(0, _firstPos.position);
        _lineRenderer.SetPosition(1, _firstPos.position + dir * 30);
        yield return new WaitForSeconds(_time);
        _lineRenderer.positionCount = 0;
    }

    private IEnumerator SetLine(Transform[] enemys)
    {
        _lineRenderer.positionCount =2;
        for (int i = 0; i < enemys.Length; i++)
        {
            if (i == 0)
            {
                _lineRenderer.SetPosition(0, _firstPos.position);
                _lineRenderer.SetPosition(1, enemys[i].position);
            }
            else
            {
                _lineRenderer.SetPosition(0, enemys[i - 1].position);
                _lineRenderer.SetPosition(1, enemys[i].position);
            }
            yield return new WaitForSeconds(_time);
        }


        _lineRenderer.positionCount = 0;
    }


}

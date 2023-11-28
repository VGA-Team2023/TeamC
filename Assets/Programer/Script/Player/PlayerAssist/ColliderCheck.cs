using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColliderCheck
{

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public Collider[] Search(Vector3 offSet, Vector3 size, LayerMask layer)
    {
        var posX = _playerControl.PlayerT.position.x + offSet.x;
        var posY = _playerControl.PlayerT.position.y + offSet.y;
        var posz = _playerControl.PlayerT.position.z + offSet.z;

        Quaternion r = _playerControl.PlayerT.rotation;
        r.x = 0;
        r.z = 0;

        var d = Physics.OverlapBox(new Vector3(posX, posY, posz), size, r, layer);
        Debug.Log("õ“G:"+d.Length);
        Debug.Log("ƒŒƒCƒ„[:"+layer);
        foreach (Collider c in d)
        {
            Debug.Log("–¼‘O:" + c.gameObject.name);
        }

        return d;
    }

    public Transform[] EnemySearch(SearchType searchType, Vector3 offSet, Vector3 size, LayerMask layer)
    {
        var hits = Search(offSet, size, layer);

        if (hits.Length == 0)
        {
            Transform[] t = new Transform[0];
            return t;
        }


        if (searchType == SearchType.AllEnemy)
        {
            Transform[] t = new Transform[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                t[i] = hits[i].transform;
            }
            return t;
        }
        else
        {
            float minEnemy = Vector3.Distance(hits[0].transform.position, _playerControl.PlayerT.position);
            GameObject nearEnemy = hits[0].gameObject;

            for (int i = 1; i < hits.Length; i++)
            {
                float dis = Vector3.Distance(hits[i].transform.position, _playerControl.PlayerT.position);
                if (dis < minEnemy)
                {
                    minEnemy = dis;
                    nearEnemy = hits[i].gameObject;
                }
            }

            Transform[] t = new Transform[1];
            t[0] = nearEnemy.transform;
            return t;
        }
    }
}



public enum SearchType
{
    /// <summary>”ÍˆÍ“à‚Ì‘S‚Ä‚Ì“G</summary>
    AllEnemy,

    /// <summary>ˆê”Ô‹——£‚Ì‹ß‚¢“G</summary>
    NearlestEnemy,
}
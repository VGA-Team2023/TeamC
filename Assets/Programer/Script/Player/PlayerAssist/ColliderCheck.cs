using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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

        Quaternion r = Quaternion.Euler(0, _playerControl.PlayerT.eulerAngles.y, 0);

        //Quaternion r = _playerControl.PlayerT.rotation;
        //r.x = 0;
        //r.z = 0;

        var d = Physics.OverlapBox(new Vector3(posX, posY, posz), size, r, layer);
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
            List<Collider> colliders = new List<Collider>();

            foreach (Collider collider in hits)
            {
                // カメラに写っているかを判断する
                if (IsVisibleToCamera(collider))
                {
                    colliders.Add(collider);
                }
            }

            Transform[] t = new Transform[colliders.Count];


            for (int i = 0; i < colliders.Count; i++)
            {
                t[i] = colliders[i].transform;
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


        bool IsVisibleToCamera(Collider collider)
        {
            // カメラの視錘台（View Frustum）で判定
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

            // オブジェクトの境界ボックスがカメラの視錘台と交差しているかをチェック
            return GeometryUtility.TestPlanesAABB(planes, collider.bounds);
        }



    }
}



public enum SearchType
{
    /// <summary>範囲内の全ての敵</summary>
    AllEnemy,

    /// <summary>一番距離の近い敵</summary>
    NearlestEnemy,
}
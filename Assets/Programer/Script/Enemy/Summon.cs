using UnityEngine;


public class Summon : MonoBehaviour
{
    [SerializeField, Tooltip("召喚したいPrefab")]
    GameObject _summonPrefab;
    [SerializeField, Tooltip("召喚する敵のY座標")]
    float _summonPositionY;

    public void EnemyCreate()
    {
        Instantiate(_summonPrefab, new Vector3(transform.position.x, transform.position.y + _summonPositionY, transform.position.z), Quaternion.identity);
    }
}

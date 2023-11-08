using UnityEngine;


public class Summon : MonoBehaviour
{
    [SerializeField, Tooltip("¢Š«‚µ‚½‚¢Prefab")]
    GameObject _summonPrefab;
    [SerializeField, Tooltip("¢Š«‚·‚é“G‚ÌYÀ•W")]
    float _summonPositionY;

    public void EnemyCreate()
    {
        Instantiate(_summonPrefab, new Vector3(transform.position.x, transform.position.y + _summonPositionY, transform.position.z), Quaternion.identity);
    }
}

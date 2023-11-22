using UnityEngine;

public class MAEDoll : MonoBehaviour
{
    [SerializeField, Tooltip("機能が付いたエネミー")]
    GameObject _enemy;
    public void AnimationEnd()
    {
        gameObject.SetActive(false);
        _enemy.SetActive(true);
    }
}

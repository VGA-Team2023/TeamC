using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReseltCurslActive : MonoBehaviour
{
    [Header("キーボード操作かどうか")]
    [SerializeField] private bool _isKeyBordPlay = false;
    void Start()
    {
        if (_isKeyBordPlay)
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }


}

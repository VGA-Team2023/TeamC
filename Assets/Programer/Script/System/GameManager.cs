using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

   [SerializeField] private TimeControl _timeControl;

    private static GameManager instance;

    public static GameManager Instance => instance;

    public TimeControl TimeControl => _timeControl;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _timeControl.Init(this);

    }


}

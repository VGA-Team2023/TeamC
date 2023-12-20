using Unity.VisualScripting;
using UnityEngine;

public class OptionValueRecorder : MonoBehaviour
{
    private static OptionValueRecorder _instance;

    public float CameraSensitivity;
    public static OptionValueRecorder Instance
    {
        get
        {
            if(_instance == null )
            {
                _instance =  FindObjectOfType<OptionValueRecorder>();
            }
            return _instance; 
        }
    }
    private void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }
}

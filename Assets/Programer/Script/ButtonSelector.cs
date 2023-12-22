using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField]
    Button _leftButton;

    [SerializeField]
    Button _rightButton;

    public float InputValue
    {
        set
        {
            if(value > 0)
            {
                _rightButton.Select();
            }
            else if(value < 0)
            {
                _leftButton.Select();
            }
        }
    }

    private void OnEnable()
    {
        _leftButton.Select();
    }

    private void Update()
    {
        InputValue = Input.GetAxisRaw("Horizontal");
    }
}

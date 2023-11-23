using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TstWave : MonoBehaviour
{
    [Header("Wave1‚Ì“G")]
    [SerializeField] private List<GameObject> _wave1Enemys = new List<GameObject>();

    [Header("Wave2‚Ì“G")]
    [SerializeField] private List<GameObject> _wave1Enemys2 = new List<GameObject>();

    [Header("Wave3‚Ì“G")]
    [SerializeField] private List<GameObject> _wave1Enemys3 = new List<GameObject>();

    [Header("ŽŸ‚ÌScene–¼‘O")]
    [SerializeField] private string _name;

    private int _wave1 = 0;
    private int _wave2 = 0;
    private int _wave3 = 0;

    private int _waveDown1 = 0;
    private int _waveDown2 = 0;
    private int _waveDown3 = 0;

    private bool _isWave1 = true;
    private bool _isWave2 = false;
    private bool _isWave3 = false;

    private void Awake()
    {
        _wave1Enemys.ForEach(i => i.SetActive(true));

        _wave1 = _wave1Enemys.Count;
        _wave2 = _wave1Enemys2.Count;
        _wave3 = _wave1Enemys3.Count;
    }


    public void DesEnemy()
    {
        if (_isWave3)
        {
            _waveDown3++;

            if (_wave3 == _waveDown3)
            {
                SceneManager.LoadScene(_name);
            }
        }
        else if (_isWave2)
        {
            _waveDown2++;

            if (_wave2 == _waveDown2)
            {
                _wave1Enemys3.ForEach(i => i.SetActive(true)); 
                _isWave3 = true;
            }
           
        }
        else
        {
            _waveDown1++;

            if (_wave1 == _waveDown1)
            {
                _wave1Enemys2.ForEach(i => i.SetActive(true));
                _isWave2 = true;
            }

            
        }


    }

}

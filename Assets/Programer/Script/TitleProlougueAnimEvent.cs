using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleProlougueAnimEvent : MonoBehaviour
{
    [SerializeField] private TitleProlougue _titleProlougue;


    public void EndFadeOut()
    {
    _titleProlougue.EndFadeOut();
    }

    public void ProlougueShow()
    {
    _titleProlougue.ProlougueShow();
    }


}

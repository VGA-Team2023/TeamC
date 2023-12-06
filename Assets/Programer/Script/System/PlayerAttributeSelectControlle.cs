using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>選択したPlayerの属性を保存する処理を行うメソッド</summary>
/// <param name="isEnumNumber">属性のenumの代わりとなる数値(０は氷１は草)</param>
public class PlayerAttributeSelectControlle : MonoBehaviour
{
    public void PlayerAttributeSelect(int isEnumNumber)
    {
        if (isEnumNumber > -1 && isEnumNumber < 2)
        {
            GameManager.Instance.PlayerAttributeChange((PlayerAttribute)isEnumNumber);
        }
        else
        {
            //エラーを出す
            Debug.LogError("下記を呼んだうえで0 ～ 1までの数字を入れてください\n" +
                " 氷属性は 0   草属性は 1 ");
        }
    }
}

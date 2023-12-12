using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WalkTutorialEnterBox : MonoBehaviour
{
    private TutorialMissionWalk _walk;
    public void Set(TutorialMissionWalk walk)
    {
        _walk = walk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _walk.End();
        }
    }

}

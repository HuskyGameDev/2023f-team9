using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = new Vector3(0, GameManager.Instance.runnerWinHeight);
    }

    void ChangeHeight(float height)
    {
        GameManager.Instance.runnerWinHeight = height;
    }
}

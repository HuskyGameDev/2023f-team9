using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = this.gameObject.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.y >= GameManager.Instance.runnerWinHeight)
        {
            GameManager.Instance.GameOver(true);
        }
    }
}

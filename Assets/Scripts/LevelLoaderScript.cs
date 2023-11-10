using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;

    public void loadMainScreen()
    {
        StartCoroutine(LoadMainScreen());
    }
    public void loadEndScreen()
    {
        StartCoroutine(LoadEndScreen());
    }

    IEnumerator LoadEndScreen()
    {
        transition.SetTrigger("EndStage");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("End Screen");
    }

    IEnumerator LoadMainScreen()
    {
        transition.SetTrigger("EndStage");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("MainGame");
    }
}

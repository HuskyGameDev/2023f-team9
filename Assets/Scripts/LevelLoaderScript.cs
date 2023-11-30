using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;
    
    public void loadStartScreen()
    {
        StartCoroutine(LoadStartScreen());
    }
    public void loadMainScreen()
    {
        StartCoroutine(LoadMainScreen());
    }
    public void loadRunnerEndScreen()
    {
        StartCoroutine(LoadRunnerEndScreen());
    }

    public void loadDropperEndScreen()
    {
        StartCoroutine(LoadDropperEndScreen());
    }

    IEnumerator LoadRunnerEndScreen()
    {
        transition.SetTrigger("EndStage");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("RunnerEndScreen");
    }

    IEnumerator LoadDropperEndScreen()
    {
        transition.SetTrigger("EndStage");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("DropperEndScreen");
    }

    IEnumerator LoadMainScreen()
    {
        transition.SetTrigger("EndStage");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator LoadStartScreen()
    {
        transition.SetTrigger("EndStage");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("MainMenu");
    }
}

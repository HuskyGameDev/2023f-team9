using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    public void loadEndScreen()
    {
        StartCoroutine(LoadEndScreen());
    }

    IEnumerator LoadEndScreen()
    {
        transition.SetTrigger("crossfade_end");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("End Screen");
    }
}

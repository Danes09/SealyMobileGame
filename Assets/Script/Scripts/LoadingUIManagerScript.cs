using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingUIManagerScript : MonoBehaviour
{
    public Image fadePanel;
    public float fadingDuration;
    
    void Start()
    {
        StartCoroutine(EnterGameFadeIn());
    }
    
    void Update()
    {
        
    }

    IEnumerator EnterGameFadeIn()
    {
        fadePanel.CrossFadeAlpha(0.0f, fadingDuration, false);
        yield return new WaitForSeconds(fadingDuration + 2.0f);
        fadePanel.GetComponent<Image>().CrossFadeAlpha(1.0f, fadingDuration, false);
        yield return new WaitForSeconds(fadingDuration + 1.0f);

        StartCoroutine(DelayBeforeChangeScene());
    }

    IEnumerator DelayBeforeChangeScene()
    {
        // This gives the PersistentDataManager time to get the data.
        // #Critical: Ideal time is 2 seconds.
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenuScene");
    }
}

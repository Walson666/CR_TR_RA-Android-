// dnSpy decompiler from Assembly-CSharp.dll class: SplashLoader
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

 

    IEnumerator LoadAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(this.asyncLoad.progress / 0.9f);
            this.percText.text = string.Format("{0:0.0}%", progress * 100f);
            this.barImg.fillAmount = progress;
            yield return null;
        }
    }

    public Text percText;

	public Image barImg;

	private AsyncOperation asyncLoad;
}

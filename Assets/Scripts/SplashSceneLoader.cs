//using Sirenix.OdinInspector;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SplashSceneLoader : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image progressBar; // Reference to the Slider UI component
    [SerializeField] private TMP_Text percentage;

    [Header("Scene Settings")]
    [SerializeField] private string gameplaySceneName = "MainMenu"; // Scene name to load
    [SerializeField] private float loadDuration = 10f; // Time to complete the loading

    private void Start()
    {
        StartCoroutine(LoadSceneWithProgress());
        Time.timeScale = 1;
        LabAnalytics.Instance.LogEvent("GAMESTART");

    }

    private IEnumerator LoadSceneWithProgress()
    {
        float elapsedTime = 0f;

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameplaySceneName);
        operation.allowSceneActivation = false; // Prevent auto-loading

        while (elapsedTime < loadDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / loadDuration);

            // Update slider value
            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
                float pro = progressBar.fillAmount * 100;
                percentage.text = pro.ToString("F0") + "%";
            }

            yield return null;
        }

        // Ensure progress bar is full before activating scene
        if (progressBar != null)
        {
            progressBar.fillAmount = 1f;
        }

        operation.allowSceneActivation = true; // Load the scene after 10 seconds
    }

    //[Button]
    private void ClearData()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

        foreach (FileInfo file in di.GetFiles())
            file.Delete();
        foreach (DirectoryInfo dir in di.GetDirectories())
            dir.Delete(true);

        PlayerPrefs.DeleteAll();
    }
}

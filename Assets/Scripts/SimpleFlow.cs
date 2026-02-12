





using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControllerFlow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loadingPanel;
    public GameObject characterPanel;
    public GameObject levelSelectionPanel;

    [Header("Loading Bar")]
    public Image loadingBar;

    public float loadingTime = 2.5f;
    private float timer;

    private void Start()
    {
        loadingPanel.SetActive(true);
        characterPanel.SetActive(false);
        levelSelectionPanel.SetActive(false);

        StartLoading(ShowCharacterPanel);
    }

    void StartLoading(System.Action onComplete)
    {
        loadingBar.fillAmount = 0;
        timer = 0;
        StartCoroutine(FillLoading(onComplete));
    }

    IEnumerator FillLoading(System.Action onComplete)
    {
        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            loadingBar.fillAmount = timer / loadingTime;
            yield return null;
        }
        onComplete?.Invoke();
    }

    void ShowCharacterPanel()
    {
        loadingPanel.SetActive(false);
        characterPanel.SetActive(true);
    }

    // CHARACTER NEXT BUTTON
    public void OnClick_CharacterNext()
    {
        AdsManager.Instance.ShowAds();
        characterPanel.SetActive(false);
        loadingPanel.SetActive(true);

        StartLoading(ShowLevelPanel);
    }

    void ShowLevelPanel()
    {
        loadingPanel.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }

    // LEVEL CONTINUE BUTTON
    public void OnClick_LevelContinue()
    {
        levelSelectionPanel.SetActive(false);
        loadingPanel.SetActive(true);

        StartLoading(StartGame);
    }

    void StartGame()
    {
        loadingPanel.SetActive(false);
        Debug.Log("GAME START HERE!");
        // Add your game start logic here
    }
}



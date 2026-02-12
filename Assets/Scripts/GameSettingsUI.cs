


//using UnityEngine;
//using UnityEngine.UI;

//public class GameSettingsUI : MonoBehaviour
//{
//    [Header("Main Panels")]
//    public GameObject settingPanel;     // Your SettingPanel
//    public GameObject mainMenuPanel;    // Your Main Menu Panel
//    public GameObject panel2;    // Your Main Menu Panel

//    [Header("Music Buttons")]
//    public GameObject musicOnBtn;       // Green ON button
//    public GameObject musicOffBtn;      // Red OFF button

//    [Header("Sound Buttons")]
//    public GameObject soundOnBtn;       // Green ON button
//    public GameObject soundOffBtn;      // Red OFF button

//    private void OnEnable()
//    {
//        RefreshUI();
//    }

//    // Refresh UI based on saved PlayerPrefs
//    private void RefreshUI()
//    {
//        musicOnBtn.SetActive(AudioManager.MusicEnabled);
//        musicOffBtn.SetActive(!AudioManager.MusicEnabled);

//        soundOnBtn.SetActive(AudioManager.SoundEnabled);
//        soundOffBtn.SetActive(!AudioManager.SoundEnabled);
//    }

//    // ========================= MUSIC =========================
//    public void OnMusicOnClicked()  // when user clicks ON → turn music OFF
//    {
//        AudioManager.ToggleMusic(false);
//        AudioManager.PlayButtonSound();
//        RefreshUI();
//        Debug.Log("🎵 Music turned OFF");
//    }

//    public void OnMusicOffClicked() // when user clicks OFF → turn music ON
//    {
//        AudioManager.ToggleMusic(true);
//        AudioManager.PlayButtonSound();
//        RefreshUI();
//        Debug.Log("🎵 Music turned ON");
//    }

//    // ========================= SOUND =========================
//    public void OnSoundOnClicked()  // when user clicks ON → turn sound OFF
//    {
//        AudioManager.ToggleSound(false);
//        AudioManager.PlayButtonSound();
//        RefreshUI();
//        Debug.Log("🔈 Sound turned OFF");
//    }

//    public void OnSoundOffClicked() // when user clicks OFF → turn sound ON
//    {
//        AudioManager.ToggleSound(true);
//        AudioManager.PlayButtonSound();
//        RefreshUI();
//        Debug.Log("🔈 Sound turned ON");
//    }

//    // ========================= SAVE =========================
//    public void OnSaveButton()
//    {
//        PlayerPrefs.Save();
//        AudioManager.ApplySettings();
//        AudioManager.PlayButtonSound();
//        Debug.Log("💾 Settings saved!");
//        CloseSettings();
//    }

//    // ========================= CLOSE =========================
//    public void CloseSettings()
//    {
//        settingPanel.SetActive(false);
//        mainMenuPanel.SetActive(true);
//        panel2.SetActive(false);
//        AudioManager.PlayButtonSound();
//        Debug.Log("🔙 Closed settings, returned to main menu");
//    }

//    // ========================= OPEN =========================
//    public void OpenSettings()
//    {
//        settingPanel.SetActive(true);
//        panel2.SetActive(true);
//        mainMenuPanel.SetActive(false);
//        AudioManager.PlayButtonSound();
//        RefreshUI();
//        Debug.Log("⚙️ Opened Settings Panel");
//    }
//}

/////////////////////////
/////
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUI : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject settingPanel;   // Settings Panel (the popup)
    public GameObject mainMenuPanel;  // Main Menu Panel (Play, Rate Us, etc.)
    public GameObject panel2;         // Optional parent panel if needed

    [Header("Music Buttons")]
    public GameObject musicOnBtn;     // Green ON button
    public GameObject musicOffBtn;    // Red OFF button

    [Header("Sound Buttons")]
    public GameObject soundOnBtn;     // Green ON button
    public GameObject soundOffBtn;    // Red OFF button

    private void OnEnable()
    {
        RefreshUI();
    }

    // 🔄 Refresh button visibility based on current saved state
    private void RefreshUI()
    {
        musicOnBtn.SetActive(AudioManager.MusicEnabled);
        musicOffBtn.SetActive(!AudioManager.MusicEnabled);

        soundOnBtn.SetActive(AudioManager.SoundEnabled);
        soundOffBtn.SetActive(!AudioManager.SoundEnabled);
    }

    // ==================== MUSIC ====================
    public void OnMusicOnClicked()
    {
        // ❌ No sound here
        AudioManager.ToggleMusic(false);
        RefreshUI();
        Debug.Log("🎵 Music turned OFF");
    }

    public void OnMusicOffClicked()
    {
        // ❌ No sound here
        AudioManager.ToggleMusic(true);
        RefreshUI();
        Debug.Log("🎵 Music turned ON");
    }

    // ==================== SOUND ====================
    public void OnSoundOnClicked()
    {
        // ❌ No sound here
        AudioManager.ToggleSound(false);
        RefreshUI();
        Debug.Log("🔈 Sound turned OFF");
    }

    public void OnSoundOffClicked()
    {
        // ❌ No sound here
        AudioManager.ToggleSound(true);
        RefreshUI();
        Debug.Log("🔈 Sound turned ON");
    }

    // ==================== SAVE ====================
    public void OnSaveButton()
    {
        // ✅ Only Save button plays sound
        //AudioManager.PlayButtonSound();
        PlayerPrefs.Save();
        AudioManager.ApplySettings();

        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        if (panel2 != null) panel2.SetActive(false);

        Debug.Log("💾 Settings saved and closed");
    }

    // ==================== CLOSE ====================
    public void CloseSettings()
    {
        // ❌ No sound here
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        if (panel2 != null) panel2.SetActive(false);

        Debug.Log("🔙 Closed settings, returned to main menu");
    }

    // ==================== OPEN ====================
    public void OpenSettings()
    {
        // ❌ No sound here
        settingPanel.SetActive(true);
        if (panel2 != null) panel2.SetActive(true);
        mainMenuPanel.SetActive(false);
        RefreshUI();

        Debug.Log("⚙️ Opened Settings Panel");
    }
}

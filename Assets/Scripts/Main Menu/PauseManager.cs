using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour {

    [Header("Levels to load")]
    public string levelToLoad;
    public string levelToReset;


    public GameObject pauseMenu;
    public GameObject Blur;

    private InputSystem_Actions inputActions;
    public bool isPaused = false;

    [SerializeField] private TimeManager timeManager;

    private void Awake() {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable() {
        inputActions.Player.Enable();
    }

    private void OnDisable() {
        inputActions.Player.Disable();
    }

    private void Update() {
        if (inputActions.Player.Pause.WasPressedThisFrame()) {
            Pause();
        }
        if (inputActions.Player.Reset.WasPressedThisFrame()) {
            ResetLevel(); 
        }
    }
    public void Pause() {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Blur.SetActive(isPaused);   
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void SaveTime() {
        
        float finalTime = timeManager.GetElapsedTime();

        List<float> times = LoadTimes();
        times.Add(finalTime);
        times.Sort(); 

        if (times.Count > 10)
            times.RemoveAt(times.Count - 1);

        for (int i = 0; i < times.Count; i++) {
            PlayerPrefs.SetFloat("BestTime" + i, times[i]);
        }
        PlayerPrefs.SetInt("BestTimeCount", times.Count);
        PlayerPrefs.Save();
    }

    public List<float> LoadTimes() {
        List<float> times = new List<float>();
        int count = PlayerPrefs.GetInt("BestTimeCount", 0);

        for (int i = 0; i < count; i++) {
            times.Add(PlayerPrefs.GetFloat("BestTime" + i, 0f));
        }

        return times;
    }

    public void LoadMenu() {
        Pause();
        SceneManager.LoadScene(levelToLoad);
    }
    public void ResetLevel() {

        if (isPaused) Pause();
        

        SceneManager.LoadScene(levelToReset);
    }
}
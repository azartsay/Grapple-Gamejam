using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour {

    public TextMeshProUGUI timerText;
    public float timeElapsed = 0f;
    private bool isRunning = true;

    void Update() {
        if (!isRunning) return;

        timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        int milliseconds = Mathf.FloorToInt((timeElapsed * 1000f) % 1000f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void PauseTimer() => isRunning = false;
    public void ResumeTimer() => isRunning = true;
    public float GetElapsedTime() => timeElapsed;
}


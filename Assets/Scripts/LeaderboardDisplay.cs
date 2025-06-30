using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardDisplay : MonoBehaviour {
    public TextMeshProUGUI leaderboardText;  

    private const int maxEntries = 5;

    public void LoadAndDisplayTimes() {
        List<float> times = LoadTimes();

        if (times.Count == 0) {
            leaderboardText.text = "Start playing lil bro";
            return;
        }

        leaderboardText.text = "";
        for (int i = 0; i < times.Count && i < maxEntries; i++) {
            leaderboardText.text += $"{i + 1}. {FormatTime(times[i])}\n";
        }
    }

    private List<float> LoadTimes() {
        List<float> times = new List<float>();
        int count = PlayerPrefs.GetInt("BestTimeCount", 0);

        for (int i = 0; i < count; i++) {
            times.Add(PlayerPrefs.GetFloat("BestTime" + i, 0f));
        }

        return times;
    }

    private string FormatTime(float time) {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
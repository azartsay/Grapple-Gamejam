using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField] private TimeManager timeManager;
    [SerializeField] private PauseManager pauseManager;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("End")) {
            if (timeManager != null) {
                timeManager.PauseTimer();
                pauseManager.SaveTime();

                pauseManager.ResetLevel();

                if (pauseManager.isPaused) {
                    pauseManager.Pause();
                }
            }
        }

        if (collision.CompareTag("Start")) {
            if (timeManager != null) {
                timeManager.isStarted = true;
            }
        }
    }
}

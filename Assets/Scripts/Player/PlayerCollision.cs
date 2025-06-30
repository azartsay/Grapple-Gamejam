using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField] private TimeManager timeManager;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject backGroundTwo;
    [SerializeField] GameObject cam;
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

        if (collision.CompareTag("Teleport")) {

            if(collision.name == "TeleporterCave") {
                backGround.SetActive(false);
                backGroundTwo.SetActive(true);
            }

            transform.position = collision.transform.GetChild(0).position;
            cam.transform.position = collision.transform.GetChild(0).position;

        }
    }
}

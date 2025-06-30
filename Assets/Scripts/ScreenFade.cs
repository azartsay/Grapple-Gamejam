using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour {
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    public IEnumerator FadeOutIn(System.Action onMiddle = null) {
        // Fade to black
        yield return Fade(0f, 1f);

        // Call optional teleport/move/etc
        onMiddle?.Invoke();

        // Wait 0.1s while black
        yield return new WaitForSeconds(0.1f);

        // Fade back to visible
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float start, float end) {
        float elapsed = 0f;
        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float alpha = Mathf.Lerp(start, end, t);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }
}
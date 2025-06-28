using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.IO.LowLevel.Unsafe;

public class MenuController : MonoBehaviour
{
    [Header("Levels to load")]
    public string levelToLoad;
    public void OnNewGameStart() {
        SceneManager.LoadScene(levelToLoad);
    }
    public void OnExit() { 
        Application.Quit();
    }

}




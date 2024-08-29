using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float timerDuration = 60f; // Duration before returning to login scene
    private float time;
    // Start is called before the first frame update
    void Start()
    {
                // Load the remaining time from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey(Timer.TimerKey))
        {
            Timer.timer = PlayerPrefs.GetFloat(Timer.TimerKey);
        }
        else
        {
            Timer.timer = timerDuration;
        }

        // If time is up, reset to login scene
        if (Timer.timer <= 0)
        {
            SceneManager.LoadScene("Login_VR");
        }
    }

    void Update()
    {
        Timer.timer -= Time.deltaTime;
        time = Timer.timer;
        if (Timer.timer <= 0)
        {
            PlayerPrefs.DeleteKey(Timer.TimerKey); // Clear the Timer.timer when it runs out
            SceneManager.LoadScene("Login");
        }
    }

        void OnApplicationQuit()
    {
        // Save the remaining time to PlayerPrefs
        PlayerPrefs.SetFloat(Timer.TimerKey, Timer.timer);
        PlayerPrefs.Save();
    }
}

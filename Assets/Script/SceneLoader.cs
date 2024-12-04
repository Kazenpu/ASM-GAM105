using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Play()
    {   
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
    public void Hidden()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(2);
    }
    public void Setting()
    {
        Debug.Log("Cai dat");
    }
    public void Exit()
    {
        Debug.Log("Thoat ganme");   
    }
}

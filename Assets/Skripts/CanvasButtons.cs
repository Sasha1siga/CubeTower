using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
   public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // непонятно, делает перезагрузку игры
    }
    public void LoadInstagram ()
    {
        Application.OpenURL("https://vk.com/sasha.malk");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    // Este método lo asignamos al botón en el Inspector
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

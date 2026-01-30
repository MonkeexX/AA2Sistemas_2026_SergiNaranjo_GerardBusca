using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToReplays : MonoBehaviour
{
    public Button myButton; 

    public string sceneToLoad; 

    void Start()
    {
        if (myButton != null)
            myButton.onClick.AddListener(() => GoToScene(sceneToLoad));
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

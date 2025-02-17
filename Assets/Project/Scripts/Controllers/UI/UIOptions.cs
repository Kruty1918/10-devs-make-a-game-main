using Bonjoura.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIOptions : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    private void Update()
    {
       //OpenPauseMenu();
    }

    private void OpenPauseMenu()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!_pausePanel.activeSelf)
            {
                OpenPanel(_pausePanel);
            }
            else
            {
                ClosePanel(_pausePanel);
            }
        }
    }

    public void OpenPanel(GameObject Panel)
    {
        Panel.SetActive(true);
        Time.timeScale = 0f;
        PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
    }

    public void ClosePanel(GameObject Panel)
    {
        Panel.SetActive(false);
        Time.timeScale = 1.0f;
        PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
    }

    public void OpenScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
        Time.timeScale = 1.0f;
        PlayerController.Instance.FPSCamera.enabled = !PlayerController.Instance.FPSCamera.enabled;
    }

}

using UnityEngine;
using UnityEngine.SceneManagement; // Påkrævet for at skifte scener

public class MainMenu : MonoBehaviour
{
    // Reference til dit indstillingspanel i Unity
    public GameObject settingsPanel;

    // Start-knap: Indlæser scenen med navnet "Game Scene"
    public void PlayGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    // Settings-knap: Åbner og lukker indstillingerne
    public void ToggleSettings(bool isOpen)
    {
        settingsPanel.SetActive(isOpen);
    }

    // Valgfrit: En Quit-knap, hvis du vil have en i din menu
    public void QuitGame()
    {
        Debug.Log("Spillet lukker..."); // Vises kun i Unity-editoren
        Application.Quit(); // Lukker selve spillet, når det er bygget
    }
}

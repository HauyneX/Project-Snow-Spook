using UnityEngine;

public class Main_Menu : MonoBehaviour
{
    // Reference til dit indstillingspanel i Unity
    public GameObject SettingsPanel;

    // Åbner indstillingerne og skjuler dem igen (bruges til både 'Settings' og 'Luk/Back' knapper)
    public void ToggleSettings(bool isOpen)
    {
        SettingsPanel.SetActive(isOpen);
    }
}

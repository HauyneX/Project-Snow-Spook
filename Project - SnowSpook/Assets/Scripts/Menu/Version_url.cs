using UnityEngine;

public class URLOpener : MonoBehaviour
{
    [SerializeField] private string urlToOpen = "https://github.com/HauyneX/Project-Snow-Spook";

    // This method must be public so the Unity Button component can see it
    public void OpenURL()
    {
        if (!string.IsNullOrEmpty(urlToOpen))
        {
            Application.OpenURL(urlToOpen);
        }
        else
        {
            Debug.LogWarning("URL string is empty! Please assign a valid URL in the Inspector.");
        }
    }
}

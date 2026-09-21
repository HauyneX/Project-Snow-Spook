using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlightLight; // Drag your Spotlight object here
    public AudioSource audioSource;    // Optional: Drag an AudioSource here for the click sound
    public AudioClip clickSound;       // Optional: Assign a crunchy click sound effect

    [Header("Battery Flicker")]
    public bool canFlicker = true;
    public float flickerChance = 0.05f; // Chance per frame to flicker when active
    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    private bool isOn = false;
    private bool isFlickering = false;
    private Light lightComponent;
    private float defaultIntensity;

    void Start()
    {
        lightComponent = flashlightLight.GetComponent<Light>();
        defaultIntensity = lightComponent.intensity;

        // Start with flashlight turned off
        flashlightLight.SetActive(false);
    }

    void Update()
    {
        // Toggle flashlight with the Left Mouse Click
        if (Input.GetMouseButtonDown(1))
        {
            ToggleFlashlight();
        }

        // Randomly flicker to simulate a dying 1980s battery in the cold forest
        if (isOn && canFlicker && !isFlickering)
        {
            if (Random.value < flickerChance * Time.deltaTime)
            {
                StartCoroutine(FlickerRoutine());
            }
        }
    }

    void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLight.SetActive(isOn);

        // Play a satisfying retro click sound
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    System.Collections.IEnumerator FlickerRoutine()
    {
        isFlickering = true;

        // Drop intensity quickly to simulate a bad connection
        lightComponent.intensity = defaultIntensity * Random.Range(0.1f, 0.3f);
        yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));

        // Restore intensity
        lightComponent.intensity = defaultIntensity;

        isFlickering = false;
    }
}

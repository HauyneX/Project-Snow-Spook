using UnityEngine;
using System.Collections; // Required for Coroutines/IEnumerator

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlightLight; // Drag your Spotlight object here
    public AudioSource audioSource;    // Optional: Drag an AudioSource here for the click sound
    public AudioClip clickSound;       // Optional: Assign a crunchy click sound effect

    [Header("Timings")]
    public float turnOnDelay = 0.5f;   // Delay before the light turns on after clicking

    [Header("Battery Flicker")]
    public bool canFlicker = true;
    public float flickerChance = 0.05f; // Chance per frame to flicker when active
    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    private bool isOn = false;
    private bool isFlickering = false;
    private bool isTurningOn = false; // Prevents spamming clicks while waiting to turn on
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
        // Toggle flashlight with the Right Mouse Click (Input.GetMouseButtonDown(1))
        if (Input.GetMouseButtonDown(1) && !isTurningOn)
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

        // Play the click sound instantly every time
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

        if (isOn)
        {
            // If turning ON, wait for the delay before activating the light object
            StartCoroutine(TurnOnWithDelayRoutine());
        }
        else
        {
            // If turning OFF, happen instantly
            flashlightLight.SetActive(false);
        }
    }

    IEnumerator TurnOnWithDelayRoutine()
    {
        isTurningOn = true;

        // Wait for the specified delay (1 second) while the sound plays
        yield return new WaitForSeconds(turnOnDelay);

        // Turn the light on
        flashlightLight.SetActive(true);
        lightComponent.intensity = defaultIntensity; // Reset intensity in case it was flickering

        isTurningOn = false;
    }

    IEnumerator FlickerRoutine()
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
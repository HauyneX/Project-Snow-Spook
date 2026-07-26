using UnityEngine;

public class PoliceSiren : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 720f; // Fast spin (degrees per second)

    [Header("Strobe Settings")]
    [SerializeField] private bool useStrobeEffect = true;
    [SerializeField] private float flashSpeed = 15f;

    private Light sirenLight;
    private float baseIntensity;

    void Start()
    {
        sirenLight = GetComponent<Light>();
        baseIntensity = sirenLight.intensity;
    }

    void Update()
    {
        // 1. Spin the light around its Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        // 2. Optional: Add a high-frequency flicker for an intense strobe effect
        if (useStrobeEffect)
        {
            float noise = Mathf.PingPong(Time.time * flashSpeed, 1.0f);
            sirenLight.intensity = baseIntensity * (noise > 0.4f ? 1f : 0.1f);
        }
    }
}

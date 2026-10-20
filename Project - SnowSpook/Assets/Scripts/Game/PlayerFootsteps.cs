using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource footstepAudioSource; // AudioSource component for footstep sounds
    public AudioClip[] footstepClips;       // Drag multiple step clips here for variety!

    [Header("Timing")]
    public float walkStepInterval = 0.5f;   // Time in seconds between walk steps
    public float runStepInterval = 0.3f;    // Time in seconds between run steps

    [Header("Pitch Randomization (Optional)")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    [Header("Player Components (Optional)")]
    public CharacterController characterController; // If using CharacterController, drag it here

    private float stepTimer;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        // Auto-get components on this GameObject if not manually assigned
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        // Don't play if missing required sound setup
        if (footstepAudioSource == null || footstepClips == null || footstepClips.Length == 0) return;

        // Calculate current movement speed
        float speed = 0f;

        if (characterController != null)
        {
            // Use velocity if a CharacterController is present and grounded
            if (characterController.isGrounded)
            {
                speed = characterController.velocity.magnitude;
            }
        }
        else
        {
            // Fallback speed check based on position change
            speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;
        }

        // If the player is moving fast enough
        if (speed > 0.1f)
        {
            // Detect if holding Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentInterval = isRunning ? runStepInterval : walkStepInterval;

            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = currentInterval; // Reset timer
            }
        }
        else
        {
            // Reset timer when stationary so step plays instantly when walking again
            stepTimer = 0f;
        }
    }

    void PlayFootstepSound()
    {
        // Pick a random clip from the array
        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip selectedClip = footstepClips[randomIndex];

        // Apply slight pitch variance so steps don't sound identical
        footstepAudioSource.pitch = Random.Range(minPitch, maxPitch);
        footstepAudioSource.PlayOneShot(selectedClip);
    }
}
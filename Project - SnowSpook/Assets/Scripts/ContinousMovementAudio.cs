using UnityEngine;
using System.Collections;

public class ContinuousMovementAudio : MonoBehaviour
{
    // Surface sound group structure
    [System.Serializable]
    public struct SurfaceAudioGroup
    {
        public string surfaceTag;       // e.g., "Wood", "Metal", "Concrete"
        public AudioClip walkLoopClip;   // 6-second walk loop for this surface
        public AudioClip runLoopClip;    // 6-second run loop for this surface
    }

    [Header("Audio Setup")]
    public AudioSource movementAudioSource; // Dedicated AudioSource for movement

    [Header("Default Surface (Fallback)")]
    public AudioClip defaultWalkLoopClip;
    public AudioClip defaultRunLoopClip;

    [Header("Custom Surface Audio Sets")]
    public SurfaceAudioGroup[] customSurfaces; // Add entries for Wood, Metal, etc.

    [Header("Ground Detection Settings")]
    public Transform groundCheckOrigin;  // Optional: Leave blank to use player transform
    public float raycastDistance = 1.5f; // Distance down to check for ground
    public LayerMask groundLayers = ~0;  // Which layers count as ground

    [Header("Player Components (Optional)")]
    public CharacterController characterController;

    [Header("Movement Threshold")]
    public float movementThreshold = 0.1f;

    private Vector3 lastPosition;
    private string currentDetectedTag = "";

    void Start()
    {
        lastPosition = transform.position;

        if (movementAudioSource == null)
            movementAudioSource = GetComponent<AudioSource>();

        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (groundCheckOrigin == null)
            groundCheckOrigin = transform;

        if (movementAudioSource != null)
            movementAudioSource.loop = true;
    }

    void Update()
    {
        HandleMovementAudio();
    }

    void HandleMovementAudio()
    {
        if (movementAudioSource == null) return;

        // 1. Calculate current movement speed
        float speed = 0f;

        if (characterController != null)
        {
            if (characterController.isGrounded)
                speed = characterController.velocity.magnitude;
        }
        else
        {
            speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;
        }

        // 2. Check movement state
        if (speed > movementThreshold)
        {
            // Detect surface type beneath player's feet
            DetectSurface();

            // Determine if running
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            // Fetch correct audio clip based on detected surface and speed
            AudioClip targetClip = GetAudioClipForCurrentSurface(isRunning);

            // Switch clips seamlessly if surface or speed state changes
            if (targetClip != null)
            {
                if (movementAudioSource.clip != targetClip)
                {
                    movementAudioSource.clip = targetClip;
                    movementAudioSource.Play();
                }
                else if (!movementAudioSource.isPlaying)
                {
                    movementAudioSource.Play();
                }
            }
        }
        else
        {
            // Stop sound when player comes to a standstill
            if (movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
            }
        }
    }

    void DetectSurface()
    {
        // Shoot a ray straight down from the player's feet
        Ray ray = new Ray(groundCheckOrigin.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, groundLayers))
        {
            currentDetectedTag = hit.collider.gameObject.tag;
        }
        else
        {
            currentDetectedTag = ""; // In air or no ground hit
        }
    }

    AudioClip GetAudioClipForCurrentSurface(bool isRunning)
    {
        // Check if detected tag matches any custom surface group
        if (!string.IsNullOrEmpty(currentDetectedTag) && customSurfaces != null)
        {
            foreach (SurfaceAudioGroup surface in customSurfaces)
            {
                if (surface.surfaceTag.Equals(currentDetectedTag, System.StringComparison.OrdinalIgnoreCase))
                {
                    return isRunning ? surface.runLoopClip : surface.walkLoopClip;
                }
            }
        }

        // Fallback to default clips if surface tag isn't explicitly matched
        return isRunning ? defaultRunLoopClip : defaultWalkLoopClip;
    }

    // Draw raycast in Scene view to help debug ground check distance
    void OnDrawGizmosSelected()
    {
        Transform origin = groundCheckOrigin != null ? groundCheckOrigin : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin.position, origin.position + Vector3.down * raycastDistance);
    }
}
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioClip[] FootstepClips;
    public float StepInterval = 0.5f;
    
    private AudioSource audioSource;
    private float stepTimer;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        if (FootstepClips.Length == 0)
            return;
        
        AudioClip clip = FootstepClips[Random.Range(0, FootstepClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void Update() {
        stepTimer += Time.deltaTime;
        if (stepTimer >= StepInterval) {
            PlayFootstep();
            stepTimer = 0.0f;
        }
    }
}

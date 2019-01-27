using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip forwardSound;
    [SerializeField] AudioClip backSound;
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip winningSound;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayForward()
    {
        audioSource.PlayOneShot(forwardSound);
    }

    public void PlayBack()
    {
        audioSource.PlayOneShot(backSound);
    }

    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }
}

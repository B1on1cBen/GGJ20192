// Benjamin Gordon 2018
namespace Medley.Input
{
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class UISoundPlayer : MonoBehaviour
    {
        public AudioClip uiForward;
        public AudioClip uiBack;
        public AudioClip uiOnHover;
        public AudioClip uiTap;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayUIForward()
        {
            source.PlayOneShot(uiForward);
        }

        public void PlayUITap()
        {
            source.PlayOneShot(uiTap);
        }

        public void PlayUIBack()
        {
            source.PlayOneShot(uiBack);
        }

        public void PlayUIOnHover()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            source.PlayOneShot(uiOnHover);
#endif
        }
    }
}

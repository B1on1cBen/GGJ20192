// Benjamin Gordon 2017
namespace Medley.Input
{
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class ButtonConfigurer : MonoBehaviour
    {
        private Animator animator;
        private int hoverID;

        void Awake()
        {
            animator = GetComponent<Animator>();
            hoverID = Animator.StringToHash("Hover");
        }

        public void SetHoverState(bool value)
        {
            animator.SetBool(hoverID, value);
        }
    }
}

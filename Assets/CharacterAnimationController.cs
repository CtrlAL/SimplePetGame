using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private string _triggerName = "PlayAniamtion";

        public void Awake()
        {
            KickEventPublisher.Instance.PlayerKickEvent += PlayAnimtion;
        }

        private void PlayAnimtion(object sender, KickEventArgs e)
        {
            _animator.SetTrigger(_triggerName);
        }

        private void OnDestroy()
        {
            KickEventPublisher.Instance.PlayerKickEvent -= PlayAnimtion;
        }
    }
}


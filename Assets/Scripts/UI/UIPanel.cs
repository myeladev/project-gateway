using UnityEngine;
using DG.Tweening;

namespace ProjectGateway.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIPanel : MonoBehaviour
    {
        [HideInInspector]
        public CanvasGroup canvasGroup;
        private Vector3 startPos;
        public KeyCode hotKey;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            startPos = transform.localPosition;
        }

        public void Show()
        {
            OnShow();
            
            Sequence showSequence = DOTween.Sequence();
            showSequence.Join(canvasGroup.DOFade(1f, 0.5f));
            showSequence.Join(transform.DOLocalMoveY(startPos.y, 0.5f));
            showSequence.OnComplete(() =>
            {
                transform.localPosition = startPos;
            });
        }

        public abstract void OnShow();

        public void Hide()
        {
            OnHide();
            Sequence hideSequence = DOTween.Sequence();
            hideSequence.Join(canvasGroup.DOFade(0f, 0.5f));
            hideSequence.Join(transform.DOLocalMoveY(startPos.y + 20f, 0.5f));
            hideSequence.OnComplete(() => {
                gameObject.SetActive(false);
                transform.localPosition = startPos;
                OnHidden();
            });
        }

        protected abstract void OnHide();
        protected abstract void OnHidden();
    }
}

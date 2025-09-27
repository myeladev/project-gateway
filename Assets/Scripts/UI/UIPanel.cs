using UnityEngine;
using DG.Tweening;

namespace ProjectGateway.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public abstract class UIPanel : MonoBehaviour
    {
        [HideInInspector]
        public CanvasGroup canvasGroup;
        private Vector3 startPos;
        public bool exitOnNewPanelPush;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            startPos = transform.localPosition;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            OnShow();
            
            Sequence showSequence = DOTween.Sequence();
            showSequence.Join(canvasGroup.DOFade(1f, 0.5f));
            showSequence.Join(transform.DOLocalMoveY(startPos.y, 0.5f));
            showSequence.OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
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
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                OnHidden();
            });
        }

        protected abstract void OnHide();
        protected abstract void OnHidden();
    }
}

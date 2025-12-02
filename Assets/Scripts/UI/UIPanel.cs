using UnityEngine;
using DG.Tweening;

namespace ProjectDaydream.UI
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
            
            Sequence showSequence = DOTween.Sequence().SetUpdate(true);
            showSequence.Join(canvasGroup.DOFade(1f, 0.5f).SetUpdate(true));
            showSequence.Join(transform.DOLocalMoveY(startPos.y, 0.5f).SetUpdate(true));
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
            Sequence hideSequence = DOTween.Sequence().SetUpdate(true);
            hideSequence.Join(canvasGroup.DOFade(0f, 0.5f).SetUpdate(true));
            hideSequence.Join(transform.DOLocalMoveY(startPos.y + 20f, 0.5f).SetUpdate(true));
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

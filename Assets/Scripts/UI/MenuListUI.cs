using DG.Tweening;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class MenuListUI : UIPanel
    {
        public CanvasGroup gameTitle;
        private Vector3 gameTitleStartPos;

        protected override void Awake()
        {
            base.Awake();
            gameTitleStartPos = gameTitle.transform.localPosition;
        }

        public override void OnShow()
        {
            gameTitle.gameObject.SetActive(true);
            Sequence showSequence = DOTween.Sequence();
            showSequence.Join(gameTitle.DOFade(1f, 0.5f));
            showSequence.Join(gameTitle.transform.DOLocalMoveY(gameTitleStartPos.y, 0.5f));
            showSequence.OnComplete(() =>
            {
                gameTitle.transform.localPosition = gameTitleStartPos;
            });
        }

        protected override void OnHide()
        {
            Sequence hideSequence = DOTween.Sequence();
            hideSequence.Join(gameTitle.DOFade(0f, 0.5f));
            hideSequence.Join(gameTitle.transform.DOLocalMoveY(gameTitleStartPos.y + 20f, 0.5f));
            hideSequence.OnComplete(() => {
                gameTitle.gameObject.SetActive(false);
                gameTitle.transform.localPosition = gameTitleStartPos;
            });
        }

        protected override void OnHidden()
        {
            
        }
    }
}

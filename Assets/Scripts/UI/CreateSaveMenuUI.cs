using ProjectDaydream.DataPersistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectDaydream.UI
{
    public class CreateSaveMenuUI : UIPanel
    {
        [SerializeField] private TMP_InputField saveNameInput;
        [SerializeField] private Toggle ambientMode;
        
        public void ClickCreate()
        {
            DataPersistenceManager.Instance.CreateNewProfile(saveNameInput.text, ambientMode.isOn);
        }

        public override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnHidden()
        {
        }
    }
}

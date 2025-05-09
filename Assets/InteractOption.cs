using TMPro;
using UnityEngine;

namespace ProjectGateway
{
    public class InteractOption : MonoBehaviour
    {
        private string _option;
        public void SetOption(string option)
        {
            _option = option;
            GetComponent<TextMeshProUGUI>().text = option;
        }

        public void Click()
        {
            OptionsUI.Instance.ChooseOption(_option);
        }
    }
}

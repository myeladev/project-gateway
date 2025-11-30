using System;
using System.Globalization;
using ProjectGateway.Core;
using ProjectGateway.DataPersistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGateway.UI
{
    public class SaveSelectItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI saveName;
        [SerializeField] private TextMeshProUGUI saveDate;
        [SerializeField] private TextMeshProUGUI gameVersion;
        [SerializeField] private RawImage saveImage;

        private GameMetaData _metaData;
        
        private string FormatSaveDate(DateTime date)
        {
            if (date.Date == DateTime.Today)
            {
                return $"{date:HH:mm} Today";
            }
            if (date.Date == DateTime.Today.AddDays(-1))
            {
                return $"{date:HH:mm} Yesterday";
            }
            return date.ToString("HH:mm dd/MM/yyyy");
        }

        public void SetSave(GameMetaData metaData, Texture image)
        {
            _metaData = metaData;
            saveName.text = metaData.profileName;
            var savedDate = DateTime.Parse(metaData.lastSavedDate, CultureInfo.InvariantCulture);
            saveDate.text = FormatSaveDate(savedDate);
            saveImage.texture = image;
            gameVersion.text = $"v{metaData.gameVersion}";
            if (metaData.gameVersion != Application.version)
            {
                gameVersion.color = Color.red;
            }
            if (image is null) saveImage.texture = Texture2D.blackTexture;
        }

        public void LoadSave()
        {
            MainMenuUI.Instance.PopAllPanels();
            SceneManager.Instance.LoadScenesForProfile(_metaData.profileName);
        }
        
        public void DeleteSave()
        {
            
        }
    }
}

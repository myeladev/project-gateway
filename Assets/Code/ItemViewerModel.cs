using ProjectGateway.Code;
using UnityEngine;

namespace ProjectGateway
{
    public class ItemViewerModel : MonoBehaviour
    {
        public GameObject currentItem;
        
        // Update is called once per frame
        void Update()
        {
            if (currentItem)
            {
                currentItem.transform.position = transform.position;
                currentItem.transform.Rotate(0f, -Time.deltaTime * 20f, 0f, Space.Self);
            }
        }

        public void SetItem(GameObject item)
        {
            ClearItem();

            currentItem = item;
            currentItem.layer = LayerMask.NameToLayer("ItemViewer");
            currentItem.transform.rotation = Quaternion.Euler(-30f, 0, 0);
        }

        public void ClearItem()
        {
            if (currentItem)
            {
                currentItem.transform.position = Utilities.InventoryPoolPosition;
                currentItem.layer = LayerMask.NameToLayer("Prop");
            }

            currentItem = null;
        }
    }
}

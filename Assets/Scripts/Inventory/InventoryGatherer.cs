using Rebirth.Items;
using UnityEngine;

namespace Rebirth.Inventory
{
    public class InventoryGatherer : MonoBehaviour
    {
        public InventoryObject inventory;

        public void OnTriggerEnter(Collider other)
        {
            var worldItem = other.GetComponent<WorldItem>();
            if (worldItem)
            {
                inventory.AddItem(worldItem.itemStack.Item, worldItem.itemStack.Size);
                Destroy(other.gameObject);
            }
        }

        private void OnApplicationQuit() {
            inventory.Items.Clear();
        }
    }
}
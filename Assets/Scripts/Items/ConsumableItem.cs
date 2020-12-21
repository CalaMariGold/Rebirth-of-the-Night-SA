using UnityEngine;

namespace Rebirth.Items
{
    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Rebirth/Items/Consumable")]
    public class ConsumableItem : ItemObject
    {
        public void Awake()
        {
            type = ItemType.Consumable;
            maxSize = 1;
        }
    }
}
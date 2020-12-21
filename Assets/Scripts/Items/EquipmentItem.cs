using UnityEngine;

namespace Rebirth.Items
{
    [CreateAssetMenu(fileName = "New Equipment Item", menuName = "Rebirth/Items/Equipment")]
    public class EquipmentItem : ItemObject
    {
        public void Awake()
        {
            type = ItemType.Equipment;
            maxSize = 1;
        }
    }
}
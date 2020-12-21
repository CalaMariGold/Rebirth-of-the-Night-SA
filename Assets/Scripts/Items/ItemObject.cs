using UnityEngine;

namespace Rebirth.Items
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Tool,
        Default
    }

    public abstract class ItemObject : ScriptableObject
    {
        public GameObject prefab;
        public ItemType type;
        public int maxSize;
        [TextArea(15, 20)]
        public string description;
    }
}
using UnityEngine;

namespace Rebirth.Items
{
    [CreateAssetMenu(fileName = "New Default Item", menuName = "Rebirth/Items/Default")]
    public class DefaultItem : ItemObject
    {
        public void Awake()
        {
            type = ItemType.Default;
            maxSize = 64;
        }
    }
}
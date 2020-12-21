using UnityEngine;

namespace Rebirth.Items
{
    [CreateAssetMenu(fileName = "New Tool Object", menuName = "Rebirth/Items/Tool")]
    public class ToolItem : ItemObject
    { 
        public void Awake()
        {
            type = ItemType.Tool;
            maxSize = 1;
        }
    }
}
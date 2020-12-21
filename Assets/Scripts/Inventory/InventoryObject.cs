using System.Collections;
using System.Collections.Generic;
using Rebirth.Items;
using UnityEngine;

namespace Rebirth.Inventory {

    [CreateAssetMenu(fileName = "New Inventory", menuName = "Rebirth/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public List<ItemStack> Items = new List<ItemStack>();

        public void AddItem(ItemObject item, int amount)
        {
            var itemMatches = Items.FindAll(x => x.Item == item);
            foreach (var match in itemMatches)
            {
                if (amount > 0)
                {
                    amount = match.AddSize(amount);
                }
                else
                {
                    break;
                }
            }

            if (amount > 0)
            {
                Items.Add(new ItemStack(item, amount));
            }
        }
    }
}
namespace Rebirth.Items {
    [System.Serializable]
    public class ItemStack
    {
        public ItemObject Item;
        public int Size;
        public ItemStack(ItemObject item, int size)
        {
            Item = item;
            Size = size;
        }

        public int AddSize(int value)
        {
            var excess = 0;
            Size += value;
            if(Size > Item.maxSize)
            {
                excess = Size - Item.maxSize;
                Size = Item.maxSize;
            }
            return excess;
        }
    }
}
namespace ApplicationCore.Entities
{
    public class OrderItem : BaseEntity
    {
        public ItemOrdered OrderedItem { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Units { get; private set; }

        public OrderItem() { }

        public OrderItem(ItemOrdered item, decimal pricePerUnit, int units)
        {
            OrderedItem = item;
            UnitPrice = pricePerUnit;
            Units = units;
        }
    }
}

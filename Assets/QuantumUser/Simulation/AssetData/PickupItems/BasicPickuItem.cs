namespace Quantum {
    public class BasicPickuItem : PickupItemBase
    {
        public override void PickupItem(Frame f, EntityRef pickupItemEntity, EntityRef pickerEntity)
        {
            f.Destroy(pickupItemEntity);
        }
    }
}

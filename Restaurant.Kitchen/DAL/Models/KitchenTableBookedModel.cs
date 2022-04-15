namespace Restaurant.Kitchen.DAL.Models
{
    public class KitchenTableBookedModel
    {
        public int Id { get; set; }
        public Guid MessageId { get; set; }
        public Guid OrderId { get; set; }
    }
}

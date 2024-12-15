namespace CombinedAPI.Models
{
  public class Cart
  {
    public int cartId { get; set; }
    public required int userId { get; set; }
    public required SortedDictionary<int, int> itemList { get; set; }
    public required double totalPrice { get; set; }
  }
}

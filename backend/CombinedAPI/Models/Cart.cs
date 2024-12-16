namespace CombinedAPI.Models
{
  public class Cart
  {
    public int cartId { get; set; }
    public int userId { get; set; }
    public SortedDictionary<int, int> itemList { get; set; }
    public double totalPrice { get; set; }
  }
}
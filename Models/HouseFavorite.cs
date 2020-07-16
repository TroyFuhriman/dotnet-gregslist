using System.ComponentModel.DataAnnotations;

namespace fullstack_gregslist.Models
{
  public class DTOHouseFavorite
  {
    public int Id { get; set; }
    [Required]
    public int HouseId { get; set; }
    public string User { get; set; }
  }
}
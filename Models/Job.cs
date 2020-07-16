using System.ComponentModel.DataAnnotations;

namespace fullstack_gregslist.Models
{
  public class Job
  {
    public int Id { get; set; }
    public string UserId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    [Required]
    public int Price { get; set; }
  }
  public class ViewModelJobFavorite : Car
  {
    public int FavoriteId { get; set; }
  }
}
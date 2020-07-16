using System.ComponentModel.DataAnnotations;

namespace fullstack_gregslist.Models
{
  public class DTOJobFavorite
  {
    public int Id { get; set; }
    [Required]
    public int JobId { get; set; }
    public string User { get; set; }
  }
}
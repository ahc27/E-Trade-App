using System.ComponentModel.DataAnnotations.Schema;

namespace classLib;

public class GetCategoryDto
{
    public int Id { get; set; } 
    public string Name { get; set; }

    [ForeignKey("ParentCategory")]
    public String? ParentCategoryName { get; set; }
}

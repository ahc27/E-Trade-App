using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryMicroservice.Service.Dtos
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; }

        [ForeignKey("ParentCategory")]
        public String? ParentCategoryName { get; set; }
    }
}

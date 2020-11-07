using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.DataLayer.TableEntity
{
    [Table("ToDo")]
    public class ToDo : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToDoId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at most {1} characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        public string ToDoTitle { get; set; }

        public bool IsDone { get; set; }

        [Column(TypeName = "date"), Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

    }
}

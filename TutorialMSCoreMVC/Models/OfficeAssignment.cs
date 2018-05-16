using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialMSCoreMVC.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public Instructor Instructor { get; set; }

        /*Há uma relação um para zero ou um entre as entidades Instructor e OfficeAssignment. Uma atribuição de escritório existe apenas em relação ao instrutor ao qual ela é atribuída e, portanto, sua chave primária também é a chave estrangeira da entidade Instructor. Mas o Entity Framework não pode reconhecer InstructorID automaticamente como a chave primária dessa entidade porque o nome não segue a convenção de nomenclatura ID ou classnameID. Portanto, o atributo Key é usado para identificá-la como a chave:*/
    }
}

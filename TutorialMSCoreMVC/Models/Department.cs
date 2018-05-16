using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialMSCoreMVC.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")] /*Anteriormente, você usou o atributo Column para alterar o mapeamento de nome de coluna. No código da entidade Department, o atributo Column está sendo usado para alterar o mapeamento de tipo de dados SQL, do modo que a coluna seja definida usando o tipo de dinheiro do SQL Server no banco de dados:*/
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; } /*Um departamento pode ou não ter um administrador, e um administrador é sempre um instrutor. Portanto, a propriedade InstructorID é incluída como a chave estrangeira na entidade Instructor e um ponto de interrogação é adicionado após a designação de tipo int para marcar a propriedade como uma propriedade que permite valor nulo. A propriedade de navegação é chamada Administrator, mas contém uma entidade Instructor*/
        public Instructor Administrator { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialMSCoreMVC.Models
{
    public class Course
    {
        /*O atributo DatabaseGenerated com o parâmetro None na propriedade CourseID especifica que os valores de chave primária são fornecidos pelo usuário, em vez de serem gerados pelo banco de dados.*/
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseID { get; set; }
        public string Title { get; set; }
        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
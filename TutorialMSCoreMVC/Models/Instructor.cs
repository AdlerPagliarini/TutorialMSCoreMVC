using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TutorialMSCoreMVC.Models
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        //public ICollection<CourseAssignment> CourseAssignments { get; set; }
        private ICollection<CourseAssignment> _courseAssignments;
        //with this code, onCreate at Instructor dont need to initialize CourseAssigments
        public ICollection<CourseAssignment> CourseAssignments
        {
            get
            {
                return _courseAssignments ?? (_courseAssignments = new List<CourseAssignment>());
            }
            set
            {
                _courseAssignments = value;
            }
        }

        public OfficeAssignment OfficeAssignment { get; set; }
        /*As regras de negócio do Contoso Universidade indicam que um instrutor pode ter apenas, no máximo, um escritório; portanto, a propriedade OfficeAssignment contém uma única entidade OfficeAssignment (que pode ser nulo se o escritório não está atribuído).*/
    }
}

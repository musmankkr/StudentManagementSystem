using StudentManagementSystemAPI.Models;
using System.Collections.Generic;

namespace StudentManagementSystemAPI.Data
{
    public class StudentStore
    {
        public static List<StudentModel> StudentList = new List<StudentModel>
        {
            new StudentModel{Id=1, Firstname ="Usman" , Dateofbirth = new System.DateTime(2012,12,12) ,
                Gender="Male",Lastname= "Khokhar" },

            new StudentModel{Id=2,Firstname ="Alina" , Dateofbirth = new System.DateTime(1990,12,12),
                Gender="Female",Lastname= "Ahmed"  },

            new StudentModel{Id=3 ,Firstname ="Musthaq" , Dateofbirth = new System.DateTime(1990,12,12),
                Gender="Male",Lastname= "Amjad"  },

            new StudentModel{Id=4,Firstname ="Bashir" , Dateofbirth = new System.DateTime(1998,12,12),
                Gender="Male",Lastname= "Irtiza"  }
            };
    }
}

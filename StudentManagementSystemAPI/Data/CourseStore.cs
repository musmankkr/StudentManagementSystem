using StudentManagementSystemAPI.Models;
using System.Collections.Generic;

namespace StudentManagementSystemAPI.Data
{
    public class CourseStore
    {
        public static List<CourseModel> CourseList = new List<CourseModel>
        {
            new CourseModel{ CourseId =1 , CourseCode="E" , CourseCredit=3 , CourseName="English" },

            new CourseModel{ CourseId =2 , CourseCode="WP" , CourseCredit=3 , CourseName="Web Programming" },

            new CourseModel{ CourseId =3 , CourseCode="DS" , CourseCredit=3 , CourseName="Data Structures" },

            new CourseModel{ CourseId =4 , CourseCode="A1" , CourseCredit=3 , CourseName="Calculus" }
            };
    }
}

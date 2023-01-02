using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystemAPI.Models
{
    public class AttendanceModel
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
    }
}

//export interface AttendanceViewModel
//{
//    id: number;
//  status: string;
//  studentId: number;
//  studentName: string;
//  date: string;
//  courseId: number;
//  courseName: string;
//}
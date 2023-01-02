using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystemAPI.DbContexts;
using StudentManagementSystemAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.Controllers
{
    [Route("api/AttendanceApi")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {

        private IApplicationDbContext _context;

        public AttendanceController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> GetAttendanceAsync()
        {
            var attendance = await _context.Attendance.ToListAsync();
            if (attendance == null) return NotFound();
            return Ok(attendance);
        }

        [HttpGet("{id:int}", Name = "GetAttendanceAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AttendanceModel>> GetAttendanceAsync(int id)
        {
            var attendance = await _context.Attendance.Where(a => a.StudentId == id).FirstOrDefaultAsync();
            if (attendance == null) return NotFound();
            return Ok(attendance);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AttendanceModel>> CreateAttendance(AttendanceModel Attendance)
        {
            _context.Attendance.Add(Attendance);
            await _context.SaveChanges();
            return Ok(Attendance);
        }

        [HttpGet("{studentId:int},{courseId:int}", Name = "GetAttendanceWithStudentAndCourse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AttendanceDetail>> GetAttendanceWithStudentAndCourse(int studentId, int courseId)
        {
            var course = _context.Courses.Where(c => c.CourseId == courseId).FirstOrDefault();
            if (course == null)
            {
                ModelState.AddModelError("Custom Error", "Invalid Course");
                return BadRequest(ModelState);
            }
            var student = _context.Students.Where(s => s.Id == studentId).FirstOrDefault();
            if (student == null)
            {
                ModelState.AddModelError("Custom Error", "Invalid studentid");
                return BadRequest(ModelState);
            }
            var attendance = _context.Attendance.Where(a => (a.CourseId == courseId) && a.StudentId == studentId).FirstOrDefault();
            if (attendance == null)
            {
                ModelState.AddModelError("Custom Error", "No attendance found");
                return BadRequest(ModelState);
            }
            AttendanceDetail attendanceDetail = new AttendanceDetail();
            attendanceDetail.student = student;
            attendanceDetail.course = course;
            attendanceDetail.attendance = attendance;
            return Ok(attendanceDetail);
        }

        [HttpDelete("({id:int})", Name = "DeleteAttendance")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendance.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (attendance == null) return NotFound();
            _context.Attendance.Remove(attendance);
            await _context.SaveChanges();
            return Ok(attendance.Id);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("({id:int})", Name = "DeleteAttendance")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] AttendanceModel attendanceUpdate)
        {
            var attendance = _context.Attendance.Where(a => a.Id == id).FirstOrDefault();
            if (attendance == null) return NotFound();
            else
            {
                attendance.Status = attendanceUpdate.Status;
                attendance.StudentId = attendanceUpdate.StudentId;
                await _context.SaveChanges();
                return Ok(attendance.Id);
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("({id:int})", Name = "UpdatePartialAttendance")]
        public IActionResult UpdatePartialAttendance(int id, [FromBody] JsonPatchDocument<AttendanceModel> patchDto)

        {
            return NoContent();
        }

    }
}

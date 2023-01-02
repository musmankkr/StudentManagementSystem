using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystemAPI.Data;
using StudentManagementSystemAPI.DbContexts;
using StudentManagementSystemAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.Controllers
{
    [Route("api/GradeApi")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        IApplicationDbContext _context;

        public GradeController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GradeModel>> GetGrades()
        {
            return Ok(_context.Courses);
        }

        [HttpGet("{id:int}", Name = "GetGrades")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GradeModel> GetGrades(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var course = _context.Grades.FirstOrDefault(u => u.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            else
                return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GradeModel>> CreateGrades(GradeModel grade)
        {
            var obj = _context.Grades.FirstOrDefault(u => u.Grade.ToLower() == grade.Grade.ToLower());

            if (obj != null)
            {
                ModelState.AddModelError("Custom Error", "course alreay exists");
                return BadRequest(ModelState);
            }
            if (grade == null)
            {
                return BadRequest(grade);
            }
            if (grade.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            _context.Grades.Add(grade);
            await _context.SaveChanges();
            return CreatedAtRoute("Getcourse", new { id = grade.Id }, grade);
        }

        [HttpDelete("({id:int})", Name = "DeleteGrades")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteGrades(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var course = _context.Grades.FirstOrDefault(u => u.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            _context.Grades.Remove(course);
            return NoContent();
        }

        [HttpGet("{studentId:int},{courseId:int}", Name = "GetGradeDetails")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AttendanceDetail>> GetGradeDetails(int studentId, int courseId)
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
            var attendance = _context.Grades.Where(a => (a.courseId == courseId) && a.studentId == studentId).FirstOrDefault();
            if (attendance == null)
            {
                ModelState.AddModelError("Custom Error", "No Grade found");
                return BadRequest(ModelState);
            }
            GradeDetails gradeDetail = new GradeDetails();
            gradeDetail.Student = student;
            gradeDetail.Course = course;
            gradeDetail.Grade = attendance;
            return Ok(gradeDetail);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("({id:int})", Name = "UpdateGrades")]
        public IActionResult UpdateGrades(int id, [FromBody] GradeModel grade)
        {
            if (id == 0 || id != grade.Id)
            {
                ModelState.AddModelError("CustomError", "Invalid Id");
                return BadRequest(ModelState);
            }

            var _grade = _context.Grades.FirstOrDefault(u => u.Id == id);

            _grade.Id = grade.Id;
            _grade.Grade = grade.Grade;

            return NoContent();
        }
    }
}

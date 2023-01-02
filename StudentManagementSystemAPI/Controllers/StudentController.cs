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
    [Route("api/StudentApi")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IApplicationDbContext _context;
        public StudentController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();
            if (students == null) return NotFound();
            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentModel> GetStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var student = _context.Students.FirstOrDefault(u => u.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            else
                return Ok(student);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentModel>> CreateStudent(StudentModel student)
        {
            var obj = _context.Students.Where(u => u.Firstname.ToLower() == student.Firstname.ToLower()).FirstOrDefault();
            if ( obj != null)
            {
                ModelState.AddModelError("Custom Error", "Student alreay exists");
                return BadRequest(ModelState);
            }
            if (student == null)
            {
                return BadRequest(student);
            }
            if (student.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            _context.Students.Add(student);
            await _context.SaveChanges();
            return CreatedAtRoute("Getstudent", new { id = student.Id }, student);
        }

        [HttpDelete("({id:int})", Name = "Deletestudent")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteStudent(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var student = _context.Students.FirstOrDefault(u => u.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            StudentStore.StudentList.Remove(student);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("({id:int})", Name = "UpdateStudent")]
        public IActionResult Updatestudent(int id, [FromBody] StudentModel student)
        {
            if (id == 0 || id != student.Id)
            {
                ModelState.AddModelError("CustomError", "Invalid Id");
                return BadRequest(ModelState);
            }

            var _student = _context.Students.FirstOrDefault(u => u.Id == id);

            _student.Firstname = student.Firstname;
            _student.Dateofbirth = student.Dateofbirth;
            _student.Gender = student.Gender;
              
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("({id:int})", Name = "UpdatePartialStudent")]
        public IActionResult UpdatePartialStudent(int id, [FromBody] JsonPatchDocument<StudentModel> patchDto)
        {
            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }
            var student = _context.Students.FirstOrDefault(u => u.Id == id);
            if (student == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(student, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}

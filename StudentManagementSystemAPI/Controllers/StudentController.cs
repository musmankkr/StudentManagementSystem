using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystemAPI.Data;
using StudentManagementSystemAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystemAPI.Controllers
{
    [Route("api/StudentApi")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentModel>> GetStudents()
        {
            return Ok(StudentStore.StudentList);
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

            var student = StudentStore.StudentList.FirstOrDefault(u => u.Id == id);

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
        public ActionResult<StudentModel> CreateStudent(StudentModel student)
        {
            if (StudentStore.StudentList.FirstOrDefault(u => u.Firstname.ToLower() == student.Firstname.ToLower()) != null)
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
            student.Id = StudentStore.StudentList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            StudentStore.StudentList.Add(student);
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
            var student = StudentStore.StudentList.FirstOrDefault(u => u.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            StudentStore.StudentList.Remove(student);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("({id:int})", Name = "DeleteStudent")]
        public IActionResult Updatestudent(int id, [FromBody] StudentModel student)
        {
            if (id == 0 || id != student.Id)
            {
                ModelState.AddModelError("CustomError", "Invalid Id");
                return BadRequest(ModelState);
            }

            var _student = StudentStore.StudentList.FirstOrDefault(u => u.Id == id);

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
            var student = StudentStore.StudentList.FirstOrDefault(u => u.Id == id);
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

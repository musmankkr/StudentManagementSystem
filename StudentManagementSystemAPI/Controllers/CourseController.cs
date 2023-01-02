using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystemAPI.Data;
using StudentManagementSystemAPI.DbContexts;
using StudentManagementSystemAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.Controllers
{
    [Route("api/CourseApi")]
    [ApiController]
    public class courseController : ControllerBase
    {
        private IApplicationDbContext _context;

        public courseController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CourseModel>> GetCourses()
        {
            return Ok(_context.Courses);
        }

        [HttpGet("{id:int}", Name = "GetCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CourseModel> GetCourse(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var course = _context.Courses.FirstOrDefault(u => u.CourseId == id);

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
        public async Task<ActionResult<CourseModel>> CreateCourse(CourseModel course)
        {
            var obj = _context.Courses.FirstOrDefault(u => u.CourseName.ToLower() == course.CourseName.ToLower());

            if ( obj != null)
            {
                ModelState.AddModelError("Custom Error", "course alreay exists");
                return BadRequest(ModelState);
            }
            if (course == null)
            {
                return BadRequest(course);
            }
            if (course.CourseId > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            _context.Courses.Add(course);
            await _context.SaveChanges();
            return CreatedAtRoute("Getcourse", new { id = course.CourseId }, course);
        }

        [HttpDelete("({id:int})", Name = "DeleteCourse")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult>DeleteCourse(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var course = _context.Courses.FirstOrDefault(u => u.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }
            CourseStore.CourseList.Remove(course);
            await _context.SaveChanges();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("({id:int})", Name = "DeleteCourse")]
        public IActionResult Updatecourse(int id, [FromBody] CourseModel course)
        {
            if (id == 0 || id != course.CourseId)
            {
                ModelState.AddModelError("CustomError", "Invalid Id");
                return BadRequest(ModelState);
            }

            var _course = _context.Courses.FirstOrDefault(u => u.CourseId == id);

            _course.CourseName = course.CourseName;
            _course.CourseCode = course.CourseCode;
            _course.CourseCredit = course.CourseCredit;

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("({id:int})", Name = "UpdatePartialcourse")]
        public IActionResult UpdatePartialCourse(int id, [FromBody] JsonPatchDocument<CourseModel> patchDto)
        {
            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }
            var course = _context.Courses.FirstOrDefault(u => u.CourseId == id);
            if (course == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(course, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}

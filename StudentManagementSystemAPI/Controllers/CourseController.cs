using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystemAPI.Data;
using StudentManagementSystemAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystemAPI.Controllers
{
    [Route("api/CourseApi")]
    [ApiController]
    public class courseController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CourseModel>> GetCourses()
        {
            return Ok(CourseStore.CourseList);
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

            var course = CourseStore.CourseList.FirstOrDefault(u => u.CourseId == id);

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
        public ActionResult<CourseModel> CreateCourse(CourseModel course)
        {
            if (CourseStore.CourseList.FirstOrDefault(u => u.CourseName.ToLower() == course.CourseName.ToLower()) != null)
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
            course.CourseId = CourseStore.CourseList.OrderByDescending(u => u.CourseId).FirstOrDefault().CourseId + 1;
            CourseStore.CourseList.Add(course);
            return CreatedAtRoute("Getcourse", new { id = course.CourseId }, course);
        }

        [HttpDelete("({id:int})", Name = "DeleteCourse")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteCourse(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var course = CourseStore.CourseList.FirstOrDefault(u => u.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }
            CourseStore.CourseList.Remove(course);
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

            var _course = CourseStore.CourseList.FirstOrDefault(u => u.CourseId == id);

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
            var course = CourseStore.CourseList.FirstOrDefault(u => u.CourseId == id);
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

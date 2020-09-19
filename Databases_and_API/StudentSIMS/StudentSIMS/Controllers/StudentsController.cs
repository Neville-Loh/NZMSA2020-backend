using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSIMS.Data;
using StudentSIMS.Models;

namespace StudentSIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _context;

        public StudentsController(StudentContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await _context.Student.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.studentId)
            {
                return BadRequest();
            }

            var updateStudent = await _context.Student.FirstOrDefaultAsync(s => s.studentId == student.studentId);
            _context.Entry(updateStudent).State = EntityState.Modified;

            updateStudent.firstName = student.firstName;
            updateStudent.lastName = student.lastName;
            updateStudent.emailAddress = student.emailAddress;
            updateStudent.phoneNumber = student.phoneNumber;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            student.timeCreated = DateTime.Now;
            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.studentId }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.studentId == id);
        }


        // End point for Adding Adress for student
        [HttpPost("{studentId}/AddAddress")]
        public async Task<IActionResult> AddAddressForStudent(int studentId, Address newAddress)
        {
            if (studentId != newAddress.studentId)
            {
                return BadRequest();
            }

            var existingStudent = _context.Student.Where(s => s.studentId == studentId).Include(s => s.addresses).SingleOrDefault();

            if (existingStudent != null)
            {
                existingStudent.addresses.Add(newAddress);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound("Student Id does not exist");
            }

            return Ok();
        }

        // End point for Changing Adresses for student
        [HttpPut("{studentId}/{addressId}/UpdateAddress")]
        public async Task<IActionResult> UpdateAddressForStudent(int studentId, int addressId, Address updatedAddress)
        {

            if (studentId != updatedAddress.studentId || addressId != updatedAddress.addressId)
            {
                return BadRequest("Conflicting request, multiple student id or address id");
            }

            var existingStudent = _context.Student.Where(s => s.studentId == studentId).Include(s => s.addresses).SingleOrDefault();

            if (existingStudent != null)
            {
                var existingAddress = existingStudent.addresses.Where(add => add.addressId == updatedAddress.addressId).SingleOrDefault();

                if (existingAddress != null)
                {
                    _context.Entry(existingAddress).CurrentValues.SetValues(updatedAddress);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound($"No address with address id is found for student");
                }
            }
            else
            {
                return NotFound($"Student with studentId={studentId} does not exist");
            }


            return Ok();
        }
    }
}

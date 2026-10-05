









using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Models.Repositories;

namespace StudentManagementApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repository;


        public StudentController()
        {
            _repository =
                new StudentRepository();
        }


        // =====================================================
        // INDEX
        // =====================================================

        public async Task<IActionResult> Index()
        {
            List<Student> students =
                await _repository.GetAllStudentsAsync();


            return View(students);
        }



        // =====================================================
        // CREATE - GET
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        // =====================================================
        // CREATE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }


            await _repository.AddStudentAsync(student);


            TempData["SuccessMessage"] =
                "Student added successfully.";


            return RedirectToAction(nameof(Index));
        }



        // =====================================================
        // EDIT - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Student? student =
                await _repository.GetStudentByIdAsync(id);


            if (student == null)
            {
                return NotFound();
            }


            return View(student);
        }



        // =====================================================
        // EDIT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }


            await _repository.UpdateStudentAsync(student);


            TempData["SuccessMessage"] =
                "Student updated successfully.";


            return RedirectToAction(nameof(Index));
        }



        // =====================================================
        // DELETE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteStudentAsync(id);


            TempData["SuccessMessage"] =
                "Student deleted successfully.";


            return RedirectToAction(nameof(Index));
        }
    }
}
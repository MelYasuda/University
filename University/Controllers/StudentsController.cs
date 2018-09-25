using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
    public class StudentsController : Controller
    {
      [HttpGet("/students")]
      public ActionResult Index()
      {
        List<Student> allStudents = Student.GetAll();
        return View(allStudents);
      }
    }
}

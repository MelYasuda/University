using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;
using System;

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
      [HttpGet("/students/new")]
      public ActionResult CreateForm()
      {
        return View();
      }

      [HttpPost("/students")]
      public ActionResult Create()
      {
        Student newStudent = new Student(Request.Form["name"], DateTime.Parse(Request.Form["date"]));
        newStudent.Save();
        return RedirectToAction("Index");
      }

    }

}

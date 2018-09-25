using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;
using System;

namespace University.Controllers
{
    public class CoursesController : Controller
    {
      [HttpGet("/courses")]
      public ActionResult Index()
      {
        List<Course> allCourses = Course.GetAll();
        return View(allCourses);
      }
      [HttpGet("/courses/new")]
      public ActionResult CreateForm()
      {
        return View();
      }

      [HttpPost("/courses")]
      public ActionResult Create()
      {
        Course newCourse = new Course(Request.Form["description"], Request.Form["course_number"]);
        newCourse.Save();
        return RedirectToAction("Index");
      }

      [HttpGet("/courses/{id}")]
      public ActionResult Details(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course selectedCourse = Course.Find(id);
        List<Student> listStudents = selectedCourse.GetStudents();
        model.Add("selectedCourse", selectedCourse);
        model.Add("listStudents", listStudents);
        return View(model);

      }

    }

}

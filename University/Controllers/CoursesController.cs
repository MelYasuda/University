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

    }

}

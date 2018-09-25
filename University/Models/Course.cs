using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University;

namespace University.Models
{
  public class Course
  {
    private int _id;
    private string _description;
    private string _course_number;

    public Course(string description, string courseNumber, int id=0)
    {
      _id = id;
      _description = description;
      _course_number = courseNumber;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetDescription()
    {
      return _description;
    }

    public string GetCourseNumber()
    {
      return _course_number;
    }


    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (description, course_number) VALUES (@description, @course_number);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter course_number = new MySqlParameter();
      course_number.ParameterName = "@course_number";
      course_number.Value = this._course_number;
      cmd.Parameters.Add(course_number);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseDescription = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);

        Course newCourse = new Course(courseDescription, courseNumber, courseId);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int courseId = 0;
      string courseDescription = "";
      string courseNumber = "";

      rdr.Read();
      courseId = rdr.GetInt32(0);
      courseDescription = rdr.GetString(1);
      courseNumber = rdr.GetString(2);

      Course newCourse = new Course(courseDescription, courseNumber, courseId);
      conn.Close();
      if (conn != null)
    {
      conn.Dispose();
    }

    return newCourse;
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = _id;
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@StudentId";
      student_id.Value = newStudent.GetId();
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Student> GetStudents()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT students.* FROM courses
      JOIN students_courses ON (courses.id = students_courses.course_id)
      JOIN students ON (students_courses.student_id = students.id)
      WHERE courses.id = @CourseId;";

      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = _id;
      cmd.Parameters.Add(courseIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Student> students = new List<Student>{};

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, enrollmentDate, studentId);
        students.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return students;
    }
  }
}

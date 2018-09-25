using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University;

namespace University.Models
{
  public class Student
  {
    private int _id;
    private string _name;
    private DateTime _date;

    public Student( string name, DateTime date, int id = 0)
    {
      _id = id;
      _name = name;
      _date = date;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public DateTime GetDate()
    {
      return _date;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, date) VALUES (@name, @date);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter date = new MySqlParameter();
      date.ParameterName = "@date";
      date.Value = this._date;
      cmd.Parameters.Add(date);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentDate = rdr.GetDateTime(2);

        Student newStudent = new Student(studentName, studentDate, studentId);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

      public static Student Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int studentId = 0;
        string studentName = "";

        rdr.Read();
        studentId = rdr.GetInt32(0);
        studentName = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);

        Student newStudent = new Student(studentName, enrollmentDate, studentId);
        conn.Close();
        if (conn != null)
      {
        conn.Dispose();
      }

      return newStudent;
      }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = newCourse.GetId();
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@StudentId";
      student_id.Value = _id;
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Course> GetCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT courses.* FROM students
      JOIN students_courses ON (students.id = students_courses.student_id)
      JOIN courses ON (students_courses.course_id = courses.id)
      WHERE students.id = @StudentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Course> courses = new List<Course>{};

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseDescription = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseDescription, courseNumber, courseId);
        courses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return courses;
    }
  }
}

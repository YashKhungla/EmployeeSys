using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using EmployeeSys.Models;

namespace EmployeeSys.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: Registration page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Register user
        [HttpPost]
        public IActionResult Index(RegisterModel model)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("EmployeeSysContext");

                // Open connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to insert user
                    string sql = @"INSERT INTO [dbo].[User] (Name, Surname, Birthdate, Employeenumber, Manager, Role, Salary, Email, Password) 
                            VALUES (@Name, @Surname, @Birthdate, @Employeenumber, @Manager, @Role, @Salary, @Email, @Password)";

                    // Create command object
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Surname", model.Surname);
                        command.Parameters.AddWithValue("@Birthdate", model.Birthdate);
                        command.Parameters.AddWithValue("@Employeenumber", model.Employeenumber);
                        command.Parameters.AddWithValue("@Manager", model.Manager);
                        command.Parameters.AddWithValue("@Role", model.Role);
                        command.Parameters.AddWithValue("@Salary", model.Salary);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Password", model.Password);

                        // Execute command
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login"); // Redirect to login page after successful registration
            }
            catch (Exception ex)
            {
                // Handle exception
                return RedirectToAction("Error", "Home"); // Redirect to error page
            }
        }

        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Employee Management System - Login";
            return View();
        }

        // POST: Handle login
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("EmployeeSysContext");

                // Open connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to fetch user data by email and password
                    string sql = @"SELECT COUNT(*) FROM [dbo].[User] WHERE Email = @Email AND Password = @Password";

                    // Create command object
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);

                        // Execute command to check if user exists with the provided credentials
                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            // Authentication successful, redirect to home page
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // Authentication failed, redirect back to the login page with an error message
                            ViewBag.ErrorMessage = "Invalid email or password.";
                            return View();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return RedirectToAction("Error", "Home"); // Redirect to error page
            }
        }
    }
}

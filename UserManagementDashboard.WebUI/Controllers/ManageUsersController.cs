using Microsoft.AspNetCore.Mvc;
using UserManagementDashboard.Data;
using UserManagementDashboard.Models;

namespace UserManagementDashboard.WebUI.Controllers;

public class ManageUsersController : Controller
{
    private readonly UserManagementDashboardContext _context;

    public ManageUsersController(UserManagementDashboardContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> ManageUsers(int? id)
    {
        var listOfUsers = new List<User>(); 
        User selectedUser = null;

        try
        {
            listOfUsers = await UsersData.GetList(_context);

            if (id.HasValue)
            {
                selectedUser = await UsersData.GetUser(id.Value, _context);
            }
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }

        ViewBag.Users = listOfUsers;

        return View(selectedUser);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserAsync(User user)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await UsersData.GetUser(user.Id, _context);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;

                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = user.Password; 
                }

                _context.SaveChanges();
            }

            return RedirectToAction("ManageUsers");
        }

        ViewBag.Users = _context.Users.ToList();
        return View("ManageUsers", user);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveAsync(int id)
    {
        var user = await UsersData.GetUser(id, _context);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        return RedirectToAction("ManageUsers");
    }


    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddUser(User user, string confirmPassword)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        if (user.Password != confirmPassword)
        {
            ModelState.AddModelError("Password", "Passwords do not match");
            return View(user);
        }

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User successfully added!";
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
            return View(user);
        }

        return RedirectToAction("ManageUsers");
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public async Task<IActionResult> ManageUsers()
    {
        var listOfUsers = new List<User>();
   
        try
        {
            listOfUsers = await UsersData.GetList(_context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }

        return View(listOfUsers);
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
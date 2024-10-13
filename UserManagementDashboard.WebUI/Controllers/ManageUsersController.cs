using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserManagementDashboard.Data;
using UserManagementDashboard.Models;

namespace UserManagementDashboard.WebUI.Controllers;

public class ManageUsersController(UserManagementDashboardContext context) : Controller
{
    private readonly UserManagementDashboardContext _context = context;

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

    public async Task<IActionResult> AddOrUpdate(int? id)
    {
        User? user = null;
        try
        {

            if (id == null) return View();
            user = await UsersData.GetUser((int)id, _context);
            if (user == null)
                return RedirectToAction("Index", "NotFound", new { entity = "User", backUrl = "/ManageUsers/" });
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrUpdate(User user)
    {
        if (!ModelState.IsValid)
        {
            return (user.Id == 0) ? View() : View(user);
        }

        try
        {
            if (user.Id == 0)
                await UsersData.Insert(user, _context);
            else
                await UsersData.Update(user, _context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
            return (user.Id == 0) ? View() : View(user);
        }
        return RedirectToAction(nameof(Index));
    }
}

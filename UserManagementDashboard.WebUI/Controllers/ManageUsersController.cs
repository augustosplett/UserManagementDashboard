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
    public async Task<IActionResult> RemoveAsync(int id)
    {
        var user = await UsersData.GetUser(id, _context);
        if (user != null)
        {
            await UsersData.Delete(user, _context);
        }

        await LoadUsersAsync();
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

    private async Task LoadUsersAsync()
    {
        ViewBag.Users = await UsersData.GetList(_context);
    }

    // Ação para atualizar informações pessoais
    [HttpPost]
    public async Task<IActionResult> UpdatePersonalInfo(User user)
    {
        //remove os campos que não devem ser validados por esse método
        ModelState.Remove("Email");
        ModelState.Remove("Password");

        if (ModelState.IsValid)
        {
            var existingUser = await UsersData.GetUser(user.Id, _context);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;

                await UsersData.Update(existingUser, _context);
            }

            TempData["SuccessMessage"] = "Personal information updated successfully!";
            return RedirectToAction("ManageUsers", new { id = user.Id });
        }

        TempData["DangerMessage"] = "Failed to update personal information!";
        ViewBag.Users = await UsersData.GetList(_context);
        return View("ManageUsers", user);
    }

    // Ação para atualizar informações de segurança (e-mail e senha)
    [HttpPost]
    public async Task<IActionResult> UpdateSecurityInfo(User user)
    {
        // Removendo os campos não relacionados ao formulário de segurança
        ModelState.Remove("FirstName");
        ModelState.Remove("LastName");

        if (ModelState.IsValid)
        {
            var existingUser = await UsersData.GetUser(user.Id, _context);
            if (existingUser != null)
            {
                existingUser.Email = user.Email;

                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = user.Password;
                }

                await UsersData.Update(existingUser, _context);
            }

            TempData["SuccessMessage"] = "Security information updated successfully!";
            return RedirectToAction("ManageUsers", new { id = user.Id });
        }

        TempData["DangerMessage"] = "Failed to update security information!";
        ViewBag.Users = await UsersData.GetList(_context);
        return View("ManageUsers", user);
    }
}
using Microsoft.EntityFrameworkCore;
using UserManagementDashboard.Models;

namespace UserManagementDashboard.Data;

public class UsersData
{
    public static async Task Insert(User user, UserManagementDashboardContext context)
    {
        // TODO: Add the password encrypt
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public static async Task Update(User user, UserManagementDashboardContext context)
    {
        // TODO: Add the password encrypt
        _ = context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public static async Task<User?> GetUser(int userId, UserManagementDashboardContext context)
    {
        return await context.Users.FindAsync(userId);
    }

    public static async Task<List<User>> GetList(UserManagementDashboardContext context)
    {
        return await context.Users.ToListAsync();
    }

    public static async Task Delete(User user, UserManagementDashboardContext context)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}
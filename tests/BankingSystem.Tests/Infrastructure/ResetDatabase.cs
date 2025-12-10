using System;
using BankingSystem.App.Data;

namespace BankingSystem.Tests.Infrastructure
{
    public static class ResetDatabase
    {
        public static void Reset(AppDbContext db)
        {
            //db.Users.RemoveRange(db.Users);
            //db.SaveChanges();
        }
    }
}
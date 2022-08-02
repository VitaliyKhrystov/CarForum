using Microsoft.AspNetCore.Identity;
using System;

namespace CarForum.Models
{
    public class User: IdentityUser
    {
        public DateTime Date { get; set; }
    }
}

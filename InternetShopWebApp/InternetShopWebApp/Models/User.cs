using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InternetShopWebApp.Models
{
    public class User:IdentityUser
    {
        public int NormalCode { get; set; }
    }
}

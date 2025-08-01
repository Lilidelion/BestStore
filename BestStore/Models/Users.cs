using Microsoft.AspNetCore.Identity;

namespace BestStore.Models
{
    public class Users : IdentityUser
    {
        public string Fullame { get; set; } = "";
        public string FullName { get; internal set; } = "";
    }
}

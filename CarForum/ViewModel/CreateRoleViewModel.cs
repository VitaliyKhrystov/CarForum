
using System.ComponentModel.DataAnnotations;


namespace CarForum.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

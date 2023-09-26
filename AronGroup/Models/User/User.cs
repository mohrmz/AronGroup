using AronGroup.Models.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AronGroup.Models.User;

public class User : IdentityUser<int>, IEntity
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    public Guid Token { get; set; }

}


using AronGroup.Models.Base;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AronGroup.Models.User;

public class Role : IdentityRole<int>, IEntity
{
    [Required]
    [StringLength(100)]
    public string Description { get; set; }
}

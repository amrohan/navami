using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace navami.Models;

public partial class UserMaster
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? Mobile { get; set; }

    public string? Password { get; set; }

    public bool? IsDeactivated { get; set; }

    // CreatedAt
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}

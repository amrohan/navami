﻿using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;
}

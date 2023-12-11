using System;
using System.Collections.Generic;

namespace sifoodproject.Models
{
    public partial class Admin
    {
        public int Id { get; set; }

        public string? Account { get; set; }

        public byte[]? Password { get; set; }

        public string? Name { get; set; }

        public byte[]? PasswordSalt { get; set; }
    }
}


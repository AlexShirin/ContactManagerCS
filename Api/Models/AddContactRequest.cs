﻿namespace ContactManagerCS.Models
{
    public class AddContactRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Work { get; set; }
    }
}

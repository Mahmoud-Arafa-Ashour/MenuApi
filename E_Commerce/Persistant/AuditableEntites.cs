﻿namespace E_Commerce.Persistant
{
    public class AuditableEntites
    {
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ApplicationUser CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }
    }
}

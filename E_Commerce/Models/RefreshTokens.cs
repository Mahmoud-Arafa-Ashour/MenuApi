﻿namespace E_Commerce.Models;

[Owned]
public class RefreshTokens
{
    public string Token {  get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedOn { get; set; }
    public bool IsExpird => DateTime.UtcNow >= ExpiresOn;
    public bool IsActive => !IsExpird && RevokedOn is null;
}

﻿namespace BankAccountManagement.Models;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public int CreditRating { get; set; }
}

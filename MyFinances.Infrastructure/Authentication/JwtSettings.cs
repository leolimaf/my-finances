﻿namespace MyFinances.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; }
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public int Minutes { get; init; }
    public int DaysToExpiry { get; init; }
}
﻿namespace MyFinances.Application.Authentication.Common.Responses;

public record LoginUsuarioResponse(
    bool Authenticated,
    string Created,
    string Expiration,
    string AccessToken,
    string RefreshToken
);
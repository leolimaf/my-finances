﻿using System.ComponentModel.DataAnnotations;

namespace MinhasFinancas.Auth.DTOs;

public class LoginUsuarioDTO
{
    public string? Nome { get; set; }
    
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required,  DataType(DataType.Password)]
    public string Senha { get; set; }
}
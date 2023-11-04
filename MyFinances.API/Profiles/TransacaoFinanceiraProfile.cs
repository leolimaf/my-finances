﻿using AutoMapper;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.Domain.Models;

namespace MyFinances.API.Profiles;

public class TransacaoFinanceiraProfile : Profile
{
    public TransacaoFinanceiraProfile()
    {
        CreateMap<CreateTransacaoDTO, TransacaoFinanceira>();
        CreateMap<TransacaoFinanceira, ReadTransacaoDTO>();
        CreateMap<UpdateTransacaoDTO, TransacaoFinanceira>();
    }
}
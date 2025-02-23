﻿using Application.Dtos;
using Application.Dtos.category;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.handlers.transactions.queries.Transactions.GetUserTransactions;
using AutoMapper;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Core.common;

namespace Application.mappers;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<TransactionCreateDto, CreateTransactionCommand>();
        CreateMap<BaseSearchDto, GetUserTransactionsQuery>();
        CreateMap<GetUserTransactionsQuery, BaseBudgetSearchFilter>();
        CreateMap<Category, CategoryLookupDto>();
        CreateMap<BaseItemListDto, CancelTransactionsCommand>()
            .ForMember(dest => dest.TransactionIds, opt => opt.MapFrom(dto => dto.ItemsIds));

        CreateMap<Transaction, TransactionInfo>()
            .ForMember(dest => dest.Budget, opt => opt.MapFrom(model => model.Budget));

        CreateMap<TransactionEntity, Transaction>()
            .ForPath(model => model.Amount, opt => opt.MapFrom(ent => ent.Money.Amount))
            .ForPath(model => model.Currency, opt => opt.MapFrom(ent => ent.Money.Currency));
        
        CreateMap<Transaction, TransactionEntity>()
            .ForPath(ent => ent.Money.Amount, opt => opt.MapFrom(model => model.Amount))
            .ForPath(ent => ent.Money.Currency, opt => opt.MapFrom(model => model.Currency));
    }
}
using Application.Dtos;
using Application.handlers.transactions.commands.CancelTransactions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.handlers.transactions.queries.GetUserTransactions;
using AutoMapper;
using Domain.Models;

namespace Application.mappers;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<TransactionCreateDto, CreateTransactionCommand>();
        CreateMap<BaseFilterSearchDto, GetUserTransactionsQuery>();
        CreateMap<GetUserTransactionsQuery, BaseSearchDto>();
        CreateMap<Category, CategoryLookupDto>();
        CreateMap<BaseItemListDto, CancelTransactionsCommand>()
            .ForMember(dest => dest.TransactionIds, opt => opt.MapFrom(dto => dto.ItemsIds));
        CreateMap<Transaction, TransactionLookupExtendedDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
    }
}
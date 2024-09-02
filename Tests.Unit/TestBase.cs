using Application.mappers;
using AutoMapper;

namespace Tests.Unit;

public class TestBase
{
    protected IMapper Mapper;

    protected TestBase() 
    {
        var configBuilder = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(AuthMappingProfile));
            cfg.AddProfile(typeof(BudgetMappingProfile));
            cfg.AddProfile(typeof(TransactionMappingProfile));
            cfg.AddProfile(typeof(CategoryMappingProfile));
        });

        Mapper = configBuilder.CreateMapper();
    }
}
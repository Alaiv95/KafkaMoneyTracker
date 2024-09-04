using Application.common.FileInfoConfigurators;
using Application.Dtos;
using Application.handlers.transactions.queries.Transactions.common;
using Application.mediator.interfaces;
using Application.specs;
using Infrastructure.FileUtils.dtos;
using Infrastructure.FileUtils.writers;
using Infrastructure.Repositories.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Application.handlers.transactions.queries.Transactions.DownloadTransactionsSummary;

public class
    DownloadTransactionsSummaryQueryHandler : IRequestHandler<DownloadUserTransactionsQuery, FileWriteResultDto?>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly TransactionSpecs _spec;
    private readonly FileWriterFactory _factory;
    private readonly IConfigurator<List<TransactionSummaryDto>> _infoConfigurator;

    public DownloadTransactionsSummaryQueryHandler(
        ITransactionRepository transactionRepository,
        TransactionSpecs spec,
        FileWriterFactory factory,
        IConfigurator<List<TransactionSummaryDto>> infoConfigurator
    )
    {
        _transactionRepository = transactionRepository;
        _spec = spec;
        _factory = factory;
        _infoConfigurator = infoConfigurator;
    }


    public async Task<FileWriteResultDto?> Handle(DownloadUserTransactionsQuery command)
    {
        var filter = new BaseSearchDto
        {
            BudgetId = command.BudgetId,
            UserId = command.UserId
        };

        var transactions = await _transactionRepository.SearchWithIncludeAsync(_spec.Build(filter));

        if (transactions.IsNullOrEmpty())
        {
            return null;
        }

        var transactionsData = TransactionsUtils.GetTransactionsSummaryInfo(transactions);

        var info = _infoConfigurator.Configure(command.FileType, transactionsData);
        return _factory.GetWriter(command.FileType, info).WriteFile();
    }
}
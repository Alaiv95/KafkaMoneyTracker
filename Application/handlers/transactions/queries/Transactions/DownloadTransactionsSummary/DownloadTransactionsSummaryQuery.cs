using Application.mediator.interfaces;
using Infrastructure.FileUtils;
using Infrastructure.FileUtils.dtos;

namespace Application.handlers.transactions.queries.Transactions.DownloadTransactionsSummary;

public class DownloadUserTransactionsQuery : IRequest<FileWriteResultDto?>
{
    public Guid UserId { get; set; }
    
    public Guid BudgetId { get; set; }

    public FileType FileType { get; set; }
}
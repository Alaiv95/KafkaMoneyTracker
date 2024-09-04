using Infrastructure.FileUtils;
using Infrastructure.FileUtils.dtos;

namespace Application.common.FileInfoConfigurators;

public interface IConfigurator<in T>
{
    IInputInfo Configure(FileType type, T data);
}
using Infrastructure.FileUtils;
using Core.fileUtils;

namespace Application.utils.FileInfoConfigurators;

public interface IConfigurator<in T>
{
    IInputInfo Configure(FileType type, T data);
}
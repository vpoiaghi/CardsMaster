using System.Windows.Threading;

namespace CardMasterExport
{
    public interface IThreadedExporterOwner : IExporterOwner
    {
        Dispatcher Dispatcher { get; }
    }
}

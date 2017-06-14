using System.Windows.Threading;

namespace CardMasterExport
{
    public interface IExporterOwner
    {
        Dispatcher Dispatcher { get; }

        void ExportProgressChanged(object sender, ProgressChangedArg args);
    }
}

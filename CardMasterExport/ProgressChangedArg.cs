using System;

namespace CardMasterExport
{
    public enum ProgressState
    {
        ExportInProgress,
        ExportEnded,
        TreatmentEnded
    };

    public class ProgressChangedArg
    {
        public ProgressChangedArg(ProgressState state, string message, int index, int total)
        {
            this.State = state;
            this.Message = message;
            this.Index = index;
            this.Total = total;
        }

        public int Index { get; }
        public int Total { get; }
        public string Message { get; }
        public ProgressState State { get; }
    }

}

using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace CardMasterCommon.Dialog
{
    public static class FolderDialog
    {
        public static string SelectFolder()
        {
            var f = new FileInfo(Application.ExecutablePath);
            return SelectFolder(f.Directory.FullName);
        }

        public static string SelectFolder(string currentDirectory)
        {
            string f = null;

            Thread t = new Thread(() => f = ShowDialog(currentDirectory));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return f;
        }

        private static string ShowDialog(string currentDirectory)
        {
            string folder = null;

            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = currentDirectory;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                folder = dlg.SelectedPath;
            }

            return folder;

        }



    }
}

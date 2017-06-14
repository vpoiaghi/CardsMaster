using System;
using System.Windows;

namespace CardMasterManager
{
    class MainClass
    {
        [STAThread]
        static void Main(string[] args)
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // On passe ici si l'appli est lancée en mode debug
                Start(args);
            }
            else
            {
                // On passe ici si l'appli est lancée en mode release
                try
                {
                    Start(args);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur...", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private static void Start(string[] args)
        {
            MainWindow w = new MainWindow();
            w.ShowDialog();
        }
    }
}

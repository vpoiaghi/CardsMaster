using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace CardMasterCommon.Dialog
{
    public static class PrinterDialog
    {
        public static PrintDocument SelectPrintParameters(int pageCount)
        {
            PrintDocument pDoc = new PrintDocument();

            pDoc.DefaultPageSettings.PrinterResolution.Kind = PrinterResolutionKind.High;

            pDoc.DefaultPageSettings.PrinterSettings.FromPage = 1;
            pDoc.DefaultPageSettings.PrinterSettings.ToPage = pageCount; // this.cardsList.Count;

            PrintDialog printDlg = new PrintDialog();
            printDlg.Document = pDoc;

            printDlg.AllowSomePages = true;

            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                pDoc.DefaultPageSettings.PrinterResolution.X = 300;
                pDoc.DefaultPageSettings.PrinterResolution.Y = 300;

                pDoc.OriginAtMargins = true;

                double cmToUnits = 100 / 2.54;
                int margin = (int)Math.Ceiling(1 * cmToUnits);

                pDoc.DefaultPageSettings.Margins.Top = margin;
                pDoc.DefaultPageSettings.Margins.Left = margin;
                pDoc.DefaultPageSettings.Margins.Bottom = margin;
                pDoc.DefaultPageSettings.Margins.Right = margin;
            }
            else
            {
                pDoc = null;
            }

            printDlg.Dispose();
            printDlg = null;

            return pDoc;
        }

    }
}

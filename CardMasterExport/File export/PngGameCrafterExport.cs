using CardMasterCard.Card;
using CardMasterExport.Export;
using CardMasterImageBuilder;
using CardMasterSkin.Skins;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CardMasterExport.FileExport

{
    public class PngGameCrafterExport : Exporter, IDisposable
    {
        private DirectoryInfo targetFolder = null;

        protected override bool BeforeCardsExport(ExportParameters parameters)
        {
            this.targetFolder = parameters.TargetFolder;

            return (this.targetFolder != null);
        }

        protected override void MakeCardExport(JsonCard card, JsonSkin skin)
        {
            Drawer drawer = null;

            string fileName = card.Name
                .Replace("\\", " ")
                .Replace("/", " ")
                .Replace(":", "-")
                .Replace("*", " ")
                .Replace("?", " ")
                .Replace("\"", "'")
                .Replace("<", " ")
                .Replace(">", " ")
                .Replace("|", " ");

         
            string targetFolderHD = null;
            string targetFolderSD = null;

            lock (_lock){
                targetFolderHD = this.targetFolder.FullName + "\\HD\\" + card.BackSide;
                targetFolderSD = this.targetFolder.FullName + "\\SD\\" + card.BackSide;
            }

            ImageCodecInfo encoderJpg = GetEncoder(ImageFormat.Jpeg);
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            //100 high Quality
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            drawer = new Drawer(card, skinsFile, null);
            Directory.CreateDirectory(targetFolderHD);
            Directory.CreateDirectory(targetFolderSD);

            for (int i = 1; i <= card.Nb; i++)
            {
                if (card.Warning == false)
                {

                    Image imgFrontHD = null;
                    imgFrontHD = drawer.DrawCard();
                    imgFrontHD.Save(Path.Combine(targetFolderHD, fileName + "-" + i + ".png"));



                    imgFrontHD.Save(Path.Combine(targetFolderSD, fileName + "-" + i + ".jpg"), encoderJpg, myEncoderParameters);
                    imgFrontHD.Dispose();
                    imgFrontHD = null;
                }
            }
            if (!File.Exists(Path.Combine(targetFolderHD, "0-Back" + ".png")))
            {
                Image imgBackHD = null;
                imgBackHD = drawer.DrawBackCard();
           
                imgBackHD.Save(Path.Combine(targetFolderHD, "0-Back" + ".png"));
                imgBackHD.Save(Path.Combine(targetFolderSD, "0-Back" + ".jpg"), encoderJpg, myEncoderParameters);
                imgBackHD.Dispose();
                imgBackHD = null;
            }
            drawer = null;
            

        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (var info in ImageCodecInfo.GetImageEncoders())
                if (info.FormatID == format.Guid)
                    return info;
            return null;
        }

        protected override void AfterCardsExport()
        { }

        protected override string GetProgressMessage(ProgressState state, int index, int total)
        {
            string message = "";

            if (state == ProgressState.ExportInProgress)
            {
                message = "Export en cours : " + index + " / " + total;
            }
            else if (state == ProgressState.ExportEnded)
            {
                message = "Export terminé";
            }

            return message;
        }

    }
}

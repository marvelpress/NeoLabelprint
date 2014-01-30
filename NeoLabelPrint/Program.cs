using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


using Neodynamic.SDK.Printing;
using Neodynamic.Windows.ThermalLabelEditor;

namespace NeoLabelPrint
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 2;
            while (counter > 0)
            {
                //Define a ThermalLabel object and set unit to inch and label size
                ThermalLabel tLabel = new ThermalLabel(UnitType.Inch, 4, 4);

                Console.WriteLine(tLabel.Width);
                //tLabel.GapLength = 0.2;

                //get ThermalLabel SDk install dir and get the sample images
                string imgFolder = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Neodynamic\\SDK\\ThermalLabel SDK 5.0 for .NET\\InstallDir").GetValue(null).ToString() + "\\Samples\\Images\\";


                //Define an ImageItem for Postage
                ImageItem awLogo = new ImageItem(2, 0.1);
                awLogo.SourceFile = @"C:\Users\osmith\Desktop\1st Class PPI.jpg";
                awLogo.Width = 2;
                awLogo.LockAspectRatio = LockAspectRatio.WidthBased;
                awLogo.MonochromeSettings.DitherMethod = DitherMethod.Threshold;
                awLogo.MonochromeSettings.Threshold = 80;


                //Define an ImageItem for MP logo
                ImageItem mpLogo = new ImageItem(0.1, 0.1);
                mpLogo.SourceFile = @"C:\Users\osmith\Desktop\MP - Report Header.jpg";
                mpLogo.Width = 2;
                mpLogo.LockAspectRatio = LockAspectRatio.WidthBased;
                mpLogo.MonochromeSettings.DitherMethod = DitherMethod.Threshold;
                mpLogo.MonochromeSettings.Threshold = 80;

                //Define a TextItem
                TextItem txtModelName = new TextItem(0.1, 1.75, 3.8, 0.25, "Test String: Это метка на кириллице");
                //font settings
                txtModelName.Font.Name = "Arial";
                txtModelName.Font.Unit = FontUnit.Point;
                txtModelName.Font.Size = 12;
                //white text on black background
                txtModelName.BackColor = Neodynamic.SDK.Printing.Color.Black;
                txtModelName.ForeColor = Neodynamic.SDK.Printing.Color.White;
                //padding
                txtModelName.TextPadding = new FrameThickness(0.075, 0.03, 0, 0);

                //Define a BarcodeItem
                string serialNum = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);
                BarcodeItem serialBarcode = new BarcodeItem(3, 3, 0.5, 0.5, BarcodeSymbology.QRCode, serialNum);
                serialBarcode.BorderThickness = new FrameThickness(0);

                //Add items to ThermalLabel object...
                tLabel.Items.Add(awLogo);
                tLabel.Items.Add(mpLogo);
                tLabel.Items.Add(serialBarcode);
                tLabel.Items.Add(txtModelName); 


                PrinterSettings ps = new PrinterSettings();
                ps.PrinterName = @"\\MPPRN01\WS08_LABEL_ZebraGK420d";


                using (PrintJob pj = new PrintJob(ps))
                {
                    pj.Copies = 1; // set copies
                    pj.PrintOrientation = PrintOrientation.Portrait; //set orientation
                    pj.ThermalLabel = tLabel; // set the ThermalLabel object

                    pj.ExportToPdf(@"test.pdf", 208);
                    pj.Print(); // print the ThermalLabel object                 
                }
                counter--;
            }

        }
    }
}

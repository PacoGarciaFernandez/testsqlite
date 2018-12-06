using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

namespace biodanza
{
    public partial class TextEditor : Form
    {
        private Font verdana10Font;
        private StreamReader reader;
        private System.Drawing.Printing.PrintDocument docToPrint =    new System.Drawing.Printing.PrintDocument();
        private string stringToPrint;

        public TextEditor()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {


        }

        private void PrintTextFileHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            
        }


        private void TextEditor_Load(object sender, EventArgs e)
        {
            editor.SelectionLength = 0;
            SendKeys.Send("{UP}");
            editor.Select(0, 0);
        }

        private void ReadFile()
        {
            string docName = "testPage.txt";
            string docPath = @"c:\";
            printDocument1.DocumentName = docName;
            using (FileStream stream = new FileStream(docPath + docName, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                stringToPrint = reader.ReadToEnd();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace Visualizer.Util
{
	public class PrintUtility
	{
		private string _filePath;

		public PrintUtility(string filePath)
		{
			this._filePath = filePath;
		}

		public void Print()
		{
			// Create the print dialog
			System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
			PrintDocument printDocument = new PrintDocument();
			printDocument.DocumentName = "Print Document";
			printDocument.PrintPage += new PrintPageEventHandler(this.PrintPage);
			if (printDialog.ShowDialog() == true)
			{
				printDocument.Print();
			}
		}

		// The PrintPage event is raised for each page to be printed.
		private void PrintPage(object sender, PrintPageEventArgs ev)
		{
			float yPos = 0;
			int count = 0;
			float leftMargin = ev.MarginBounds.Left;
			float topMargin = ev.MarginBounds.Top;
			string line = null;

			var printFont = new Font("Arial", 10);

			// Calculate the number of lines per page.
			float linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);

			var streamToPrint = new StreamReader(this._filePath);

			try
			{
				// Print each line of the file.
				while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
				{
					yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
					ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
					count++;
				}

				// If more lines exist, print another page.
				if (line != null)
				{
					ev.HasMorePages = true;
				}
				else
				{
					ev.HasMorePages = false;
				}
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show(e.Message);
			}
			finally
			{
				streamToPrint.Close();
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using SalonManager.Views;

namespace SalonManager.Helpers
{
    class Printer
    {
        public static void print(DataGrid grid){


           // FlowDocument doc = new FlowDocument();
            //FixedDocument fixedDoc = new FixedDocument();
            //PageContent pageContent = new PageContent();
            //FixedPage fixedPage = new FixedPage();
            //fixedPage.Children.Add(grid);
            //((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            //fixedDoc.Pages.Add(pageContent);

            //PrintWindow pWin = new PrintWindow();
            //pWin.SetDoc(fixedDoc);
            //pWin.ShowDialog();

            PrintDialog pDialog = new PrintDialog();      
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;

            double HorizontalOffset = 20;
            double VerticalOffset = 20;
            double oriHeight = grid.Height;
            double oriWidth = grid.Width;
            grid.Height = Double.NaN;
            if (grid.Width > pDialog.PrintableAreaWidth - HorizontalOffset)
            {
                grid.Width = pDialog.PrintableAreaWidth - HorizontalOffset;
            }
            grid.UpdateLayout();

            Nullable<Boolean> print = pDialog.ShowDialog();

            if (print == true)
            {
                string title = "KimWorks-SalonManager";
                grid.Measure(new Size(Double.PositiveInfinity,Double.PositiveInfinity));
                Size sizeGrid = grid.DesiredSize;
                Point ptGrid = new Point(HorizontalOffset, VerticalOffset);
                grid.Arrange(new Rect(ptGrid, sizeGrid));
                pDialog.PrintVisual(grid, title);

                double diff;
                int i = 1;
                while ((diff = sizeGrid.Height - (pDialog.PrintableAreaHeight - VerticalOffset) * i) > 0)
                {
                    //Position of the grid 
                    var ptSecondGrid = new Point(HorizontalOffset, -sizeGrid.Height + diff + VerticalOffset);

                    // Layout of the grid
                    grid.Arrange(new Rect(ptSecondGrid, sizeGrid));

                    //print
                    int k = i + 1;
                    pDialog.PrintVisual(grid, title + " (Page " + k + ")");

                    i++;
                }

                //Size pageSize = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);
                //grid.Measure(pageSize);
                //grid.Arrange(new Rect(5,5,pageSize.Width,pageSize.Width));
                //pDialog.PrintVisual(grid, "KimWorks-SalonManager");
            }
            grid.Height = oriHeight;
            grid.Width = oriWidth;
            grid.UpdateLayout();
        }
        
    }
}

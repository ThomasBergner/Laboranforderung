using Laboranforderung.Daten;
using Laboranforderung.ViewModels.FileMangement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace Laboranforderung
{
    /// <summary>
    /// Interaktionslogik für PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintWindow()
        {
            InitializeComponent();
        }

        public PrintWindow(Datensammlung datensammlung)
        {
            InitializeComponent();
            LoadContent(datensammlung);
        }


        private void LoadContent(Datensammlung datensammlung)
        {
            FlowDocument flowDocument = new FlowDocument();

            foreach (var item in datensammlung.Profile)
            {
                flowDocument.Blocks.Add(CreateProfilBlock(item));
            }
            flowDocument.ColumnWidth = 250;
            flowDocument.FontFamily = this.FontFamily;
            flowDocument.FontSize = 12;

            DocumentViewer.Document = flowDocument;
        }
        private Block CreateProfilBlock(Laborprofil profil)
        {
            Section result = new Section();
            result.BorderBrush = System.Windows.Media.Brushes.Gray;

            Paragraph profilheader = new Paragraph();
            profilheader.Inlines.Add(new Underline(new Run($"Profil: ")));
            profilheader.Inlines.Add(new Underline(new Run(profil.Profilname)));
            profilheader.Margin = new System.Windows.Thickness(0);
            result.Blocks.Add(profilheader);

            Table t = new Table();
            t.Margin = new System.Windows.Thickness(0);
            t.CellSpacing = 0;
            t.TextAlignment = System.Windows.TextAlignment.Left;
         


            TableColumn tableColumn = new TableColumn();
            tableColumn.Width = new GridLength(10D);
            t.Columns.Add(tableColumn);

            TableColumn tableColumn1 = new TableColumn();
            tableColumn1.Width = new GridLength(1, GridUnitType.Auto);
            t.Columns.Add(tableColumn1);


            TableRowGroup rowGroup = new TableRowGroup();
            t.RowGroups.Add(rowGroup);

            foreach (var item in profil.Werte)
            {
                
                TableRow row = new TableRow();
                TableCell cell0 = new TableCell(new Paragraph(new Run("-")));
                row.Cells.Add(cell0);

                Paragraph inhalt = new Paragraph();
                inhalt.Inlines.Add(new Run(item.Name));
                inhalt.Inlines.Add(new Run($" ({item.Bezeichnung})"));

                TableCell cell1 = new TableCell(inhalt);
                cell1.Padding = new System.Windows.Thickness(0);
                row.Cells.Add(cell1);

                rowGroup.Rows.Add(row);
                
            }
            
            


            result.Blocks.Add(t);


            //    foreach (var item in profil.Werte)
            //{
            //    Paragraph itemParagraph = new Paragraph();
            //    itemParagraph.Inlines.Add(new Run($"- {item.Name} ({item.Bezeichnung})"));
            //    itemParagraph.Margin = new System.Windows.Thickness(0);
            //    result.Blocks.Add(itemParagraph);
            //}

            result.BorderThickness = new System.Windows.Thickness(1);
            result.Padding = new System.Windows.Thickness(5);
            result.Margin = new System.Windows.Thickness(0, 20, 0, 20);
            result.FontFamily = new System.Windows.Media.FontFamily("Arial");

            return result;
        }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            string str = System.Windows.Markup.XamlWriter.Save(DocumentViewer.Document);
            StringReader stringReader = new StringReader(str);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            FlowDocument clonedDocument = (FlowDocument)System.Windows.Markup.XamlReader.Load(xmlReader);

            if(clonedDocument != null)
            {
                System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    clonedDocument.PageHeight = printDialog.PrintableAreaHeight;
                    clonedDocument.PageWidth = printDialog.PrintableAreaWidth;
                    clonedDocument.PagePadding = new System.Windows.Thickness(50);
                    printDialog.PrintDocument(((IDocumentPaginatorSource)clonedDocument).DocumentPaginator, "Laborprofile");
                }
            }

        }
    }
}

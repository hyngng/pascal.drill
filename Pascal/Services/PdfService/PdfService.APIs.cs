using Microsoft.UI.Xaml.Controls;
using Pascal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharpDocument = PdfSharp.Pdf.PdfDocument;
using UglyToadDocument = UglyToad.PdfPig.PdfDocument;

namespace Pascal.Services.PdfService
{
    public partial class PdfService : IPdfService
    {
        public int FindPageRanges(string filePath)
        {
            try
            {
                using UglyToadDocument document = UglyToadDocument.Open(filePath);
                return document.NumberOfPages;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void MergePdf(string outputPath, ObservableCollection<Models.PdfItem> pdfItems)
        {
            PdfSharpDocument outputDocument = new(outputPath);

            foreach (var pdfItem in pdfItems)
            {
                string inputFilePath = pdfItem.FilePath;
                List<int> selectedPages = pdfItem.PagesToProcess;

                // 리스트에 추가된 파일을 삭제하고 저장하면
                // 여기서 튕기고 세계가 멸망하고 우주가 폭발함.
                // 주의.
                PdfSharpDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                
                if (selectedPages.Count == 0)
                {
                    for (int idx = 0; idx < pdfItem.PageCount; idx++)
                        outputDocument.AddPage(inputDocument.Pages[idx]);
                }
                else
                {
                    foreach (int pageIndex in selectedPages)
                        if (pageIndex >= 0 && pageIndex < pdfItem.PageCount)
                            outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                }
                inputDocument.Close();
            }
            outputDocument.Close();
        }

        public void SplitPdf(string outputPath, ObservableCollection<Models.PdfItem> pdfItems)
        {
            foreach (var pdfItem in pdfItems)
            {
                string inputFilePath = pdfItem.FilePath;
                string outputPrefix = "output_part_";
                int splitFrom = 0;
                int splitTo = FindPageRanges(inputFilePath);
                int splitInterval = 10;

                PdfSharpDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                int partNumber = 1;

                for (int i = splitFrom; i < splitTo; i += splitInterval)
                {
                    string outputFilePath = Path.Combine(Directory.CreateDirectory(outputPath).FullName, $"{outputPrefix}{partNumber}.pdf");
                    PdfSharpDocument outputDocument = new(outputPath);
                    for (int j = i; j < i + splitInterval && j < splitTo; j++)
                        outputDocument.AddPage(inputDocument.Pages[j]);

                    outputDocument.Close();
                    partNumber++;
                }
                inputDocument.Close();
            }
        }
    }
}

using Microsoft.UI.Xaml.Controls;
using Pascal.Models;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.Services.PdfService
{
    public partial class PdfService : IPdfService
    {
        public void MergePdf(Stream stream, ObservableCollection<PdfItemToMerge> pdfItems)
        {
            // PDF 병합 기능 구현
            PdfDocument outputDocument = new PdfDocument(stream);
            foreach (var pdfItem in pdfItems)
            {
                string inputFilePath = pdfItem.FilePath;
                List<int> pagesToExtract = new List<int> { 0, 1, 2 }; // 예시: 첫 3페이지만 추출
                PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                if (pagesToExtract.Count == 0)
                {
                    // 모든 페이지를 병합
                    for (int idx = 0; idx < inputDocument.PageCount; idx++)
                        outputDocument.AddPage(inputDocument.Pages[idx]);
                }
                else
                {
                    // 지정된 페이지만 병합
                    foreach (int pageIndex in pagesToExtract)
                        if (pageIndex >= 0 && pageIndex < inputDocument.PageCount)
                            outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                }
            }
            outputDocument.Close();
        }

        public void SplitPdf()
        {
            // PDF 분할 기능 구현
        }
    }
}

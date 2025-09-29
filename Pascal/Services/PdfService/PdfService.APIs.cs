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
        public int FindPageRanges(string filePath)
        {
            try
            {
                using (PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                {
                    return document.PageCount;
                }
            }
            catch (Exception ex)
            {
                // 오류 처리 (예: 파일이 PDF가 아닌 경우)
                return 0;
            }
        }

        public void MergePdf(Stream stream, ObservableCollection<PdfItemToMerge> pdfItems)
        {
            // PDF 병합 기능 구현
            PdfDocument outputDocument = new PdfDocument(stream);
            foreach (var pdfItem in pdfItems)
            {
                string inputFilePath = pdfItem.FilePath;
                List<int> selectedPages = pdfItem.PagesToExtract;
                PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                if (selectedPages.Count == 0)
                {
                    // 모든 페이지를 병합
                    for (int idx = 0; idx < inputDocument.PageCount; idx++)
                        outputDocument.AddPage(inputDocument.Pages[idx]);
                }
                else
                {
                    // 지정된 페이지만 병합
                    foreach (int pageIndex in selectedPages)
                        if (pageIndex >= 0 && pageIndex < inputDocument.PageCount)
                            outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                }
                inputDocument.Close();
            }
            outputDocument.Close();
        }

        public void SplitPdf()
        {
            // PDF 분할 기능 구현
        }
    }
}

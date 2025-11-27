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
            //// PdfSharp 사용
            //try
            //{
            //    using PdfSharpDocument document = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
            //    return document.PageCount;
            //}
            //catch (Exception)
            //{
            //    // 오류 처리 (예: 파일이 PDF가 아닌 경우)
            //    return 0;
            //}

            // UglyToad.PdfPig 사용
            try
            {
                using var document = UglyToadDocument.Open(filePath);
                return document.NumberOfPages;
            }
            catch (Exception)
            {
                // 오류 처리 (예: 파일이 PDF가 아닌 경우)
                return 0;
            }
        }

        public void MergePdf(string outputPath, ObservableCollection<Models.PdfItem> pdfItems)
        {
            // PDF 병합 기능 구현
            PdfSharpDocument outputDocument = new(outputPath);

            foreach (var pdfItem in pdfItems)
            {
                var inputFilePath = pdfItem.FilePath;
                var selectedPages = pdfItem.PagesToProcess;

                // 리스트에 추가된 파일을 삭제하고 저장하면 여기서 튕기고 세계가 멸망하고 우주가 폭발함. 주의.
                var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);

                if (selectedPages.Count == 0)
                {
                    // 모든 페이지를 병합
                    for (var idx = 0; idx < pdfItem.PageCount; idx++)
                        outputDocument.AddPage(inputDocument.Pages[idx]);
                }
                else
                {
                    // 지정된 페이지만 병합
                    foreach (var pageIndex in selectedPages)
                        if (pageIndex >= 0 && pageIndex < pdfItem.PageCount)
                            outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                }
                inputDocument.Close();
            }
            outputDocument.Close();
        }

        public void SplitPdf(string outputFolder, ObservableCollection<Models.PdfItem> pdfItems)
        {
            // PDF 분할 기능 구현
            foreach (var pdfItem in pdfItems)
            {
                var inputFilePath = pdfItem.FilePath;
                var selectedPages = pdfItem.PagesToProcess;
                var splitInterval = pdfItem.SplitSize;
                var outputPrefix = $"{pdfItem.FileName}_";
                var partNumber = 1;

                var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);

                if (selectedPages.Count == 0)
                {
                    foreach (var group in Enumerable.Range(0, pdfItem.PageCount).Chunk(splitInterval))
                    {
                        var outputFilePath = Path.Combine(Directory.CreateDirectory(outputFolder).FullName, $"{outputPrefix}{partNumber}.pdf");
                        PdfSharpDocument outputDocument = new(outputFilePath);
                        foreach (var pageIndex in group)
                        {
                            if (pageIndex >= 0 && pageIndex < pdfItem.PageCount)
                                outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                        }
                        outputDocument.Close();
                        partNumber++;
                    }
                }
                else
                {
                    // 지정된 페이지만 사용
                    foreach (var group in selectedPages.Chunk(splitInterval))
                    {
                        var outputFilePath = Path.Combine(Directory.CreateDirectory(outputFolder).FullName, $"{outputPrefix}{partNumber}.pdf");
                        PdfSharpDocument outputDocument = new(outputFilePath);
                        foreach (var pageIndex in group)
                        {
                            if (pageIndex >= 0 && pageIndex < pdfItem.PageCount)
                                outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                        }
                        outputDocument.Close();
                        partNumber++;
                    }
                }
                inputDocument.Close();
            }
        }
    }
}

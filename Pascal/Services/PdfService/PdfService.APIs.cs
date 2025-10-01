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
                using UglyToadDocument document = UglyToadDocument.Open(filePath);
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
                string inputFilePath = pdfItem.FilePath;
                List<int> selectedPages = pdfItem.PagesToExtract;
                // 리스트에 추가된 파일을 삭제하고 저장하면 여기서 튕기고 세계가 멸망하고 우주가 폭발함. 주의.
                PdfSharpDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                if (selectedPages.Count == 0)
                {
                    // 모든 페이지를 병합
                    for (int idx = 0; idx < pdfItem.PageCount; idx++)
                        outputDocument.AddPage(inputDocument.Pages[idx]);
                }
                else
                {
                    // 지정된 페이지만 병합
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
            // PDF 분할 기능 구현
            foreach (var pdfItem in pdfItems)
            {
                string inputFilePath = pdfItem.FilePath;
                //string outputPrefix = pdfItem.SplitName;
                //int splitFrom = pdfItem.SplitStart;
                //int splitTo = pdfItem.SplitEnd;
                //int splitInterval = pdfItem.SplitSize;
                string outputPrefix = "output_part_"; // 예: output_part_1.pdf, output_part_2.pdf, ...
                int splitFrom = 0; // 예: 1페이지부터
                int splitTo = FindPageRanges(inputFilePath); // 예: 마지막 페이지까지
                int splitInterval = 10; // 예: 10페이지씩 분할

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

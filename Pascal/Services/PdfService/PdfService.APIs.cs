using Microsoft.UI.Xaml.Controls;
using Pascal.Models;
using PdfSharp.Pdf;
using UglyToad.PdfPig;
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
            //// PdfSharp 사용
            //try
            //{
            //    using PdfSharp.Pdf.PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
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
                using UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(filePath);
                return document.NumberOfPages;
            }
            catch (Exception)
            {
                // 오류 처리 (예: 파일이 PDF가 아닌 경우)
                return 0;
            }
        }

        public void MergePdf(Stream stream, ObservableCollection<PdfItemToMerge> pdfItems)
        {
            // PDF 병합 기능 구현
            PdfSharp.Pdf.PdfDocument outputDocument = new(stream);
            foreach (var pdfItem in pdfItems)
            {
                string inputFilePath = pdfItem.FilePath;
                List<int> selectedPages = pdfItem.PagesToExtract;
                PdfSharp.Pdf.PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
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

        public void SplitPdf(ObservableCollection<PdfItemToMerge> pdfItems)
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

                PdfSharp.Pdf.PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(inputFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                int partNumber = 1;
                for (int i = splitFrom; i < splitTo; i += splitInterval)
                {
                    PdfSharp.Pdf.PdfDocument outputDocument = new();
                    for (int j = i; j < i + splitInterval && j < splitTo; j++)
                    {
                        outputDocument.AddPage(inputDocument.Pages[j]);
                    }
                    string outputFilePath = Path.Combine(Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(inputFilePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileNameWithoutExtension(inputFilePath))).FullName, $"{outputPrefix}{partNumber}.pdf");
                    outputDocument.Save(outputFilePath);
                    outputDocument.Close();
                    partNumber++;
                }
                inputDocument.Close();
            }
        }
    }
}

using Pascal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.Services.PdfService
{
    public interface IPdfService
    {
        // 가빈씨 짱
        // PDF 관련 멤버 정의
        int FindPageRanges(string filePath);
        void MergePdf(string outputPath, ObservableCollection<PdfItem> pdfItems);
        void SplitPdf(string outputPath, ObservableCollection<PdfItem> pdfItems);
    }
}

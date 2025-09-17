using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.Services.PdfService
{
    public interface IPdfService
    {
        // 가빈씨 짱
        // PDF 관련 멤버 정의

        IDictionary<string, List<int>> PdfPlanDict { get; set; }

        void MergePdf();
        void SplitPdf();
    }
}

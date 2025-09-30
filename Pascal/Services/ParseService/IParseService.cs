using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pascal.Models;

namespace Pascal.Services.ParseService
{
    public interface IParseService
    {
        List<int> ParsePageRange(string input, int maxPage);
        void ParsePageRange(ObservableCollection<PdfItem> pdfItems);
    }
}

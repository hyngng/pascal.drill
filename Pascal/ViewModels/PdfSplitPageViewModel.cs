using Pascal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.ViewModels
{
    public partial class PdfSplitPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<PdfItemToMerge> pdfItems = new();

        public PdfSplitPageViewModel()
        {
            // Constructor logic if needed
        }
    }
}

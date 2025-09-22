using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pascal.Models
{
    public class PdfItemToMerge : INotifyPropertyChanged
    {
        private int fileOrder;
        private string filePath = string.Empty;
        private string fileName = string.Empty;
        private string fileSize = string.Empty;
        private int pageCount;
        private string pageRange = string.Empty;
        private List<int> pagesToExtract = [];

        public int FileOrder { get => fileOrder; set { fileOrder = value; OnPropertyChanged(); } }
        public string FilePath { get => filePath; set { filePath = value; OnPropertyChanged(); } }
        public string FileName { get => fileName; set { fileName = value; OnPropertyChanged(); } }
        public string FileSize { get => fileSize; set { fileSize = value; OnPropertyChanged(); } }
        public int PageCount { get => pageCount; set { if (pageCount != value) { pageCount = value; OnPropertyChanged(); } } }
        public string PageRangeHint => PageCount <= 1 ? "1" : $"1-{PageCount}";
        public string PageRange { get => pageRange; set { if (pageRange != value) { pageRange = value; OnPropertyChanged(); } } }
        public List<int> PagesToExtract { get => pagesToExtract; set { pagesToExtract = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

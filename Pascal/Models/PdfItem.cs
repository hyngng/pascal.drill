using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pascal.Models
{
    public class PdfItem : INotifyPropertyChanged
    {
        private int fileOrder;
        private int pageCount;
        private int processUnit;
        private string filePath = string.Empty;
        private string fileName = string.Empty;
        private string fileSize = string.Empty;
        private string pageRange = string.Empty;
        private List<int> pagesToProcess = [];

        public int FileOrder { get => fileOrder; set { fileOrder = value; OnPropertyChanged(); } }
        public int PageCount { get => pageCount; set { pageCount = value; OnPropertyChanged(); } }
        public int ProcessUnit { get => processUnit; set { processUnit = value; OnPropertyChanged(); } }
        public string FilePath { get => filePath; set { filePath = value; OnPropertyChanged(); } }
        public string FileName { get => fileName; set { fileName = value; OnPropertyChanged(); } }
        public string FileSize { get => fileSize; set { fileSize = value; OnPropertyChanged(); } }
        public string PageRangeHint => PageCount <= 1 ? "1" : $"1-{PageCount}";
        public string PageRange { get => pageRange; set { if (pageRange != value) { pageRange = value; OnPropertyChanged(); } } }
        public List<int> PagesToProcess { get => pagesToProcess; set { pagesToProcess = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

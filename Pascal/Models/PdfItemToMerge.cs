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
        private double rangeStart;
        private double rangeEnd;

        public int FileOrder { get => fileOrder; set { fileOrder = value; OnPropertyChanged(); } }
        public string FilePath { get => filePath; set { filePath = value; OnPropertyChanged(); } }
        public string FileName { get => fileName; set { fileName = value; OnPropertyChanged(); } }
        public string FileSize { get => fileSize; set { fileSize = value; OnPropertyChanged(); } }
        public int PageCount { get => pageCount; set { pageCount = value; OnPropertyChanged(); } }
        public double RangeStart { get => rangeStart; set { rangeStart = value; OnPropertyChanged(); } }
        public double RangeEnd { get => rangeEnd; set { rangeEnd = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

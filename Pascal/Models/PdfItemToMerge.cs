using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pascal.Models
{
    public partial class PdfItemToMerge : INotifyPropertyChanged
    {
        private bool isChecked;
        private string fileName = string.Empty;
        private string fileSizeText = string.Empty;
        private int pageCount;
        private double rangeStart;
        private double rangeEnd;

        public bool IsChecked { get => isChecked; set { isChecked = value; OnPropertyChanged(); } }
        public string FileName { get => fileName; set { fileName = value; OnPropertyChanged(); } }
        public string FileSizeText { get => fileSizeText; set { fileSizeText = value; OnPropertyChanged(); } }
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

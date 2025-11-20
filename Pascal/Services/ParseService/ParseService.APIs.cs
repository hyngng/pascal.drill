using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pascal.Models;

namespace Pascal.Services.ParseService
{
    public partial class ParseService : IParseService
    {
        private static readonly Regex RangeRegex = new(@"^(\d*)-(\d*)$", RegexOptions.Compiled);

        private static IEnumerable<int> ParseToken(string part, int min, int max)
        {
            if (string.IsNullOrWhiteSpace(part))
                yield break;

            part = part.Trim();

            var m = RangeRegex.Match(part);
            if (m.Success)
            {
                int start = string.IsNullOrEmpty(m.Groups[1].Value) ? min
                           : (int.TryParse(m.Groups[1].Value, out var s) ? s : -1);

                int end = string.IsNullOrEmpty(m.Groups[2].Value) ? max
                         : (int.TryParse(m.Groups[2].Value, out var e) ? e : -1);

                if (start < min || end < min || start == -1 || end == -1)
                    yield break;

                if (start > end)
                    yield break;

                for (int i = start; i <= end; i++)
                    if (i >= min && i <= max)
                        yield return i;

                yield break;
            }

            if (int.TryParse(part, out int p) && p >= min && p <= max)
                yield return p;
        }

        public List<int> ParsePageRange(string input, int maxPage)
        {
            int minPage = 1;
            if (maxPage < minPage || string.IsNullOrWhiteSpace(input))
                return new();

            var tokens = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var set = new HashSet<int>();
            foreach (var raw in tokens)
            {
                var trimmed = raw.Trim();
                foreach (var oneBased in ParseToken(trimmed, minPage, maxPage))
                {
                    int zeroBased = oneBased - 1;
                    if (zeroBased >= 0 && zeroBased < maxPage)
                        set.Add(zeroBased);
                }
            }

            return set.OrderBy(p => p).ToList();
        }

        public void ParsePageRange(ObservableCollection<PdfItem> pdfItems)
        {
            if (pdfItems == null) return;
            foreach (var pdfItem in pdfItems)
                pdfItem.PagesToProcess = ParsePageRange(pdfItem.PageRange, pdfItem.PageCount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pascal.Models;

namespace Pascal.Services.ParseService
{
    public partial class ParseService : IParseService
    {
        public List<int> ParsePageRange(string input, int maxPage)
        {
            int minPage = 1;
            List<int> result = [];
            string[] parts = input.Split(',');

            foreach (string rawPart in parts)
            {
                string part = rawPart.Trim();

                if (part.Contains('-'))
                {
                    int start, end;

                    if (part.StartsWith('-'))
                    {
                        // "-20" → "1-20"
                        if (int.TryParse(part[1..].Trim(), out end))
                        {
                            start = minPage;
                        }
                        else continue;
                    }
                    else if (part.EndsWith('-'))
                    {
                        // "3-" → "3-maxPage"
                        if (int.TryParse(part[..^1].Trim(), out start))
                        {
                            end = maxPage;
                        }
                        else continue;
                    }
                    else
                    {
                        // "5-8"
                        var rangeParts = part.Split('-');
                        if (rangeParts.Length == 2 &&
                            int.TryParse(rangeParts[0].Trim(), out start) &&
                            int.TryParse(rangeParts[1].Trim(), out end))
                        {
                            // OK
                        }
                        else continue;
                    }

                    if (start <= end)
                    {
                        for (int p = start; p <= end; p++)
                        {
                            if (p >= minPage && p <= maxPage)
                            {
                                result.Add(p - 1);
                            }
                        }
                    }
                }
                else
                {
                    // 단일 페이지 숫자
                    if (int.TryParse(part, out int page))
                    {
                        if (page >= minPage && page <= maxPage)
                        {
                            result.Add(page - 1);
                        }
                    }
                }
            }

            return result;
        }

        public void ParsePageRange(ObservableCollection<PdfItemToMerge> pdfItems)
        {
            foreach (var pdfItem in pdfItems)
            {
                pdfItem.PagesToExtract = ParsePageRange(pdfItem.PageRange, pdfItem.PageCount);
            }
        }
    }
}

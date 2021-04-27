using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;


namespace SharagaReplacementsBot
{
    class SharagaReplacement
    {
   
        //Парсит html и возращает раписание в виде строки по url
        public static async Task<string> GetReplacementsString(string url)
        {
            var document = new HtmlParser().ParseDocument(await GetHtml(url));
            IElement replacementsTable = document.QuerySelectorAll("table tbody").ToArray()[^1];
            
            StringBuilder result = new StringBuilder();
            
            foreach (var row in replacementsTable.Children.Skip(1))
            {
                for (int i = 0; i < row.Children.Length; i++)
                {
                    string cellContent = row.Children[i].InnerHtml;
                    if(row.Children.Length > 1) 
                        result.Append(FormatCell(i, cellContent) + "\n");
                    else
                        result.Append(cellContent + "\n");
                }
                
                result.Append("\n");
            }

            return result.ToString();
        }

        //Возращает html по url
        public static async Task<string> GetHtml(string url) 
        {
            using WebClient client = new WebClient();
            return await client.DownloadStringTaskAsync(url);
        }
        
        //Форматирует ячейки в зависимости от их положения в строке2
        public static string FormatCell(int cellNumber, String cellContent) => cellNumber switch
        {
            0 => $"{cellContent}",
            1 => $"Номер пары: {cellContent}",
            2 => $"Пара: {cellContent}",
            3 => $"Препод: {cellContent}",
            4 => $"Кабинет: {cellContent}",
            _ => cellContent
        };




    }
}
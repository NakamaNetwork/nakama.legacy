using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TreasureGuide.Entities;

namespace TreasureGuide.Sniffer.DataParser
{
    public abstract class TreasureParser<T> : IParser
    {
        protected static readonly Regex FunctionRegex = new Regex("function(.|\r|\n)+?}");
        protected static readonly Regex SingleCommentRegex = new Regex("//(.+)");
        protected static readonly Regex MultiCommentRegex = new Regex("/\\*(.|\\r|\\n)+?\\*/");

        protected readonly string Endpoint;
        protected readonly TreasureEntities Context;

        protected TreasureParser(TreasureEntities context, string endpoint)
        {
            Context = context;
            Endpoint = endpoint;
        }

        public async Task Execute()
        {
            var data = await GetData();
            await Save(data);
        }

        protected abstract Task Save(T items);

        private async Task<T> GetData()
        {
            var stringData = await PerformRequest();
            var cleaned = CleanData(stringData);
            var trimmed = TrimData(cleaned);
            var converted = ConvertData(trimmed);
            return converted;
        }

        private async Task<string> PerformRequest()
        {
            var request = WebRequest.Create(Endpoint);
            var response = await request.GetResponseAsync();

            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        protected virtual string CleanData(string input)
        {
            input = FunctionRegex.Replace(input, "\"function\"");
            input = MultiCommentRegex.Replace(input, "");
            input = SingleCommentRegex.Replace(input, "");
            return input;
        }

        protected virtual string TrimData(string input)
        {
            var start = input.IndexOf('=') + 1;
            var end = input.LastIndexOf(';');
            return input.Substring(start, end - start);
        }

        protected virtual T ConvertData(string trimmed)
        {
            var data = JsonConvert.DeserializeObject<T>(trimmed);
            return data;
        }
    }
}

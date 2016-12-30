using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TreasureGuide.Entities;

namespace TreasureGuide.Sniffer.Parsers
{
    public interface ITreasureParser
    {
        Task Execute();
    }

    public abstract class TreasureParser<T> : ITreasureParser
    {
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

        protected abstract Task Save(T data);

        private async Task<T> GetData()
        {
            var stringData = await PerformRequest();
            var trimmed = TrimData(stringData);
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

        private string TrimData(string input)
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

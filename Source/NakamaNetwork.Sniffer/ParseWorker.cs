using System.Threading.Tasks;

namespace NakamaNetwork.Sniffer
{
    public interface IParser
    {
        Task Execute();
    }
}

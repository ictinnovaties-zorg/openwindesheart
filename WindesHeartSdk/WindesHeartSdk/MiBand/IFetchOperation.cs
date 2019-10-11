using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WindesHeartSdk.MiBand
{
    public interface IFetchOperation
    {
        /// <summary>
        /// Fetch data from device and store it locally
        /// </summary>
        Subject<bool> InitiateFetching();
    }
}
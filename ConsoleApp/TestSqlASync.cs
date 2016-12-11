using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    /// <summary>
    ///     Klidne v jednom vlaknu volamy ASYNC SQL dotazy - nemam jednotlive awaity v cyklu (serializovaly by se volani), ale
    ///     simuluju si cekani paralelne pomoci Task.WaitAll
    ///     Volani se tedy zahajuji paralelne a pri cekani na odpoved se vlakno vrati do threadpoolu. Behem dlouheho cekani na
    ///     SQL odpoved se thready tedy neblokuji. Pri zpracovani async odpovedi se startuji thready z ThreadPoolu na obslouzeni
    ///     odpovedi SQL
    ///     Toto je vhodny pripad volani na serveru, kdy delam vice IO volani a nenavazuji na sebe a muzu je tedy paralelizovat
    ///     I pokud na serveru delam jen jedno IO volani, vyhodou ze napriklad dlouhym cekanim na odpoved z DB serveru neblokuju thread
    /// </summary>
    public class TestSqlAsync : TestSqlBase
    {
        protected override int RunImpl(int callCount, TimeSpan sqlDelay)
        {
            Task<int>[] results = new Task<int>[callCount];
            for (var i = 0; i < callCount; i++)
            {
                results[i] = SqlCaller.GetValueAsync(sqlDelay, i);
            }
            Task.WaitAll(results);
            var sum = results.Sum(i => i.Result);
            return sum;
        }
    }
}
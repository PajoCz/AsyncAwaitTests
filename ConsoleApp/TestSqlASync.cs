using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    /// <summary>
    ///     Klidne v jednom vlaknu volany ASYNC SQL dotazy - nemam jednotlive awaity v cyklu (serializovaly by se volani), ale
    ///     simuluju si cekani paralelne pomoci Task.WaitAll
    ///     Volani se tedy zahajuji paralelne z hlavniho vlakna a potom se hlavni vlakno na Task.WaitAll zablokuje a ceka na asynchronni odpovedi.
    ///     Jakmile dorazi async odpovedi, tak se zpracovavaji thready z ThreadPoolu (paralelne)
    ///     Toto je vhodny pripad volani na serveru, kdy delam vice IO volani a nenavazuji na sebe a muzu je tedy paralelizovat
    ///     I pokud na serveru delam jen jedno IO volani, vyhodou ze napriklad dlouhym cekanim na odpoved z DB serveru
    ///     neblokuju thread
    /// </summary>
    public class TestSqlAsync : TestSqlBase
    {
        protected override int RunImpl(int callCount, TimeSpan sqlDelay)
        {
            Task<int>[] results = new Task<int>[callCount];
            for (var i = 1; i <= callCount; i++)
            {
                results[i - 1] = SqlCaller.GetValueAsync(sqlDelay, i);
            }
            Task.WaitAll(results);
            var sum = results.Sum(i => i.Result);
            return sum;
        }
    }
}
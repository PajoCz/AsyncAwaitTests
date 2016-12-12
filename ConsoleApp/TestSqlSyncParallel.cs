using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    /// <summary>
    ///     Ve vice vlaknech volam SYNC SQL dotaz
    ///     Priklad, kdy volam vice paralelnich volani serveru - ten startuje vice vlaken a obsluhuje pozadavky paralelne
    ///     Ale komunikase s IO jako je DB je synchronni, tedy pri cekani vsech paralelnich vlaken na SQL odpoved se blokuji
    ///     thready - toto zatezuje serverove zdroje
    /// </summary>
    public class TestSqlSyncParallel : TestSqlBase
    {
        protected override int RunImpl(int callCount, TimeSpan sqlDelay)
        {
            List<int> results = new List<int>(callCount);
            Parallel.For(1, callCount + 1, i => { results.Add(SqlCaller.GetValue(sqlDelay, i)); });
            var sum = results.Sum(i => i);
            return sum;
        }
    }
}
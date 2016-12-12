using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    /// <summary>
    ///     V cyklu volani SYNC SQL dotazu
    ///     Priklad, kdy na sluzbe nepouzivam async metody pro komunikaci s IO jako je DB
    ///     Jedno vlakno seriove vola vice SQL dotazu synchronne a sice spotrebuje jenom jedno vlakno, ale dotazy se pousti
    ///     seriove za sebou a je dlouha odezva
    ///     Pokud je pripad, kdy dotazy nemaji navaznost (vystup jednoho sql jde do vstupu druheho sql dotazu), potom je to
    ///     zbytecne je radit seriove za sebou, kdyz by mohly bezet paralelne
    /// </summary>
    public class TestSqlSync : TestSqlBase
    {
        protected override int RunImpl(int callCount, TimeSpan sqlDelay)
        {
            List<int> results = new List<int>();
            for (var i = 1; i <= callCount; i++)
            {
                results.Add(SqlCaller.GetValue(sqlDelay, i));
            }
            var sum = results.Sum(i => i);
            return sum;
        }
    }
}
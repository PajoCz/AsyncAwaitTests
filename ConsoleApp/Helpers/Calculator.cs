namespace ConsoleApp.Helpers
{
    public class Calculator
    {
        public int SumNumbersFromOneTo(int p_MaxNumber)
        {
            //int result = 0;
            //for (int i = 1; i <= p_MaxNumber; i++)
            //{
            //    result += i;
            //}
            //return result;

            return p_MaxNumber * (p_MaxNumber+1) / 2;
        }
    }
}

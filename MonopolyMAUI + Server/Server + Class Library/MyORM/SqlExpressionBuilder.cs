using System.Text;

namespace MyORM
{
    internal static class SqlExpressionBuilder
    {
        public static string PackInParentheses(IEnumerable<string> elements, string separator = " ")
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append('(');
            foreach (var element in elements)
                strBuilder.Append($"{element}{separator}");
            strBuilder.Remove(strBuilder.Length - separator.Length, separator.Length);
            strBuilder.Append(')');
            return strBuilder.ToString();
        }
    }
}

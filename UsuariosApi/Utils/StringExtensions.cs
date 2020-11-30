using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UsuariosApi.Utils
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static IEnumerable<string> SplitSemicolonSeparated(this string input)
        {
            var list = new List<string>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                list = input.Split(";").Select(x => x.Trim()).ToList();
            }

            return list;
        }
    }
}

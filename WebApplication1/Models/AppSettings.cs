using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AppSettings
    {
        public static string Url { get; set; } = "https://nezabudka-bot.herokuapp.com:443/{0}";
        public static string Name { get; set; } = "NezabudkaBot";
        public static string Key { get; set; } = "620983009:AAHOT34HPDN2J50hmW5ZFtSnK1D4JQ8TLx0";
    }
}

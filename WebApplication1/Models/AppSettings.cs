﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AppSettings
    {
        public static string Url { get; set; } = "https://URL:443/{0}";
        public static string Name { get; set; } = "<BOT_NAME>";
        public static string Key { get; set; } = "<BOT_KEY>";
    }
}

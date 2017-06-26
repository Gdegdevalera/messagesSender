﻿using System.Linq;

namespace Client.Extensions
{
    public static class StringExtension
    {
        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }
    }
}
﻿namespace Promrub.Services.API.Extensions
{
    public static class StringExtensions
    {
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}

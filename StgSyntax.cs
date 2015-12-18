using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Stg
{
    [ComVisible(false)]
    internal static class StgSyntax
    {
        //public static readonly char[] InvalidNameCharacters;

        public static readonly List<char> InvalidStringCharacters;

        //public static readonly char[] InvalidAttributeName;

        public static bool IsCharactersInValue(string value, params char[] characters)
        {
            return (value.IndexOfAny(characters) != -1);
        }

        /*public static bool IsValidAttribute(string name)
        {
            return !IsCharactersInValue(name, InvalidAttributeName);
        }*/

        public static bool IsValidName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (!(((c >= 'A') && (c <= 'Z')) || ((c >= 'a') && (c <= 'z')) || ((c >= '0') && (c <= '9')) || (c == '_') || (c == '.') || (c == ':') || (c == '-')))
                {
                    return false;
                }
            }
            return true;
            //return !IsCharactersInValue(name, InvalidNameCharacters);
        }

        static StgSyntax()
        {
            #region Invalid Name Characters initaialization

            /*InvalidNameCharacters = new char[] { '=', ' ', 
                '\t', '<', '>', '/', '\"', '[', ']', '$',
                '#', '^', '!', '~', '`', '@', '%',
                '&', '*', '(', ')', ',', '\'', '.',
                '?', '|', '+', '=', '\\',
                ';', '№', '\r', '\n' };*/

            InvalidStringCharacters = new List<char> {
                '<', '>', '/', '\"', '&', '\r', '\n', ';'};

            #endregion

            /*#region Invalid Attribute Characters initialization

            InvalidAttributeName = new char[] { '>', '<'};

            #endregion*/
        }
    }
}

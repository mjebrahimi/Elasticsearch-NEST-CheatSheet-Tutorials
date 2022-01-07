using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public static class PersionEnglishCharacterConvertor
    {
        private static readonly Dictionary<char, char> _charactersMap = new Dictionary<char, char>
        {
            ['q'] = 'ض',
            ['w'] = 'ص',
            ['e'] = 'ث',
            ['r'] = 'ق',
            ['t'] = 'ف',
            ['y'] = 'غ',
            ['u'] = 'ع',
            ['i'] = 'ه',
            ['o'] = 'خ',
            ['p'] = 'ح',
            ['['] = 'ج',
            [']'] = 'چ',
            ['a'] = 'ش',
            ['s'] = 'س',
            ['d'] = 'ی',
            ['f'] = 'ب',
            ['g'] = 'ل',
            ['h'] = 'ا',
            ['j'] = 'ت',
            ['k'] = 'ن',
            ['l'] = 'م',
            [';'] = 'ک',
            ['\''] = 'گ',
            ['\\'] = 'پ',
            ['z'] = 'ظ',
            ['x'] = 'ط',
            ['c'] = 'ز',
            ['v'] = 'ر',
            ['b'] = 'ذ',
            ['n'] = 'د',
            ['m'] = 'ئ',
            [','] = 'و',
            ['T'] = '،',
            ['Y'] = '؛',
            ['U'] = ',',
            ['I'] = ']',
            ['O'] = '[',
            ['P'] = '\\',
            ['{'] = '}',
            ['}'] = '{',
            ['G'] = 'ۀ',
            ['H'] = 'آ',
            ['J'] = 'ـ',
            ['K'] = '«',
            ['L'] = '»',
            [':'] = ':',
            ['"'] = '"',
            ['|'] = '|',
            ['Z'] = 'ة',
            ['X'] = 'ي',
            ['C'] = 'ژ',
            ['V'] = 'ؤ',
            ['B'] = 'إ',
            ['N'] = 'أ',
            ['M'] = 'ء',
            ['?'] = '؟',
        };

        public static string IncorrectFarsiToEnglish(this string str)
        {
            foreach (var item in _charactersMap)
                str = str.Replace(item.Value, item.Key);
            return str;
        }

        public static string IncorrectEnglishToFarsi(this string str)
        {
            foreach (var item in _charactersMap)
                str = str.Replace(item.Key, item.Value);
            return str;
        }
    }
}

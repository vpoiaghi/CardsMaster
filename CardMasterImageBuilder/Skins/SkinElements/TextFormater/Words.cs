using System;
using System.Collections.Generic;

namespace CardMasterImageBuilder.Elements.TextFormater
{
    abstract class Words
    {
        private const char SPECIAL_CHAR_MARKER = '\\';
        private const char OPEN_BALISE = '<';
        private const char CLOSE_BALISE = '>';
        private const char WORDS_SEPARATOR = ' ';


        public static List<string> Cut(string text)
        {
            return CutByWords(text);
        }

        private static List<string> CutByWords(string text)
        {
            List<string> words = new List<string>();
            List<string> parts = CutByParts(text);

            string strOpenBalise = OPEN_BALISE.ToString();
            string strCloseBalise = CLOSE_BALISE.ToString();

            foreach (string part in parts)
            {
                if (part.StartsWith(strOpenBalise) && part.EndsWith(strCloseBalise))
                {
                    words.Add(part.Trim());
                }
                else
                {
                    foreach (string word in TrimPart(part).Split(WORDS_SEPARATOR))
                    {
                        words.Add(word.Replace("\\" + strOpenBalise, strOpenBalise).Replace("\\" + strCloseBalise, strCloseBalise));
                    }
                }
            }

            return words;
        }

        private static string TrimPart(string part)
        {
            string sep = WORDS_SEPARATOR.ToString();

            if (part.StartsWith(sep))
            {
                part = part.Substring(sep.Length);
            }

            if (part.EndsWith(sep))
            {
                part = part.Substring(0, part.Length - sep.Length);
            }

            return part;
        }

        private static List<string> CutByParts(string text)
        {
            List<string> parts = new List<string>();
            List<string> sections = CutBySections(text);

            int pos1 = 0;
            int pos2 = 0;

            string closeBalise = null;
            bool isBalise = false;

            string strOpenBalise = OPEN_BALISE.ToString();
            string strCloseBalise = CLOSE_BALISE.ToString();
            string strSpecialCharMarker = SPECIAL_CHAR_MARKER.ToString();

            foreach (string section in sections)
            {
                while (pos1 > -1)
                {
                    isBalise = false;

                    while ((pos1 > -1) && (!isBalise))
                    {
                        pos1 = section.IndexOf(OPEN_BALISE, pos1);
                        isBalise = (pos1 > -1) && ((pos1 == 0) || (!section.Substring(pos1 - 1, 1).Equals(strSpecialCharMarker)));
                    }

                    if (pos1 != -1)
                    {
                        if (pos1 > pos2)
                        {
                            parts.Add(section.Substring(pos2, pos1 - pos2));
                        }

                        pos2 = pos1;

                        closeBalise = strCloseBalise;
                        while (section.Substring(++pos1, 1).Equals(strOpenBalise))
                        {
                            closeBalise += strCloseBalise;
                        }

                        isBalise = false;

                        while ((pos1 > -1) && (!isBalise))
                        {
                            pos1 = section.IndexOf(closeBalise, pos1);
                            isBalise = (pos1 > -1) && ((pos1 == 0) || (!section.Substring(pos1 - 1, 1).Equals(strSpecialCharMarker)));
                        }

                        if (pos1 == -1)
                        {
                            throw new FormatException("Balise fermante manquante.");
                        }
                        else
                        {
                            parts.Add(section.Substring(pos2, pos1 + closeBalise.Length - pos2));
                            pos1 = pos1 + closeBalise.Length;
                            pos2 = pos1;
                        }
                    }
                }

                if (pos2 < section.Length)
                {
                    parts.Add(section.Substring(pos2, section.Length - pos2));
                }

                pos1 = 0;
                pos2 = 0;

                parts.Add("<br/>");
            }

            if (parts.Count > 0)
            {
                parts.RemoveAt(parts.Count - 1);
            }

            return parts;
        }

        private static List<string> CutBySections(string text)
        {
            List<string> sections = new List<string>();
            sections.AddRange(text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            return sections;
        }

    }
}

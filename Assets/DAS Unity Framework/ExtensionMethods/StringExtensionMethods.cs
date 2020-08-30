using System.Collections.Generic;
using System.IO;

namespace DAS_Unity_Framework.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        //=============== String and File Extensions ==========================================

        public static List<string> GetFilesRecursivelyInDirectory(this string directory)
        {
            List<string> files = new List<string>();
            foreach (string dir in Directory.EnumerateDirectories(directory))
            {
                files.AddRange(GetFilesRecursivelyInDirectory(dir));
            }

            files.AddRange(Directory.EnumerateFiles(directory));
            return files;
        }

        public static string CamelCaseToEnglishTitle(this string camel)
        {
            string title = "";

            for (int i = 0; i < camel.Length; i++)
            {
                if (i == 0)
                {
                    title += char.ToUpperInvariant(camel[i]);
                }
                else
                {
                    if (char.IsUpper(camel[i]))
                        title += " ";
                    title += camel[i];
                }

            }

            return title;
        }

        /// <summary>
        /// removes "(Clone)" from any string passed in
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Clean(this string s)
        {
            return s.Replace("(Clone)", "");

        }
    }
}
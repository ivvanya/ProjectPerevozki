using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Documents
{
    public class WordHelper
    {
        private FileInfo _fileInfo;

        public WordHelper(string filename)
        {
            if (File.Exists(filename))
            {
                _fileInfo = new FileInfo(filename);
            }
            else
            {
                MessageBox.Show("Шаблон не найден");
            }
        }

        private List<string> SplitString(string str)
        {
            List<string> list = new List<string>();
            int i = 240;
            while (i < str.Length - 1)
            {
                list.Add(str.Substring(i, 240) + "<");
                i += 240;
            }
            return list;

        }
        internal bool Process(Dictionary<string, string> items)
        {
            Word.Application app = null;
            try
            {
                app = new Word.Application();
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;

                app.Documents.Open(file);

                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    Object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing,
                        Replace: replace);
                }

                Object newFileName = Path.Combine(_fileInfo.DirectoryName,
                    DateTime.Now.ToString("dd.MM.yyyy-HH.mm-") + _fileInfo.Name); //debug
                //Object newFileName = Path.Combine(_fileInfo.DirectoryName, "../",
                //   DateTime.Now.ToString("dd.MM.yyyy-HH.mm-") + _fileInfo.Name); //release
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();

                return true;
            }
            catch
            {
                MessageBox.Show("Ошибка формирования документа");
                return false;
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();
                }
            }
        }
    }
}

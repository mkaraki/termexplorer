using System;
using System.Collections.Generic;
using System.Text;
using static termexplorer.Config.ColorMap;
using static System.Console;
using static termexplorer.ConsoleInfo;

namespace termexplorer.DocumentWriter
{
    class VWriter
    {
        public class Content
        {
            public Content(Content ToWrite)
            {
                Title = ToWrite.Title;
                Contents = ToWrite.Contents;
                Rows = ToWrite.Rows;
                CurrentRow = 0;
                InternalDocument = ToWrite.InternalDocument;
            }

            public Content(string ContentTitle,List<string> Source,bool isInternalDocument = false)
            {
                if(Config.WriteEOFinReader)
                    Source.Add("--- EOF");

                Title = ContentTitle;

                Contents = Source;
                Rows = Source.Count;
                CurrentRow = 0;

                InternalDocument = isInternalDocument;
            }

            public bool InternalDocument;

            public int Rows;
            public int CurrentRow;

            public string Title;
            public List<string> Contents;
        }

        public static Content ToWrite;

        #region Loader
        public static void LoadDocumentWriter(Content v)
        {
            ToWrite = new Content(v);

            DWriter();
        }

        public static void LoadDocumentWriter(string ContentTitle, List<string> Source, bool isInternalDocument = false)
        {
            ToWrite = new Content(ContentTitle, Source, isInternalDocument);

            DWriter();
        }

        public static void DWriter()
        {
            while (true)
            {
                UpdateConsoleInfo();
                SWrite();
                ConsoleKeyInfo cki = ReadKey(false);
                if (cki.KeyChar == 'q')
                    break;
                UserController.Handle(cki);
            }
        }
        #endregion

        public static void SWrite()
        {
            UIWriter.UIInfo ui = new UIWriter.UIInfo();

            //UIWriter.ClearLast();

            //Clear();
            int WritedLine = 0;

            string ContentSplit = new string('-', WritableWidth);

            ForegroundColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;

            if (Config.WriteProductName)
            {
                WritedLine++;
                string titlepad = new string(' ', (WritableWidth - ProductInfo.Name.Length) / 2);
                ui.WriteLine(titlepad + ProductInfo.Name + titlepad);
            }

            //TODO: Write Options
            WritedLine++;
            ui.WriteLine("q: Back");

            WritedLine++;
            ui.WriteLine(ContentSplit);

            WritedLine++;
            ui.WriteLine("   Title: "+ToWrite.Title);

            WritedLine++;
            ui.WriteLine(ContentSplit);

            int ContentHeight = WritableHeight - 1 - WritedLine;

            for (int i = 0; i < ToWrite.Rows; i++)
            {
                if (i < ToWrite.CurrentRow) continue;
                if (i > ToWrite.CurrentRow+ContentHeight) continue;

                ui.WriteLine(ToWrite.Contents[i]);
            }

            ui.WriteDown();
        }
    }
}

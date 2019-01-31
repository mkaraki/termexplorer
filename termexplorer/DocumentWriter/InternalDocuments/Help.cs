using System.Collections.Generic;

namespace termexplorer.DocumentWriter.InternalDocuments
{
    internal class Help
    {
        public static readonly VWriter.Content Content = new VWriter.Content("Help",
            new List<string>
        {
            "Welcome to termexplorer.",
            "In this document, we will tell you about it",
        },
            true);
    }
}
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
            "",
            "Explorer View:",
            "Q                : Exit this application",
            "F1               : Open this help",
            "Up/Down Arrow    : Select item from current window",
            "Left/Right Arrow : Select window",
            "Alt + D          : Change current window's directory",
            "F5               : Update current window's directory",
            "Del              : Delete selected item from current window",

        },
            true);
    }
}
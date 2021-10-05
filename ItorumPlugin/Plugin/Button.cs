using System;

namespace Itorum
{
    public class Button
    {
        public int Id { get; set; }
        public readonly string name;
        public readonly string description;
        public readonly Ico ico;
        public readonly string tab;
        public readonly string group;
        public readonly Action macro;

        public Button(string name, string description, Ico ico, string tab, string group, Action macro)
        {
            this.name = name;
            this.description = description;
            this.ico = ico;
            this.tab = tab;
            this.group = group;
            this.macro = macro;
        }

        public bool IsApplicationTab
        {
            get { return tab.CompareTo("Приложения") == 0; }
        }
    }
}
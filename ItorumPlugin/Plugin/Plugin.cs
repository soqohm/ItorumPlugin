using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using TFlex;
using TFlex.Model;

namespace Itorum
{
    public class MyPlugin : Plugin
    {
        private readonly List<Button> buttons;

        private List<string> DeletedTabs
        {
            get { return buttons
                    .Where(e => !e.IsApplicationTab)
                    .Select(e => e.tab)
                    .Distinct()
                    .ToList(); 
            }
        }

        private List<string> DeletedAppTabGroups
        {
            get { return buttons
                    .Where(e => e.IsApplicationTab)
                    .Select(e => e.group)
                    .Distinct()
                    .ToList();
            }
        }

        public MyPlugin(PluginFactory factory, List<Button> buttons) : base(factory) 
        { 
            this.buttons = buttons; 
        }

        protected override void OnCreateTools()
        {
            DeleteButtons();

            CreateButtons();
        }

        protected override void OnCommand(Document document, int id)
        {
            try 
            { 
                buttons[id - 1].macro(); 
            }
            catch 
            { 
                base.OnCommand(document, id); 
            }
        }

        protected override void OnExiting(CancelEventArgs args) 
        { 
            DeleteButtons(); 
        }

        private void CreateButtons()
        {
            var id = 1;
            foreach (var button in buttons)
            {
                button.Id = id++;

                var ico = LoadIcon(button.ico.ToString());
                RegisterCommand(button.Id, button.name, ico, ico);

                var group = GetGroup(button)
                    .AddButton(button.Id, this)
                    .Description = button.description;
            }
        }

        private void DeleteButtons()
        {
            foreach (var name in DeletedTabs)
            {
                var tab = FindTab(name);
                if (tab != null)
                    tab.Remove();
            }

            foreach (var name in DeletedAppTabGroups)
            {
                var group = FindGroup(name, RibbonBar.ApplicationsTab);
                if (group != null)
                    group.Remove();
            }
        }

        private RibbonGroup GetGroup(Button button)
        {
            var tab = GetTab(button);
            var group = FindGroup(button.group, tab);
            if (group == null)
            {
                return tab.AddGroup(button.group);
            }
            else 
                return group;
        }

        private RibbonTab GetTab(Button button)
        {
            if (!button.IsApplicationTab)
            {
                var tab = FindTab(button.tab);
                if (tab == null)
                {
                    tab = Application.ActiveMainWindow.RibbonBar
                        .AddTab(button.tab);
                }
                return tab;
            }
            else return RibbonBar.ApplicationsTab;
        }

        private RibbonGroup FindGroup(string name, RibbonTab tab)
        {
            return tab.Groups.Where(e => e.Caption == name).FirstOrDefault();
        }

        private RibbonTab FindTab(string name)
        {
            return Application.ActiveMainWindow.RibbonBar.Tabs
                .Where(e => e.Caption == name).FirstOrDefault();
        }

        private Icon LoadIcon(string name) =>
            new Icon(GetType().Assembly.GetManifestResourceStream("Itorum.Icons." + name + ".ico"));
    }
}
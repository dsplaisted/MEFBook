using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UISampleContracts;

namespace MEFUIAdditionalExtensions
{
    public class MenuExtension
    {
        private Menu _menu;

        [ExportControl("Menu", Position = DockPosition.Top)]
        public Menu Menu { get { return _menu; } }

        public MenuExtension()
        {
            _menu = new Menu();
            var fileMenu = new MenuItem();
            fileMenu.Header = "File";
            _menu.Items.Add(fileMenu);

            var exitCommand = new MenuItem();
            exitCommand.Header = "Exit";
            exitCommand.Click += delegate { Application.Current.Shutdown(); };

            fileMenu.Items.Add(exitCommand);
        }
    }
}

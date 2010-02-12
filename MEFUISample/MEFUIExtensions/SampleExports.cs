using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using UISampleContracts;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.ComponentModel.Composition;

namespace MEFUIExtensions
{
    [ExportControl("A Button", Position = DockPosition.Center)]
    public class SimpleButton : Button
    {
        public SimpleButton()
        {
            this.Content = "Click Me!";
            this.Click += new System.Windows.RoutedEventHandler(SimpleButton_Click);
        }

        void SimpleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Content = "Thanks!";
        }
    }

    
    public class PropertyExporter
    {
        Button _button;
        int _timesClicked = 0;

        [Import]
        IStatusBarUpdate StatusBar { get; set; }

        public PropertyExporter()
        {
            _button = new Button();
            _button.Content = "Update status bar";
            _button.Click += new System.Windows.RoutedEventHandler(SimpleButton_Click);
        }

        void SimpleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _timesClicked++;
            string status;
            if (_timesClicked == 1)
            {
                status = "Clicked once";
            }
            else
            {
                status = "Clicked " + _timesClicked + " times!";
            }
            StatusBar.SetStatus(status);
        }

        [ExportControl("Another Button", Position = DockPosition.Center)]
        public Button ExportedButton
        {
            get { return _button; }
        }
    }

    [ExportControl("Exit button", Position = DockPosition.Left)]
    public class LeftButton : Button
    {
        public LeftButton()
        {
            this.Content = "Exit";
            this.Click += new System.Windows.RoutedEventHandler(SimpleButton_Click);
        }

        void SimpleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    interface IStatusBarUpdate
    {
        void SetStatus(string status);
    }

    [ExportControl("Status bar", Position = DockPosition.Bottom)]
    [Export(typeof(IStatusBarUpdate))]
    public class BottomStatusBar : StatusBar, IStatusBarUpdate
    {
        TextBlock _textBlock;
        public BottomStatusBar()
        {
            _textBlock = new TextBlock();
            _textBlock.Text = "Status Bar";
            this.Items.Add(_textBlock);
        }

        public void SetStatus(string status)
        {
            _textBlock.Text = status;
        }
    }
}

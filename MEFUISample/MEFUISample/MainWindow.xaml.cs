using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using UISampleContracts;

namespace MEFUISample
{
    public partial class MainWindow : Window
    {
        [ImportMany(UISampleConstants.ControlContractName)]
        public Lazy<UIElement, IControlMetadata> [] Extensions;

        public MainWindow()
        {
            InitializeComponent();

			CompositionInitializer.SatisfyImports(this);

            SetupArea(DockPosition.Top, _topArea);
            SetupArea(DockPosition.Bottom, _bottomArea);
            SetupArea(DockPosition.Left, _leftArea);
            SetupArea(DockPosition.Right, _rightArea);
            SetupArea(DockPosition.Center, _mainArea);
        }

        private void SetupArea(DockPosition position, ContentControl contentArea)
        {
            var extensionForThisArea = Extensions.Where(e => e.Metadata.Position == position).ToList();
            if (extensionForThisArea.Count == 0)
            {
                contentArea.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (extensionForThisArea.Count == 1)
            {
                contentArea.Content = extensionForThisArea[0].Value;
            }
            else
            {
                TabControl tabControl = new TabControl();
                foreach (var extension in extensionForThisArea)
                {
                    var tabItem = new TabItem();
                    tabItem.Header = extension.Metadata.Name;
                    tabItem.Content = extension.Value;

                    tabControl.Items.Add(tabItem);
                }
                contentArea.Content = tabControl;
            }
        }

		//private void ComposeHost()
		//{
		//    var thisCatalog = new AssemblyCatalog(this.GetType().Assembly);
		//    // For extensions create a DirectoryCatalog and add it to the AggregateCatalog
		//    var dirCatalog = new DirectoryCatalog("Extensions");
		//    var catalog = new AggregateCatalog(thisCatalog, dirCatalog);
		//    var container = new CompositionContainer(catalog);

		//    container.ComposeParts(this);
		//}
    }
}

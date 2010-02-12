using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace UISampleContracts
{
    public static class UISampleConstants
    {
        public const string ControlContractName = "UISampleControl";
    }

    public enum DockPosition
    {
        Center,
        Top,
        Bottom,
        Left,
        Right        
    }

    public interface IControlMetadata
    {
        string Name { get; }
        [DefaultValue(DockPosition.Center)]
        DockPosition Position { get; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple=false)]
    [MetadataAttribute]
    public class ExportControlAttribute : ExportAttribute, IControlMetadata
    {
        public ExportControlAttribute(string name)
            : base(UISampleConstants.ControlContractName, typeof(UIElement))
        {
            Name = name;
            Position = DockPosition.Center;
        }

        public string Name { get; private set; }

        public DockPosition Position { get; set; }
    }
}

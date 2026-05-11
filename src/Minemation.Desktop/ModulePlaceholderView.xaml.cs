using System;
using System.Collections.Generic;
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
using MaterialDesignThemes.Wpf;

namespace Minemation.Desktop;

public partial class ModulePlaceholderView : UserControl
{
    public ModulePlaceholderView(string title, string description, PackIconKind icon = PackIconKind.ApplicationCog)
    {
        InitializeComponent();

        TitleText.Text = title;
        DescriptionText.Text = description;
        ModuleIcon.Kind = icon;
    }
}

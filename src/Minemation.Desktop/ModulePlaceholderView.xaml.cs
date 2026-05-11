using System.Windows.Controls;
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

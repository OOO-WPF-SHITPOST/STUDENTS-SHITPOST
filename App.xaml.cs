using Nedelyaeva.Data;
using Nedelyaeva.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Nedelyaeva;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static User User;
    public static studContext Context = studContext.Instance;
}


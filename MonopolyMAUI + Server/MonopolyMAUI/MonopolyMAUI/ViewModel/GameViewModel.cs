
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonopolyMAUI.Models;
using MonopolyMAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MonopolyMAUI.ViewModel;

[QueryProperty("Players", "Players")]
public partial class GameViewModel : StartGameViewModel
{

}

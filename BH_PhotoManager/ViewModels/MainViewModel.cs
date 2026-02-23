using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace BH_PhotoManager.ViewModels
{
    public partial class MainViewModel : ObservableObject
    //,IRecipient<LoadingMessage>
    //,IRecipient<SettingRefreshMessage>
    //,IRecipient<UpdateFooterDetectedMessage>
    //,IRecipient<ShowImageNameMessage>
    {
        [ObservableProperty] private string? _title;
        public MainViewModel(INavigationService navigationService, IMessenger messenger)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using EventFrameMobileManager.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using OSIsoft.PIDevClub.PIWebApiClient;

namespace EventFrameMobileManager.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                if (_loginCommand is null)
                {
                    _loginCommand = new RelayCommand(() => Login());
                }

                return _loginCommand;
            }
        }

        private void Login()
        {
            if (!string.IsNullOrEmpty(_username) &&
                !string.IsNullOrEmpty(_password))
            {                
                var url = Xamarin.Forms.Application.Current.Resources["PIWebAPIUrl"].ToString();
                var c = new PIWebApiClient(url, false, _username, _password);
                
                if (SimpleIoc.Default.ContainsCreated<PIWebApiClient>())
                {
                    SimpleIoc.Default.Unregister<PIWebApiClient>();
                }
                SimpleIoc.Default.Register(() => c);

                var v = new SearchView();
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(v, true);
            }
        }
    }
}

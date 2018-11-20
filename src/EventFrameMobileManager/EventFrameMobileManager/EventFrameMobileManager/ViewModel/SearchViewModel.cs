using GalaSoft.MvvmLight;
using OSIsoft.PIDevClub.PIWebApiClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using OSIsoft.PIDevClub.PIWebApiClient.Model;
using GalaSoft.MvvmLight.Command;

namespace EventFrameMobileManager.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly PIWebApiClient _client;
        private string _afServerId;
        private string _afDbId;

        public SearchViewModel(PIWebApiClient client)
        {
            _client = client;

            InitClient();
        }

        private async Task InitClient()
        {
            var server = Xamarin.Forms.Application.Current.Resources["AFServer"].ToString();
            var db = Xamarin.Forms.Application.Current.Resources["AFDatabase"].ToString();
            var s = await _client.AssetServer.GetByNameAsync(server);
            var dbRes = await _client.AssetServer.GetDatabasesAsync(s.WebId);
            var d = dbRes.Items.Where(z => z.Name == db).FirstOrDefault();

            _afServerId = s.WebId;
            _afDbId = d?.WebId;

            await RefreshEventFrames();
        }

        private ObservableCollection<EventFrameViewModel> _eventFrames;
        public ObservableCollection<EventFrameViewModel> EventFrames
        {
            get { return _eventFrames; }
            set
            {
                _eventFrames = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _refreshEventFramesCommand;
        public RelayCommand RefreshEventFramesCommand
        {
            get
            {
                if (_refreshEventFramesCommand is null)
                {
                    _refreshEventFramesCommand = new RelayCommand(async () => await RefreshEventFrames());
                }

                return _refreshEventFramesCommand;
            }
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged();
            }
        }

        public async Task RefreshEventFrames()
        {
            IsRefreshing = true;
            var res = await _client.AssetDatabase.GetEventFramesAsync(_afDbId, 
                searchFullHierarchy: true,
                canBeAcknowledged: true,
                isAcknowledged: false,
                startTime: "*-180d");

            var evs = res.Items.Select(z => new EventFrameViewModel(_client)
            {
                Name = z.Name,
                StartTime = z.StartTime,
                AfServer = _afServerId,
                AfDatabase = _afDbId,
                WebId = z.WebId
            }
            );

            EventFrames = new ObservableCollection<EventFrameViewModel>(evs);
            IsRefreshing = false;
        }
    }
}

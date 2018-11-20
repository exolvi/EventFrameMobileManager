using GalaSoft.MvvmLight;
using OSIsoft.PIDevClub.PIWebApiClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using OSIsoft.PIDevClub.PIWebApiClient.Model;

namespace EventFrameMobileManager.ViewModel
{
    public class RelatedElementDetailViewModel : ViewModelBase
    {
        private readonly PIWebApiClient _client;

        public RelatedElementDetailViewModel(PIWebApiClient client)
        {
            _client = client;
        }

        public string AfServer { get; set; }
        public string AfDatabase { get; set; }
        public string RelWebId { get; set; }
        public string StartTime { get; set; }

        private ObservableCollection<PIAttribute> _attributes;
        public ObservableCollection<PIAttribute> Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                _attributes = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<PIExtendedTimedValue> _data;
        public ObservableCollection<PIExtendedTimedValue> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                RaisePropertyChanged();
            }
        }

        private PIAttribute _currrentAttribute;
        public PIAttribute CurrentAttribute
        {
            get { return _currrentAttribute  ; }
            set
            {
                _currrentAttribute = value;
                RaisePropertyChanged();
                RefreshData();
            }
        }

        public async Task RefreshAttributes()
        {
            try
            {
                var res = await _client.Element.GetAttributesAsync(RelWebId);

                Attributes = new ObservableCollection<PIAttribute>(res.Items);
            }
            catch (Exception ex)
            {

            }

        }

        private async Task RefreshData()
        {
            var start = DateTime.Parse(StartTime);
            start = start.AddHours(-1.0);
            var end = start.AddHours(2.0);
            var res = await _client.Stream.GetRecordedAsync(_currrentAttribute.WebId,
                startTime: start.ToString("G"),
                endTime: end.ToString("G"));

            Data = new ObservableCollection<PIExtendedTimedValue>(res.Items);
        }
    }
}

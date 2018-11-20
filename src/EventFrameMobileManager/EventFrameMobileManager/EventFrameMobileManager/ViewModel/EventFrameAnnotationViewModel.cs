using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OSIsoft.PIDevClub.PIWebApiClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventFrameMobileManager.ViewModel
{
    public class EventFrameAnnotationViewModel : ViewModelBase
    {
        private readonly PIWebApiClient _client;

        public EventFrameAnnotationViewModel(PIWebApiClient client)
        {
            _client = client;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public EventFrameViewModel EventFrame { get; set; }

        private RelayCommand _submitCommand;
        public RelayCommand SubmitCommand
        {
            get
            {
                if (_submitCommand is null)
                {
                    _submitCommand = new RelayCommand(async () => await Submit());
                }

                return _submitCommand;
            }
        }

        private async Task Submit()
        {
            await _client.EventFrame.CreateAnnotationAsync(EventFrame.WebId, new OSIsoft.PIDevClub.PIWebApiClient.Model.PIAnnotation
            {
                Name = Name,
                Description = Description,
                Value = Value
            });

            await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}

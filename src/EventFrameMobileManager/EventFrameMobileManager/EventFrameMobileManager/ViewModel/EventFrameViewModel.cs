using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using OSIsoft.PIDevClub.PIWebApiClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventFrameMobileManager.ViewModel
{
    public class EventFrameViewModel
    {
        private readonly PIWebApiClient _client;

        public EventFrameViewModel(PIWebApiClient client)
        {
            _client = client;
        }

        public string Name { get; set; }
        public string StartTime { get; set; }
        public string AfServer { get; set; }
        public string AfDatabase { get; set; }
        public string WebId { get; set; }
        public string RelWebId { get; set; }

        private RelayCommand _ackCommand;
        public RelayCommand AckCommand
        {
            get
            {
                if (_ackCommand is null)
                {
                    _ackCommand = new RelayCommand(async () => await Ack());
                }

                return _ackCommand;
            }
        }

        private RelayCommand _annotateCommand;
        public RelayCommand AnnotateCommand
        {
            get
            {
                if (_annotateCommand is null)
                {
                    _annotateCommand = new RelayCommand(() => Annotate());
                }

                return _annotateCommand;
            }
        }

        private async Task Ack()
        {
            await _client.EventFrame.AcknowledgeAsync(WebId);

            await SimpleIoc.Default.GetInstance<SearchViewModel>().RefreshEventFrames();
        }

        private void Annotate()
        {
            var vm = new EventFrameAnnotationViewModel(_client)
            {
                EventFrame = this
            };

            var v = new View.EventFrameAnnotationView
            {
                BindingContext = vm
            };

            Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(v, true);
        }
    }
}

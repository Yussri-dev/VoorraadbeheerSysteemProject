using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services.SaasClients;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmSaasClient : VmBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly SaasClientRequests _saasClientRequests;
        public VmSaasClient(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _saasClientRequests = new SaasClientRequests(AppConfig.ApiUrl);
            RegisterSaasCommmands = new ButtonCommand(async async => await RegisterSaasClientAsync());
        }

        #region Properties
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }
        private string _adresse;
        public string Adresse
        {
            get => _adresse;
            set
            {
                _adresse = value;
                OnPropertyChanged();
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _subscriptionType;
        public string SubscriptionType
        {
            get => _subscriptionType;
            set
            {
                _subscriptionType = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private DateTime _subscriptionExpiration;
        public DateTime SubscriptionExpiration
        {
            get => _subscriptionExpiration;
            set
            {
                _subscriptionExpiration = value;
            }
        }

        //public string Name { get; set; }
        //public string? Address { get; set; }
        //public string? Email { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public string? SubscriptionType { get; set; }
        //public DateTime? SubscriptionExpiration { get; set; }
        #endregion

        #region Commands
        public ICommand RegisterSaasCommmands { get; set; }
        #endregion

        #region Methods
        private async Task RegisterSaasClientAsync()
        {
            StatusMessage = string.Empty;

            if (
                string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Adresse) ||
                string.IsNullOrWhiteSpace(SubscriptionType)
                )
            {
                StatusMessage = "All Fields Are required";
            }
            var responseDto = new SaasClientDTO
            {
                Name = FullName,
                Address = Adresse,
                Email = Email,
                SubscriptionType = SubscriptionType,
                SubscriptionExpiration = SubscriptionExpiration
            };

            await _saasClientRequests.RegisterSaasClietnAsync(responseDto);
        }
        #endregion
    }
}

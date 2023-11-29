using Municipios.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Municipios.viewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<MuncipioAux> listaMunicipios = new ObservableCollection<MuncipioAux>();

        public ViewModel()
        {
            ListaMunicipios = new ObservableCollection<MuncipioAux>();
            CargarMunicipios();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<MuncipioAux> ListaMunicipios
        {
            get { return listaMunicipios; }
            set
            {
                listaMunicipios = value;
                OnPropertyChanged("ListaMunicipios");
            }
        }

        private async System.Threading.Tasks.Task<List<MuncipioAux>> Municipios()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;

            var uri = new Uri("https://docs.google.com/spreadsheets/d/12vgaYhOXsS4X_o-MfJN6xopnBm40mAq30hICCzz2zYY/gviz/tq?tqx=out:json&gid=0");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(uri);

            var listaMunicipios = new List<MuncipioAux>();
            if (response.IsSuccessStatusCode)
            {
                // TODO: parse JSON

            }
            return listaMunicipios;
        }

        private async void CargarMunicipios()
        {
            var municipios = await Municipios();
            foreach (var municipio in municipios)
            {
                ListaMunicipios.Add(municipio);
            }
        }
    }
}

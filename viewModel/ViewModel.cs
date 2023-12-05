using Municipios.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Municipios.viewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            ListaMunicipios = new ObservableCollection<MuncipioAux>();
            CargarMunicipios();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<MuncipioAux> listaMunicipios = new ObservableCollection<MuncipioAux>();
        public ObservableCollection<MuncipioAux> ListaMunicipios
        {
            get { return listaMunicipios; }
            set
            {
                listaMunicipios = value;
                OnPropertyChanged("ListaMunicipios");
            }
        }

        private async void CargarMunicipios()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;

            var uri = new Uri("https://docs.google.com/spreadsheets/d/12vgaYhOXsS4X_o-MfJN6xopnBm40mAq30hICCzz2zYY/gviz/tq?tqx=out:json&gid=0");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();

                // document is not a valid json, so we need to remove the first and last part of the string
                var start = data.Result.ToString().IndexOf('{');
                var json = data.Result.ToString().Remove(0, start);
                json = json.Substring(0, json.Length - 2);

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<RootMunicipios>(json);

                foreach (var row in result!.table.rows)
                {
                    var municipio = new MuncipioAux();
                    municipio.Nombre = row.c[1].v.ToString();
                    ListaMunicipios.Add(municipio);
                }
            }
        }

        private double mtemp;
        public double Mtemp
        {
            get { return mtemp; }
            set
            {
                mtemp = value;
                OnPropertyChanged("Mtemp");
            }
        }

        private string wicon;
        public string Wicon
        {
            get { return wicon; }
            set
            {
                wicon = value;
                OnPropertyChanged("Wicon");
            }
        }

        private double mtemp_min;
        public double Mtemp_min
        {
            get { return mtemp_min; }
            set
            {
                mtemp_min = value;
                OnPropertyChanged("Mtemp_min");
            }
        }

        private double mtemp_max;
        public double Mtemp_max
        {
            get { return mtemp_max; }
            set
            {
                mtemp_max = value;
                OnPropertyChanged("Mtemp_max");
            }
        }

        private double clon;
        public double Clon
        {
            get { return clon; }
            set
            {
                clon = value;
                OnPropertyChanged("Clon");
            }
        }

        private double clat;
        public double Clat
        {
            get { return clat; }
            set
            {
                clat = value;
                OnPropertyChanged("Clat");
            }
        }

        private double wspeed;
        public double Wspeed
        {
            get { return wspeed; }
            set
            {
                wspeed = value;
                OnPropertyChanged("Wspeed");
            }
        }

        private string selectedMunicipio;
        public string SelectedMunicipio
        {
            get { return selectedMunicipio; }
            set
            {
                selectedMunicipio = value;
                OnPropertyChanged("SelectedMunicipio");
                Debug.WriteLine("SelectedMunicipio: " + SelectedMunicipio);
                LeerJsonTiempo();
            }
        }

        private async void LeerJsonTiempo()
        {
            var client = new HttpClient();
            var nombre = "";
            string URLData = "http://api.openweathermap.org/data/2.5/find?&q=" + "Toledo" + ",eslang=es&units=metric&APPID=278857e8dee51f914026df21d0d40c19";


            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(new Uri(URLData));

            if (response.IsSuccessStatusCode)
            {
                var data1 = response.Content.ReadAsStringAsync();
                var weatherdata = JsonConvert.DeserializeObject<RootTiempo>(data1.Result.ToString());


                Mtemp = ((RootTiempo)weatherdata!).list[0].main.temp;

                Mtemp_min = ((RootTiempo)weatherdata).list[0].main.temp_min;

                Mtemp_max = ((RootTiempo)weatherdata).list[0].main.temp_max;

                Clon = ((RootTiempo)weatherdata).list[0].coord.lon;

                Clat = ((RootTiempo)weatherdata).list[0].coord.lat;

                Wspeed = ((RootTiempo)weatherdata).list[0].wind.speed;

                Wicon = "http://openweathermap.org/img/wn/" + ((RootTiempo)weatherdata).list[0].weather[0].icon + ".png";
            }
        }
    }
}

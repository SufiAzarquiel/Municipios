using Municipios.model;
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

        private async void CargarMunicipios()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;

            var uri = new Uri("https://docs.google.com/spreadsheets/d/12vgaYhOXsS4X_o-MfJN6xopnBm40mAq30hICCzz2zYY/gviz/tq?tqx=out:json&gid=0");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                // TODO: parse JSON
                /*
                {
                  "table": {
                    "cols": [],
                    "rows": 
                    [
                      {
                        "c": 
                        [
                          {
                            "v": 1,
                            "f": "1"
                          },
                          {
                            "v": "Abengibre"
                          }
                        ]
                      },
                      {
                      },
                      ...
                    ]
                }
                 */
                var data = response.Content.ReadAsStringAsync();

                // get index of first '{' character
                var start = data.Result.ToString().IndexOf('{');

                var json = data.Result.ToString().Remove(0, start);
                json = json.Substring(0, json.Length - 2);

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(json);
                Debug.WriteLine("Getting result...");

                foreach (var row in result!.table.rows)
                {
                    var municipio = new MuncipioAux();
                    municipio.Nombre = row.c[1].v.ToString();
                    ListaMunicipios.Add(municipio);
                    Debug.WriteLine("Name: " + municipio.Nombre);
                }

            }
        }
    }
}

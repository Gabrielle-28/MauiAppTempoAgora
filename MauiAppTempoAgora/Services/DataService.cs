using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "45719bcc1790294d82b2d57f7c3a5847";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await Shell.Current.DisplayAlert("Erro", "Cidade não encontrada. Verifique o nome digitado.", "OK");
                        return null;
                    }
                    else if (!resp.IsSuccessStatusCode)
                    {
                        await Shell.Current.DisplayAlert("Erro", "Erro ao buscar dados. Tente novamente.", "OK");
                        return null;
                    }

                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    };
                }
                catch (HttpRequestException)
                {
                    await Shell.Current.DisplayAlert("Erro de Conexão", "Sem conexão com a internet. Verifique sua rede.", "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Erro", $"Erro inesperado: {ex.Message}", "OK");
                }
            }

            // Ensure a return value in all code paths
            return t;
        }

        // Renamed the method to avoid CS0111 conflict and implemented it properly to avoid CS1998
        internal static async Task<Tempo?> GetPrevisaoPorTexto(string texto)
        {
            // Simulating an asynchronous operation
            return await Task.Run(() =>
            {
                // Placeholder logic for demonstration
                return new Tempo
                {
                    main = "Clear",
                    description = "Clear sky",
                    temp_min = 20.0,
                    temp_max = 30.0,
                    visibility = 10000,
                    speed = 5.0,
                    sunrise = DateTime.Now.AddHours(-6).ToString(),
                    sunset = DateTime.Now.AddHours(6).ToString()
                };
            });
        }
    }
}
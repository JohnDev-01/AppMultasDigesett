using AppMultasDigesett.Modelo;
using AppMultasDigesett.Servicio;
using Newtonsoft.Json.Linq;
using Plugin.Maui.Audio;
using System.Text;

namespace AppMultasDigesett.Vistas;

public partial class CrearMulta : ContentPage
{
    private SQLiteHelper _db;
    private IAudioRecorder recorder;
    private IAudioManager audioManager;
    private string rutaAudio;
    private string rutafoto;
    public CrearMulta(IAudioManager audioManager)
    {
        InitializeComponent();
        _db = new SQLiteHelper();
        // Cargar infracciones predefinidas
        TipoInfraccionPicker.ItemsSource = new List<string>
        {
            "Exceso de velocidad",
            "Conducir en estado de ebriedad",
            "Uso indebido del celular",
            "Pasarse un semáforo en rojo",
            "Estacionarse en lugar prohibido",
            "No usar cinturón de seguridad",
            "Conducir sin licencia",
            "Vehículo sin luces encendidas en la noche"
        };
        recorder =audioManager.CreateRecorder();
        this.audioManager = audioManager;
    }
    private async Task BuscarDatosVehiculo(bool Ischange)
    {
        string codigo = CodigoEntry.Text;
        if (!string.IsNullOrWhiteSpace(codigo))
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.adamix.net/itla.php?m={codigo}";
                var response = await client.GetStringAsync(url);
                var data = JObject.Parse(response);

                if (data["ok"]?.ToString() == "1")
                {
                    MarcaEntry.Text = data["marca"]?.ToString();
                    ModeloEntry.Text = data["modelo"]?.ToString();
                    ColorEntry.Text = data["color"]?.ToString();
                    AnioEntry.Text = data["anio"]?.ToString();
                    PlacaEntry.Text = data["placa"]?.ToString();
                }
                else
                {
                    MarcaEntry.Text =null;
                    ModeloEntry.Text = null;
                    ColorEntry.Text = null;
                    AnioEntry.Text = null;
                    PlacaEntry.Text = null;
                    if (Ischange)
                    {
                        await DisplayAlert("Error", "No se encontraron datos para el código ingresado", "OK");
                    }
                   
                }
            }
        }
    }
    private async void OnCodigoEntered(object sender, EventArgs e)
    {
        await BuscarDatosVehiculo(false);
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        // Validar que los campos obligatorios no estén vacíos
        if (
            string.IsNullOrWhiteSpace(PlacaEntry.Text) ||
            TipoInfraccionPicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(DescripcionEditor.Text) )
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios y deben tener valores válidos.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(rutafoto))
        {
            await DisplayAlert("Error", "Ten en cuenta que no tomaste ninguna foto", "OK");
           
        }
        if (string.IsNullOrWhiteSpace(rutaAudio))
        {
            await DisplayAlert("Error", "Ten en cuenta que no grabaste ningún audio", "OK");
        }
        var multa = new Multa
        {
            CodigoMarbete = CodigoEntry.Text,
            Marca = MarcaEntry.Text,
            Modelo = ModeloEntry.Text,
            Color = ColorEntry.Text,
            Anio = Convert.ToInt32(AnioEntry.Text),
            Placa = PlacaEntry.Text,
            TipoInfraccion = TipoInfraccionPicker.SelectedItem.ToString(),
            FechaHora = FechaPicker.Date.Add(HoraPicker.Time),
            Descripcion = DescripcionEditor.Text,
            FotoPath = rutafoto,
            AudioPath = rutaAudio
        };

        await _db.SaveMultaAsync(multa);
        await DisplayAlert("Éxito", "Multa guardada correctamente", "OK");
    }


    private async void OnTomarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                rutafoto = Path.Combine(FileSystem.AppDataDirectory, foto.FileName);

                using (var stream = await foto.OpenReadAsync())
                using (var newStream = File.OpenWrite(rutafoto))
                {
                    await stream.CopyToAsync(newStream);
                }

                await DisplayAlert("Foto tomada", "Imagen guardada correctamente", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
        }
    }

    private async void OnGrabarAudioClicked(object sender, EventArgs e)
    {
        try
        {
            // Solicitar permiso de micrófono
            if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "No se ha otorgado permiso para grabar audio", "OK");
                return;
            }

            // Obtener el grabador de audio

            if (!recorder.IsRecording)
            {
                // Comenzar grabación
                await recorder.StartAsync();
                btnGrabar.Text = "Grabando...";
                await DisplayAlert("Grabando", "La grabación ha iniciado", "OK");
            }
            else
            {
                btnGrabar.Text = "🎤 Grabar Audio";
                // Detener grabación
                var s = await recorder.StopAsync();
                    
                // Obtener el Stream del audio grabado
                using (var audioStream = s.GetAudioStream())
                {
                    if (audioStream != null)
                    {
                        // Definir la ruta del archivo donde se guardará
                        rutaAudio = Path.Combine(FileSystem.AppDataDirectory, $"audio_{DateTime.Now:yyyyMMddHHmmss}.mp3");

                        // Guardar el Stream en un archivo
                        using (var fileStream = new FileStream(rutaAudio, FileMode.Create, FileAccess.Write))
                        {
                            await audioStream.CopyToAsync(fileStream);
                        }

                        // Verificar si el archivo realmente existe
                        if (File.Exists(rutaAudio))
                        {
                            // Reproducir el audio guardado
                            var player = AudioManager.Current.CreatePlayer(rutaAudio);
                            await DisplayAlert("Grabación finalizada", $"El audio ha sido guardado", "OK");

                        }
                        else
                        {
                            await DisplayAlert("Error", "No se pudo guardar la grabación", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo obtener el stream del audio", "OK");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo grabar el audio: {ex.Message}", "OK");
        }


    }

    private async void CodigoEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        await BuscarDatosVehiculo(false);

    }
}
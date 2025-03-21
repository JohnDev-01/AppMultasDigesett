using AppMultasDigesett.Modelo;
using Plugin.Maui.Audio;

namespace AppMultasDigesett.Vistas;

public partial class DetalleMulta : ContentPage
{
    private IAudioPlayer _audioPlayer;
    private string _rutaAudio;

    public DetalleMulta(Multa multa)
    {
        InitializeComponent();

        // Mostrar datos en la UI
        PlacaLabel.Text = multa.Placa;
        InfraccionLabel.Text = multa.TipoInfraccion;
        FechaHoraLabel.Text = multa.FechaHora.ToString("dd/MM/yyyy HH:mm");
        DescripcionLabel.Text = multa.Descripcion;
        _rutaAudio = multa.AudioPath;

        // Mostrar la foto si existe
        if (!string.IsNullOrEmpty(multa.FotoPath) && File.Exists(multa.FotoPath))
        {
            FotoImage.Source = multa.FotoPath;
        }
        else
        {
            FotoImage.IsVisible = false;
        }

        // Inicializar el reproductor de audio si hay un archivo disponible
        if (!string.IsNullOrEmpty(_rutaAudio) && File.Exists(_rutaAudio))
        {
            _audioPlayer = AudioManager.Current.CreatePlayer(_rutaAudio);
        }
    }
    
    private void OnPlayAudioClicked(object sender, EventArgs e)
    {
        if (_audioPlayer != null)
        {
            _audioPlayer.Play();
        }
        else
        {
            DisplayAlert("Error", "No hay audio disponible", "OK");
        }
    }
}
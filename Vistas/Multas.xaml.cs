using AppMultasDigesett.Modelo;
using AppMultasDigesett.Servicio;
using Plugin.Maui.Audio;
using System.Threading.Tasks;

namespace AppMultasDigesett.Vistas;

public partial class Multas : ContentPage
{
    private SQLiteHelper _db;

    public Multas()
	{
		InitializeComponent();
        _db = new SQLiteHelper();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarMultas();
    }

    private async Task CargarMultas()
    {
        var multas = await _db.GetMultasAsync();
        MultasCollectionView.ItemsSource = multas;
    }

    private async void OnMultaSelected(object sender, SelectionChangedEventArgs e)
    {
      
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var audioManager = Application.Current.MainPage.Handler.MauiContext.Services.GetService<IAudioManager>();
        await Navigation.PushAsync(new CrearMulta(audioManager));


    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var multaSeleccionada = (Multa)((Frame)sender).BindingContext;
        if (multaSeleccionada != null)
        {
            await Navigation.PushAsync(new DetalleMulta(multaSeleccionada));
        }
    }
}
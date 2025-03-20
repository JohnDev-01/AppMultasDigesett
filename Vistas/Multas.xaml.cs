using Plugin.Maui.Audio;
using System.Threading.Tasks;

namespace AppMultasDigesett.Vistas;

public partial class Multas : ContentPage
{
	public Multas()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var audioManager = Application.Current.MainPage.Handler.MauiContext.Services.GetService<IAudioManager>();
        await Navigation.PushAsync(new CrearMulta(audioManager));


    }
}
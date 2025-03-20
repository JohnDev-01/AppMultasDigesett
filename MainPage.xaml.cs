using AppMultasDigesett.Vistas;

namespace AppMultasDigesett
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await imagenPolice.FadeTo(1, 500);
            await imagenPolice.FadeTo(0, 500);
            await imagenPolice.FadeTo(1, 500);
            await imagenPolice.FadeTo(0, 500);
            await imagenPolice.FadeTo(1, 500);
            Application.Current.MainPage = new NavigationPage(new Contenedotr());
        }
    }

}

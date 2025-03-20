using AppMultasDigesett.Servicio;

namespace AppMultasDigesett.Vistas;

public partial class Seguridad : ContentPage
{
    private SQLiteHelper _db;

    public Seguridad()
    {
        InitializeComponent();
        _db = new SQLiteHelper();
    }

    private async void OnBorrarTodoClicked(object sender, EventArgs e)
    {
        bool confirmacion = await DisplayAlert("Confirmación",
            "⚠️ ¿Está seguro de que desea eliminar TODAS las multas? Esta acción NO se puede deshacer.",
            "Sí, borrar todo", "Cancelar");

        if (confirmacion)
        {
            await _db.DeleteAllMultasAsync();
            await DisplayAlert("Éxito", "Todas las multas han sido eliminadas.", "OK");
        }
    }
}
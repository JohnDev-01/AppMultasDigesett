namespace AppMultasDigesett.Vistas;

public partial class AcercaDe : ContentPage
{
	public AcercaDe()
	{
		InitializeComponent();
        AgenteFoto.Source = "miimagen.jpg";
        NombreLabel.Text = "John Kerlin Silvestre";
        MatriculaLabel.Text = "20231192";
        FraseLabel.Text = "🚸 La seguridad vial no es solo una responsabilidad, es un compromiso con la vida.";
    }
}

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Proyecto_A.Data;
using Proyecto_A.Models;

namespace Proyecto_A.ViewModels;

public class HomeViewModel
{
    private readonly DatabaseService _databaseService = new();

    public ObservableCollection<ContenidoModel> Contenidos { get; set; } = new();

    public HomeViewModel()
    {
        _ = CargarContenidos();
    }

    private async Task CargarContenidos()
    {
        var lista = await _databaseService.ObtenerContenidosAsync();
        Contenidos.Clear();
        foreach (var item in lista)
        {
            Contenidos.Add(item);
        }
    }
}

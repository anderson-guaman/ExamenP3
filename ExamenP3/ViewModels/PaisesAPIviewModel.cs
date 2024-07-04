
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ExamenP3.Models;

namespace ExamenP3;
public class PaisesAPIviewModel : INotifyPropertyChanged
{
    private readonly PaisRepository _paisRepository;
    private ObservableCollection<Pais> _paises;

    public ObservableCollection<Pais> Paises
    {
        get => _paises;
        set
        {
            _paises = value;
            OnPropertyChanged();
        }
    }

    public ICommand ObtenerPaisesCommand { get; }

    public PaisesAPIviewModel()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "paises.db3");
        _paisRepository = new PaisRepository(dbPath);
        Paises = new ObservableCollection<Pais>();
        ObtenerPaisesCommand = new Command(async () => await ObtenerPaisesAsync());
        LoadPaises();
    }

    private async Task ObtenerPaisesAsync()
    {
        var paises = await _paisRepository.ObtenerPaisesDeApiAsync();
        Random random = new Random();
        foreach (var pais in paises)
        {
            string codigo = $"SC{random.Next(1000, 2000)}";
            var paisTable = new PaisTable
            {
                Name = pais.Name.Official,
                Region = pais.Region,
                SubRegion = pais.SubRegion,
                Status = pais.Status,
                Codigo = codigo
            };
            await _paisRepository.InsertPaisAsync(paisTable);
        }
        LoadPaises();
    }

    private async void LoadPaises()
    {
        Paises.Clear();
        var paises = await _paisRepository.GetPaisesAsync();
        foreach (var pais in paises)
        {
            Paises.Add(new Pais
            {
                Name = new PaisName { Official = pais.Name },
                Region = pais.Region,
                SubRegion = pais.SubRegion,
                Status = pais.Status
            });
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

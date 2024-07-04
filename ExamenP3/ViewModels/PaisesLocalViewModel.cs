using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamenP3;

public class PaisesLocalViewModel : INotifyPropertyChanged
{
    private readonly PaisRepository _paisRepository;
    private ObservableCollection<PaisTable> _paises;

    public ObservableCollection<PaisTable> Paises
    {
        get => _paises;
        set
        {
            _paises = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoadPaisesCommand { get; }
    public ICommand UpdatePaisCommand { get; }
    public ICommand DeletePaisCommand { get; }

    public PaisesLocalViewModel()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "paises.db3");
        _paisRepository = new PaisRepository(dbPath);
        Paises = new ObservableCollection<PaisTable>();
        LoadPaisesCommand = new Command(async () => await LoadPaises());
        UpdatePaisCommand = new Command<PaisTable>(async (pais) => await UpdatePais(pais));
        DeletePaisCommand = new Command<PaisTable>(async (pais) => await DeletePais(pais));
        LoadPaises();
    }

    private async Task LoadPaises()
    {
        Paises.Clear();
        var paises = await _paisRepository.GetPaisesAsync();
        foreach (var pais in paises)
        {
            Paises.Add(pais);
        }
    }

    private async Task UpdatePais(PaisTable pais)
    {
        await _paisRepository.UpdatePaisAsync(pais);
        await LoadPaises();
    }

    private async Task DeletePais(PaisTable pais)
    {
        await _paisRepository.DeletePaisAsync(pais);
        await LoadPaises();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

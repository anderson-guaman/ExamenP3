

using System.ComponentModel;

namespace ExamenP3.ViewModels
{
    internal class PaisesAPIviewModel : INotifyPropertyChanged
    {
        private readonly DatabaseHelper _dbHelper;
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

        public MainViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "paises.db3");
            _dbHelper = new DatabaseHelper(dbPath);
            Paises = new ObservableCollection<Pais>();
            ObtenerPaisesCommand = new Command(async () => await ObtenerPaisesAsync());
            LoadPaises();
        }

        private async Task ObtenerPaisesAsync()
        {
            var paises = await ObtenerPaisesDeApiAsync();
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
                await _dbHelper.InsertPaisAsync(paisTable);
            }
            LoadPaises();
        }

        private async void LoadPaises()
        {
            Paises.Clear();
            var paises = await _dbHelper.GetPaisesAsync();
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

        private async Task<List<Pais>> ObtenerPaisesDeApiAsync()
        {
            string apiUrl = "URL_DE_LA_API"; // Reemplaza con la URL de la API
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Pais>>(jsonResponse);
                }
                return new List<Pais>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

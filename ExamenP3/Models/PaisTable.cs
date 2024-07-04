using SQLite;

public class PaisTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }
    public string SubRegion { get; set; }
    public string Status { get; set; }
    public string Codigo { get; set; }
}

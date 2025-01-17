﻿using SQLite;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using ExamenP3.Models;

public class PaisRepository
{
    private readonly SQLiteAsyncConnection _database;
    private readonly string apiUrl = "URL_DE_LA_API"; // Reemplaza con la URL de la API

    public PaisRepository(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<PaisTable>().Wait();
    }

    public async Task<List<Pais>> ObtenerPaisesDeApiAsync()
    {
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

    public Task<int> InsertPaisAsync(PaisTable pais)
    {
        return _database.InsertAsync(pais);
    }

    public Task<List<PaisTable>> GetPaisesAsync()
    {
        return _database.Table<PaisTable>().ToListAsync();
    }

    public Task<int> UpdatePaisAsync(PaisTable pais)
    {
        return _database.UpdateAsync(pais);
    }

    public Task<int> DeletePaisAsync(PaisTable pais)
    {
        return _database.DeleteAsync(pais);
    }
}

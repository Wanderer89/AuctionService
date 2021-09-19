using AuctionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Providers
{
    public interface IProvider
    {
        Task<bool> ExecuteNonQueryAsync(string sqlQuery);
        Task<T> ExecuteScalarReaderAsync<T>(string sqlQuery) where T : IInflatable, new();
        Task<List<T>> ExecuteMultiReaderAsync<T>(string sqlQuery) where T : IInflatable, new();
    }
}

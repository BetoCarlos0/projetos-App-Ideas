﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dotflix.Models.Contracts.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> AddAsync(Movie movie);
        Task<Movie> GetByIdAsync(int id);
        void Update(Movie movie);
        void DeleteId(int id);
    }
}
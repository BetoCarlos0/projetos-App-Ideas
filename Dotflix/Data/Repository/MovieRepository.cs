﻿using Dotflix.Models;
using Dotflix.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotflix.Data.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DotflixDbContext _dbContext;

        public MovieRepository(DotflixDbContext dotflixDbContext)
        {
            _dbContext = dotflixDbContext;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Movie
                .Include(x => x.MovieLanguages)
                    .ThenInclude(x => x.Language)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _dbContext.Movie
                .Include(x => x.MovieLanguages)
                    .ThenInclude(x => x.Language)
                .FirstOrDefaultAsync(x => x.MovieId == id);
        }
        public async Task AddAsync(Movie movie)
        {
            await _dbContext.Movie.AddAsync(movie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            var getMovie = await _dbContext.Movie
                .FirstOrDefaultAsync(e => e.MovieId == movie.MovieId);

            if (getMovie != null)
            {
                getMovie.Image = movie.Image;
                getMovie.Title = movie.Title;
                getMovie.Sinopse = movie.Sinopse;
                getMovie.Relevance = movie.Relevance;
                getMovie.ReleaseData = movie.ReleaseData;
                getMovie.RunTime = movie.RunTime;
                getMovie.AgeGroup = movie.AgeGroup;
                getMovie.Languages = movie.Languages;
                getMovie.MovieLanguages = movie.MovieLanguages;

                await _dbContext.SaveChangesAsync();

                //return getMovie;
            }
            //return null;
        }
        public async Task DeleteId(int id)
        {
            var getMovie = await _dbContext.Movie
                .FirstOrDefaultAsync(e => e.MovieId == id);

            if (getMovie != null)
            {
                _dbContext.Movie.Remove(getMovie);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

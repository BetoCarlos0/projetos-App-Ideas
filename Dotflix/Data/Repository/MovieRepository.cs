﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiDotflix.Entities;
using ApiDotflix.Entities.Models.Contracts.Repositories;
using ApiDotflix.Entities.Models.Dtos;
using System.Linq;

namespace ApiDotflix.Data.Repository
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
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            var getMovie = await _dbContext.Movie
                .Include(x => x.About)
                    .ThenInclude(x => x.AboutCasts)
                        .ThenInclude(x => x.Cast)
                .Include(x => x.About)
                    .ThenInclude(x => x.AboutGenres)
                        .ThenInclude(x => x.Genre)
                .Include(x => x.About)
                    .ThenInclude(x => x.AboutKeywords)
                        .ThenInclude(x => x.Keyword)
                .Include(x => x.About)
                    .ThenInclude(x => x.AboutLanguages)
                        .ThenInclude(x => x.Language)
                .Include(x => x.About)
                    .ThenInclude(x => x.AboutRoadMaps)
                        .ThenInclude(x => x.RoadMap)
                .Include(x => x.About)
                    .ThenInclude(x => x.Director)
                .FirstOrDefaultAsync(x => x.MovieId.Equals(id));

            if (getMovie == null)
                throw new DbUpdateException("Id não encontrado");

            return getMovie;
        }

        public async Task<Movie> GetByNameAsync(string name)
        {
            return await _dbContext.Movie.
                FirstOrDefaultAsync(x => x.Title.Equals(name));
        }

        public async Task<bool> AddAsync(Movie movie)
        {
            await NameExist(movie.MovieId, movie.Title);
            
            if (!movie.About.AboutGenres.Any())
                throw new DbUpdateException("Gênero Vazio");
            if (!movie.About.AboutLanguages.Any())
                throw new DbUpdateException("Idioma Vazio");
            if (!movie.About.AboutCasts.Any())
                throw new DbUpdateException("Elenco Vazio");
            
            await _dbContext.Movie.AddAsync(movie);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(MoviePutInputDto movie)
        {

            var getMovie = await _dbContext.Movie
                .FirstOrDefaultAsync(x => x.MovieId.Equals(movie.MovieId));

            await NameExist(movie.MovieId, movie.Title);

            if (getMovie == null) return false;
            
            getMovie.ImageUrl = movie.ImageUrl;
            getMovie.Title = movie.Title;
            getMovie.Sinopse = movie.Sinopse;
            getMovie.Relevance = movie.Relevance;
            getMovie.ReleaseData = movie.ReleaseData;
            getMovie.RunTime = movie.RunTime;
            getMovie.AgeGroupId = movie.AgeGroupId;

            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteId(int id)
        {
            await ExistMovie(id);

            _dbContext.Movie.Remove(await ExistMovie(id));
            await _dbContext.SaveChangesAsync();

            return true;
        }
        private async Task<Movie> ExistMovie(int id)
        {
            var getMovie = await _dbContext.Movie.FindAsync(id);

            if (getMovie == null)
                throw new DbUpdateException("Id não encontrado");

            return getMovie;
        }
        private async Task NameExist(int id, string title)
        {
            var getMovie = await _dbContext.Movie
                .FirstOrDefaultAsync(x => x.Title.Equals(title));

            if (getMovie != null && id != getMovie.MovieId)
                throw new DbUpdateException($"{title} já existente");
        }
    }
}

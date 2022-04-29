﻿using Dotflix.Data;
using Dotflix.Models;
using Dotflix.Models.Contracts;
using Dotflix.Models.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dotflix.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            return Ok(await _movieService.GetAllAsync());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (id <= 0) return BadRequest();

            var result = await _movieService.GetByIdAsync(id);

            if (result == null) return NotFound($"400 - Filme com Id {id} não encontrado");
                
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateMovie(Movie movie)
        {
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            try
            {
                await _movieService.AddAsync(movie).ConfigureAwait(false);

                return CreatedAtAction(nameof(GetMovie),
                        new { id = movie.MovieId }, movie);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
                return BadRequest("Id e Filme incompatíveis");

            var result = await _movieService.GetByIdAsync(id);

            if(result == null)
                return NotFound($"Filme com Id {id} não encontrado");

            try
            {
                await _movieService.UpdateAsync(movie);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _movieService.GetByIdAsync(id);

            if (result == null)
                return NotFound($"Filme com Id {id} não encontrado");

            await _movieService.DeleteId(id);

            return NoContent();
        }
    }
}

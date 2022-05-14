﻿using ApiDotflix.Models;
using ApiDotflix.Models.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiDotflix.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        /*[ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies()
        {
            return Ok(await _aboutService.GetAllAsync());
        }*/

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetAbout(int id)
        {
            try
            {
                return Ok(await _aboutService.GetByIdAsync(id));
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao recuperar dados do banco de dados");
            }
        }

        /*[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie about)
        {
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            try
            {
                await _aboutService.AddAsync(about).ConfigureAwait(false);

                return CreatedAtAction(nameof(GetMovie),
                        new { id = about.MovieId }, about);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao recuperar dados do banco de dados");
            }
        }*/

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbout(int id, About about)
        {
            if (!ModelState.IsValid) return BadRequest(new ValidationProblemDetails(ModelState));

            if (id != about.MovieId)
                return BadRequest("Id e Filme incompatíveis");

            try
            {
                return Ok(await _aboutService.UpdateAsync(about).ConfigureAwait(false));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /*[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _aboutService.DeleteId(id));
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao recuperar dados do banco de dados");
            }
        }*/
    }
}
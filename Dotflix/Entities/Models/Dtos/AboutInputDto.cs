﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiDotflix.Entities.Models.Dtos
{
    public class AboutInputDto
    {
        public int AboutId { get; set; }

        [Required(ErrorMessage = "Filme Id obrigatório")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Diretor obrigatório")]
        public int DirectorId { get; set; }

        [Required(ErrorMessage = "Elenco obrigatório")]
        public IEnumerable<BaseEntityDto> Casts { get; set; }
        
        [Required(ErrorMessage = "Gênero obrigatório")]
        public IEnumerable<BaseEntityDto> Genres { get; set; }

        [Required(ErrorMessage = "Idioma obrigatório")]
        public IEnumerable<BaseEntityDto> Languages { get; set; }

        public IEnumerable<BaseEntityDto>? RoadMaps { get; set; }

        public IEnumerable<BaseEntityDto>? Keywords { get; set; }
    }
}

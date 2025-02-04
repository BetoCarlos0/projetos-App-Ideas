﻿using ApiDotflix.Entities.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDotflix.Entities
{
    public class Language : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<AboutLanguage> AboutLanguages { get; set; }
    }
}

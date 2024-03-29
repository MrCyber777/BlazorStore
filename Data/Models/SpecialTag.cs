﻿using System.ComponentModel.DataAnnotations;

namespace BlazorStore.Data.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field Name is required")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 characters")]
        public string Name { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        //adding data annotations for data validation
        [Required]   
        [MinLength(5, ErrorMessage = "Title can not be under 5 characters.")]
        [MaxLength(20, ErrorMessage = "Title could be at most 20 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content can not be under 5 characters.")]
        [MaxLength(160, ErrorMessage = "Content could be at most 160 characters.")]
        public string Content { get; set; } = string.Empty;

    }
}
﻿using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record CreateNewTodoListDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}

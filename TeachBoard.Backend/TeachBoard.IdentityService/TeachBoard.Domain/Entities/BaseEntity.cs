﻿using System.ComponentModel.DataAnnotations;

namespace TeachBoard.Domain.Entities;

// Parent type for each entity
public class BaseEntity
{
    [Key] public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
﻿using MongoDB.Bson;

namespace ECourse.Admin.Models;

public class BaseDto
{
    public string Id { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    public DateTime? ModifiedDateTime { get; set; }
    public bool IsDeleted { get; set; } = false;
}

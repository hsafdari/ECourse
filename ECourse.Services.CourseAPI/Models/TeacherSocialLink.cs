﻿using Infrastructure.Models;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Models
{
    public class TeacherSocialLink:BaseEntity
    {
        public static readonly string DocumentName = nameof(TeacherSocialLink);
        public ObjectId TeacherId { get; set; }
        public ObjectId SocialLinkId { get; set; }
        public required string Url { get; set; }
    }
}

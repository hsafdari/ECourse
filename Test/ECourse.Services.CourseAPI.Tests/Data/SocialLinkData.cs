using ECourse.Services.CourseAPI.Models;
using ECourse.Services.CourseAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    public class SocialLinkData : IFakeData<SocialLink, SocialLinkDto>
    {
        public SocialLink FakeGetSingleRow()
        {
            ObjectId SocialLinkId = MongoDB.Bson.ObjectId.GenerateNewId();

            return new SocialLink()
            {
                Id = ObjectId.GenerateNewId(),
                FileName = "FileName1",
                Icon = "Icon1",
                FileLocation = "location1",
                CreateDateTime = DateTime.Now,
                Name = "Instagram"
            };
        }

        public SocialLinkDto FakeGetSingleRowDTO()
        {
            return new SocialLinkDto()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                FileName = "FileName1",
                Icon = "Icon1",
                FileLocation = "location1",
                CreateDateTime = DateTime.Now,
                Name = "Instagram"
            };
        }        
    }
}

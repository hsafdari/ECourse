using ECourse.Services.CourseAPI.Enums;
using MongoDB.Bson;

namespace ECourse.Services.CourseAPI.Tests.Data
{
    public class CourseData
    {
        public CourseAPI.Models.Course GetCourseSignleRow()
        {
            ObjectId courserId = MongoDB.Bson.ObjectId.GenerateNewId();
            ObjectId levelId = new CourseLevelData().FakeGetSingleRow().Id;
            ObjectId sectionId01 = MongoDB.Bson.ObjectId.GenerateNewId();
            ObjectId sectionId02 = MongoDB.Bson.ObjectId.GenerateNewId();
            ObjectId sectionId03 = MongoDB.Bson.ObjectId.GenerateNewId();
            ObjectId itemId01 = MongoDB.Bson.ObjectId.GenerateNewId();
            return new Models.Course()
            {
                Id = courserId,
                LevelId = levelId,
                Name = "courseTest",
                Title = "CourseTitle",
                CreateDateTime = DateTime.Now,
                Description = "Description",
                UrlLink = "http://file.url",
                Sections = new List<Models.CourseSection>()
                {
                    new Models.CourseSection()
                    {
                        Id=sectionId01,
                        CourseId =courserId,
                        Title = "secTitle01",
                        CourseItems= new List<Models.CourseItem>()
                        {
                            new Models.CourseItem()
                            {
                                CourseId=courserId,
                                SectionId=sectionId01,
                                Name="item01",
                                Id=itemId01,
                                FileName="filename01",
                                FileLocation="/Uploads/filetest.mp4",
                                FileUrl="http://test.com",
                                IsPreview=true,
                                CourseType=CourseType.video,
                                TimeDuration="00:42:00",
                                FileExtension="mp4",
                                Description="Description",
                                CourseItemAttachments=new List<Models.CourseItemAttachment>()
                                {
                                    new Models.CourseItemAttachment()
                                    {
                                        CourseItemId=itemId01,
                                        Id=ObjectId.GenerateNewId(),
                                        FileName="Attachment01",
                                        FileLocation="/uploads/file.pdf",
                                        FileUrl="http://domain.com/uploads/file.pdf",
                                        FileExtension="pdf"
                                    },
                                    new Models.CourseItemAttachment()
                                    {
                                        CourseItemId=itemId01,
                                        Id=ObjectId.GenerateNewId(),
                                        FileName="Attachment02",
                                        FileLocation="/uploads/file2.pdf",
                                        FileUrl="http://domain.com/uploads/file2.pdf",
                                        FileExtension="pdf"
                                    }
                                }
                            },
                            new Models.CourseItem()
                                {
                                    CourseId=courserId,
                                    SectionId=sectionId01,
                                    Name="item02",
                                    Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                    FileName="filename02",
                                    FileLocation="/Uploads/filetest.mp4",
                                    FileUrl="http://test.com",
                                    IsPreview=true,
                                    CourseType=CourseType.video,
                                    TimeDuration="00:25:00",
                                    FileExtension="mp4",
                                    Description="Description2"
                                },
                                  new Models.CourseItem()
                                {
                                    CourseId=courserId,
                                    SectionId=sectionId01,
                                    Name="item03",
                                    Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                    FileName="filename03",
                                    FileLocation="/Uploads/filetest3.mp4",
                                    FileUrl="http://test.com",
                                    IsPreview=true,
                                    CourseType=CourseType.video,
                                    TimeDuration="00:43:00",
                                    FileExtension="mp4",
                                    Description="Description3"
                                }
                        }
                    },
                    new Models.CourseSection()
                     {
                        Id=sectionId02,
                         CourseId =courserId,
                         Title = "secTitle02",
                         CourseItems= new List<Models.CourseItem>()
                            {
                                new Models.CourseItem()
                                {
                                    CourseId=courserId,
                                    SectionId=sectionId01,
                                    Name="item021",
                                    Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                    FileName="filename021",
                                    FileLocation="/Uploads/filetest21.mp4",
                                    FileUrl="http://test.com",
                                    IsPreview=true,
                                    CourseType=CourseType.video,
                                    TimeDuration="00:42:21",
                                    FileExtension="mp4",
                                    Description="Description21"
                                },
                                new Models.CourseItem()
                                    {
                                        CourseId=courserId,
                                        SectionId=sectionId01,
                                        Name="item022",
                                        Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                        FileName="filename022",
                                        FileLocation="/Uploads/filetest2.mp4",
                                        FileUrl="http://test.com",
                                        IsPreview=true,
                                        CourseType=CourseType.video,
                                        TimeDuration="00:25:22",
                                        FileExtension="mp4",
                                        Description="Description22"
                                    },
                                new Models.CourseItem()
                                    {
                                        CourseId=courserId,
                                        SectionId=sectionId01,
                                        Name="item023",
                                        Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                        FileName="filename023",
                                        FileLocation="/Uploads/filetest23.mp4",
                                        FileUrl="http://test.com",
                                        IsPreview=true,
                                        CourseType=CourseType.video,
                                        TimeDuration="00:43:23",
                                        FileExtension="mp4",
                                        Description="Description23"
                                    }
                            }
                     },
                     new Models.CourseSection()
                     {
                         Id=sectionId03,
                         CourseId =courserId,
                         Title = "secTitle03",
                         CourseItems= new List<Models.CourseItem>()
                            {
                                new Models.CourseItem()
                                {
                                    CourseId=courserId,
                                    SectionId=sectionId01,
                                    Name="item031",
                                    Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                    FileName="filename031",
                                    FileLocation="/Uploads/filetest.mp4",
                                    FileUrl="http://test.com",
                                    IsPreview=true,
                                    CourseType=CourseType.video,
                                    TimeDuration="00:42:31",
                                    FileExtension="mp4",
                                    Description="Description31"
                                },
                                new Models.CourseItem()
                                    {
                                        CourseId=courserId,
                                        SectionId=sectionId01,
                                        Name="item032",
                                        Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                        FileName="filename032",
                                        FileLocation="/Uploads/filetest.mp4",
                                        FileUrl="http://test.com",
                                        IsPreview=true,
                                        CourseType=CourseType.video,
                                        TimeDuration="00:25:32",
                                        FileExtension="mp4",
                                        Description="Description32"
                                    },
                                new Models.CourseItem()
                                    {
                                        CourseId=courserId,
                                        SectionId=sectionId01,
                                        Name="item033",
                                        Id=MongoDB.Bson.ObjectId.GenerateNewId(),
                                        FileName="filename033",
                                        FileLocation="/Uploads/filetest33.mp4",
                                        FileUrl="http://test.com",
                                        IsPreview=true,
                                        CourseType=CourseType.video,
                                        TimeDuration="00:43:33",
                                        FileExtension="mp4",
                                        Description="Description33"
                                    }
                            }
                     }

                },
                Prices = new List<Models.CoursePrice>()
                {
                     new Models.CoursePrice(){
                          CourseId = courserId,
                          Price=125,
                          CurrencyCode="USD"
                     },
                     new Models.CoursePrice(){
                         CourseId =courserId,
                         Price=120,
                         CurrencyCode="EUR"
                    },
                     new Models.CoursePrice(){
                            CourseId = courserId,
                            Price=125,
                            CurrencyCode="GBP"
                    }
                }                
            };
        }
    }
}

using Cinnamon.Core.Models;

namespace Cinnamon.Core
{
    public class ApplicationViewModel
    {
        public async Task applySeedDemoData() {

            // Create Experience Types 
            var experienceTypes = await CoreDI.DataStore.ExperienceTypes.GetAllAsync();
            if (experienceTypes.Count == 0) 
            {
                await CoreDI.DataStore.ExperienceTypes.SaveDataAsync(new ExperienceTypeModel {
                    Id = 1,
                    Name = "In-Person"
                });
                await CoreDI.DataStore.ExperienceTypes.SaveDataAsync(new ExperienceTypeModel
                {
                    Id = 2,
                    Name = "Online"
                });
            }

            var subcategories = await CoreDI.DataStore.SubCategory.GetAllAsync();
            if(subcategories.Count == 0)
            {
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 1,
                    CatergoryId = 1,
                    SubCatergory = "Archery"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 2,
                    CatergoryId = 1,
                    SubCatergory = "Baseball"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 3,
                    CatergoryId = 1,
                    SubCatergory = "Basketball"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 4,
                    CatergoryId = 1,
                    SubCatergory = "Cheerleading"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 5,
                    CatergoryId = 1,
                    SubCatergory = "Dance"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 6,
                    CatergoryId = 1,
                    SubCatergory = "Equestiran (Equine Sports)"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 7,
                    CatergoryId = 1,
                    SubCatergory = "Field Hockey"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 8,
                    CatergoryId = 1,
                    SubCatergory = "Football"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 9,
                    CatergoryId = 1,
                    SubCatergory = "Golf"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 10,
                    CatergoryId = 1,
                    SubCatergory = "Gymnastics"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 11,
                    CatergoryId = 1,
                    SubCatergory = "Ice Hockey"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 12,
                    CatergoryId = 1,
                    SubCatergory = "Karate"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 13,
                    CatergoryId = 1,
                    SubCatergory = "Lacrosse"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 14,
                    CatergoryId = 1,
                    SubCatergory = "Rowing"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 15,
                    CatergoryId = 1,
                    SubCatergory = "Snowboarding"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 16,
                    CatergoryId = 1,
                    SubCatergory = "Soccer"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 17,
                    CatergoryId = 1,
                    SubCatergory = "Surfing"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 18,
                    CatergoryId = 1,
                    SubCatergory = "Swimming"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 19,
                    CatergoryId = 1,
                    SubCatergory = "Table Tennis"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 20,
                    CatergoryId = 1,
                    SubCatergory = "Tennis"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 21,
                    CatergoryId = 1,
                    SubCatergory = "Track and Field"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 22,
                    CatergoryId = 1,
                    SubCatergory = "Volleyball"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 23,
                    CatergoryId = 1,
                    SubCatergory = "Chess"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 24,
                    CatergoryId = 2,
                    SubCatergory = "Math"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 25,
                    CatergoryId = 2,
                    SubCatergory = "Science"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 26,
                    CatergoryId = 2,
                    SubCatergory = "Filipino"
                });
                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 27,
                    CatergoryId = 2,
                    SubCatergory = "Social Studies"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 28,
                    CatergoryId = 2,
                    SubCatergory = "Art"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 29,
                    CatergoryId = 2,
                    SubCatergory = "Music"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 30,
                    CatergoryId = 2,
                    SubCatergory = "English"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 31,
                    CatergoryId = 2,
                    SubCatergory = "Araling Panlipunan"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 32,
                    CatergoryId = 2,
                    SubCatergory = "Creative Writing"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 33,
                    CatergoryId = 2,
                    SubCatergory = "Coding"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 34,
                    CatergoryId = 2,
                    SubCatergory = "Reading"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 35,
                    CatergoryId = 2,
                    SubCatergory = "Writing"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 36,
                    CatergoryId = 4,
                    SubCatergory = "Mandarin"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 37,
                    CatergoryId = 4,
                    SubCatergory = "Spanish"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 38,
                    CatergoryId = 4,
                    SubCatergory = "Japanese"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 39,
                    CatergoryId = 4,
                    SubCatergory = "Tagalog"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 40,
                    CatergoryId = 4,
                    SubCatergory = "Korean"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 41,
                    CatergoryId = 4,
                    SubCatergory = "English"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 42,
                    CatergoryId = 5,
                    SubCatergory = "Piano"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 43,
                    CatergoryId = 5,
                    SubCatergory = "Guitar"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 44,
                    CatergoryId = 5,
                    SubCatergory = "Drums"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 45,
                    CatergoryId = 5,
                    SubCatergory = "Bass"
                });

                await CoreDI.DataStore.SubCategory.SaveDataAsync(new SubCategoryModel()
                {
                    Id = 46,
                    CatergoryId = 3,
                    SubCatergory = "Fun Play"
                });
            }

            //Create Activities
            var activityTypes = await CoreDI.DataStore.ActivityTypes.GetAllAsync();
            if (activityTypes.Count == 0)
            {
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 1,
                    Name = "Football",
                    Icon = "images/Category/Football.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 2,
                    Name = "Baking",
                    Icon = "images/Category/Baking.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 3,
                    Name = "Ballet",
                    Icon = "images/Category/Ballet.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 4,
                    Name = "Kids Gymnastics",
                    Icon = "images/Category/Gymnastics.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 5,
                    Name = "Fun Play",
                    Icon = "images/Category/Play.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 6,
                    Name = "Horseback Riding Lesson",
                    Icon = "images/Category/Horseback.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 7,
                    Name = "Kids Language Tutor",
                    Icon = "images/Category/Language.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 8,
                    Name = "Basketball Lessons",
                    Icon = "images/Category/Basketball.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 9,
                    Name = "Biking Group Activity",
                    Icon = "images/Category/Biking.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 10,
                    Name = "Golf Lessons",
                    Icon = "images/Category/Golf.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 11,
                    Name = "Swimming Lessons",
                    Icon = "images/Category/Swimming.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 12,
                    Name = "Tennis Lessons",
                    Icon = "images/Category/Tennis.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 13,
                    Name = "Taekwondo Lessons",
                    Icon = "images/Category/Taekwondo.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 14,
                    Name = "Theater Acting Lessons",
                    Icon = "images/Category/Theater.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 15,
                    Name = "Art Classes",
                    Icon = "images/Category/Art.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 16,
                    Name = "Online Ballet Classes",
                    Icon = "images/Category/Online-Ballet.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 17,
                    Name = "Coding",
                    Icon = "images/Category/Coding.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 18,
                    Name = "Chess Lessons for Kids",
                    Icon = "images/Category/Chess.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 19,
                    Name = "Yoga for Kids",
                    Icon = "images/Category/Yoga.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 20,
                    Name = "Guitar Lessons",
                    Icon = "images/Category/Guitar.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 21,
                    Name = "Violin Lessons",
                    Icon = "images/Category/Violin.png"
                });
                await CoreDI.DataStore.ActivityTypes.SaveDataAsync(new ActivityTypeModel
                {
                    Id = 22,
                    Name = "Piano Lessons",
                    Icon = "images/Category/Piano.png"
                });
            }

            var ExperienceCategory = await CoreDI.DataStore.ExperienceCategory.GetAllAsync();
            if (ExperienceCategory.Count != 0)
            {
                if (ExperienceCategory[0].Category != "New")
                {
                    foreach (var item in ExperienceCategory)
                    {
                        await CoreDI.DataStore.ExperienceCategory.DeleteDataAsync(item);
                    }
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 1,
                        Category = "New",
                        IconPath = "images/Category/New.png"

                    });
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 2,
                        Category = "Academics",
                        IconPath = "images/Category/Academic.png"
                        
                    });
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 3,
                        Category = "Language",
                        IconPath = "images/Category/Language.png"
                    });
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 4,
                        Category = "Music",
                        IconPath = "images/Category/Music.png"
                    });
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 5,
                        Category = "Skill",
                        IconPath = "images/Category/Skill.png"
                    });
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 6,
                        Category = "SPED",
                        IconPath = "images/Category/SPED.png"
                    });
                    await CoreDI.DataStore.ExperienceCategory.SaveDataAsync(new ExperienceCategoryModel
                    {
                        Id = 7,
                        Category = "Sports",
                        IconPath = "images/Category/Sports.png"
                    });
                }
            }
        }  

    }
}

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

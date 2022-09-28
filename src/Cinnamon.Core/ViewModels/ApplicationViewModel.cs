using System.Reflection.PortableExecutable;

namespace Cinnamon.Core
{
    public class ApplicationViewModel
    {
        void test() {
            // Test Call for Database
            CoreDI.DataStore.Activities.GetAllAsync();
        }
        public List<ActivityTypeModel> type;
        public List<ActivityModel> act;
        public List<ActivityImagesModels> images;
        public async Task applySeedDemoData() {
            // Create Experience Types 
            var experienceTypes = await CoreDI.DataStore.ExperienceTypes.GetAllAsync();
            if (experienceTypes.Count == 0) {
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
            if (activityTypes.Count != 0)
            {
                var result = activityTypes.Where(p => p.Id == 1).ToList();
                foreach (var item in result)
                {
                    if (item.Name != "Football")
                    {
                        type = await CoreDI.DataStore.ActivityTypes.GetAllAsync();
                        foreach (var items in type)
                        {
                            await CoreDI.DataStore.ActivityTypes.DeleteDataAsync(items);
                        }
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
                }
            }

            //Create Activities
            var activities = await CoreDI.DataStore.Activities.GetAllAsync();
            if (activities.Count != 0) {
                var result = activities.Where(p => p.Id == 1).ToList();
                foreach (var item in result)
                {
                    if (item.Title != "Paul Stiga Soccer Training")
                    {
                        act = await CoreDI.DataStore.Activities.GetAllAsync();
                        foreach (var items in act)
                        {
                            await CoreDI.DataStore.Activities.DeleteDataAsync(items);
                        }
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 1,
                            ActivityTypeId = 1,
                            ExperienceTypeId = 1,
                            Title = "Paul Stiga Soccer Training",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 8000 / person / 10 sessions",
                            Subtitle = "Soccer Training with Coach Paul Stiga",
                            Description = "We focus on player developement by focusing on the details and ball mastery. Technique matters! \r\n\r\nIndividual, group and team training",
                            ScheduleIndicator = "info",
                            Schedules = "Session 1 \r\nMonday - 3pm-5pm\r\n\r\nSession 2\r\nWednesday - 3-5pm\r\n\r\nSession 3\r\nFriday - 3-5pm\r\n\r\nSession 4\r\nSaturday - 9am-11am\r\n",
                            MapDetails = "Ceunca Park, Ayala Alabang",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = "the initial"
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 2,
                            ActivityTypeId = 2,
                            ExperienceTypeId = 1,
                            Title = "A unique baking experience",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 2500 / person / 4 sessions",
                            Subtitle = "A unique baking experience",
                            Description = "This program is especially designed for your kids dynamic imagination and enless curiosity. Your children will both learn and have fun in the kitchen. We are passionate about teaching kids and has been formally trained in child development, primary education and special education.\r\n\r\nProgram highlights: \r\n- To provide children with fun and creative activities at home\r\n- To incorporate lessons for your children and get them to make friend\r\n\r\nOverview\r\nSession 1 - Tuna Sub Sandwich\r\nSession 2 - Baggie Vanilla Ice Cream\r\nSession 3 - Banana Nut Oatleal\r\nSession 4 - My signature pizza",
                            ScheduleIndicator = "info",
                            Schedules = "Session 1 \r\nMonday - 3pm-6pm\r\n\r\nSession 2\r\nTuesday - 3-6pm\r\n\r\nSession 3\r\nThursday - 3-6pm\r\n\r\nSession 4\r\nSaturday - 8:15am-10am\"",
                            MapDetails = "Las Flores in Festival ",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 3,
                            ActivityTypeId = 3,
                            ExperienceTypeId = 1,
                            Title = "Ballet Class",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 500 / person / session",
                            Subtitle = "Ballet Class",
                            Description = "This ballet program is especially designed to develop and expand each child’s individual creativity and artistic abilities.\r\nBallet Day Care Class teaches 1 to 5 year olds through creative imagery and role-play rather than the technical approach traditionally associated with ballet. It develops an appreciation of rhythm and timing, music, movement and grace during the most receptive time in their lives. This program also develops gross motor skills, stimulates social interaction and ability to follow instruction and, gain confidence to take that first step on stage!\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "5 session - Sunday 2:30pm - 3:30pm\r\n\r\n10 session - Sunday 2:30pm - 3:30pm\r\n",
                            MapDetails = "Alabang Country Club",
                            Guarantee = "\"Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 4,
                            ActivityTypeId = 4,
                            ExperienceTypeId = 1,
                            Title = "Kids Gymnastics\r\n",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 7500 / person / 10 session\r\n",
                            Subtitle = "Kids Gymnastics - Little Gym\r\n",
                            Description = "Our gymnastics program for preschoolers and kindergarteners has been specially designed to help your child channel all that energy and reach developmental milestones. Independent enough to attend classes without parents, children in this age group still learn best in a structured environment where gymnastics activities are combined with a healthy dose of fun.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Funny Bugs: 3 - 4yo\r\nSat 10am-11am\r\n\r\nGiggle Worms: 4-5yo\r\nSat 1pm-2pm\r\n\r\nGood friends: 5-6yo\r\nSat 2pm-3pm\r\n\r\nMini Jets: 4-6yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "West Gate Alabang",
                            Guarantee = "\"Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 5,
                            ActivityTypeId = 5,
                            ExperienceTypeId = 1,
                            Title = "Gymboree - kids fun play\r\n",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 6000 / person / 10 session\r\n",
                            Subtitle = "Gymboree - kids fun play\r\n",
                            Description = "\"Play and Music classes are uniquely and intentionally designed for early childhood development. Through play, your child is challenged physically, socially and cognitively, building crucial skills to support a lifetime of learning.\r\n\r\nOur expert play leaders support you through 45 minutes of uninterrupted time to play, explore, and learn. Our custom designed curriculum challenges your child as they grow, with changing playscapes and themes every three weeks.\r\n\r\n\"\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Family Music: 6mo - 5yo\r\nSat 10am-10:45am\r\n\r\nOpen Gym: 6mo - 5yo\r\nSat 1pm-1:45pm\r\n\r\nArt 1: 18mo - 24mo\r\nSat 2pm-2:45pm\r\n\r\nFamily Art: 6mo - 5yo\r\nSat 3pm-3:45pm\r\n\r\nFamily Play: 6mo - 5yo\r\nSat 4pm-4:45pm\r\n",
                            MapDetails = "West Gate Alabang",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 6,
                            ActivityTypeId = 6,
                            ExperienceTypeId = 1,
                            Title = "Horse Riding Lessons\r\n",
                            Location = "Cavite",
                            Price = "PHP 11,200 / person / 4 session",
                            Subtitle = "Personalized Horse Riding Lessons",
                            Description = "Riding Lessons\r\nLearn how to ride a horse the english way with any of our interationally-accredited instructors.\r\n\r\n\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "4 Sessions\r\nPHP 11,200.00\r\nMust be consumed within three (3) months\r\n\r\n8 Sessions\r\nPHP 22,400.00\r\nMust be consumed within three (3) months\r\n\r\n12 Sessions\r\nPHP 33,600.00\r\nMust be consumed within three (3) months\r\n",
                            MapDetails = "Rancho Leonor",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 7,
                            ActivityTypeId = 7,
                            ExperienceTypeId = 1,
                            Title = "Filipino Language Tutor for kids",
                            Location = "Metro Manila",
                            Price = "PHP 500 / person / session",
                            Subtitle = "Filipino Language Tutor for kids",
                            Description = "Private Filipino Tutor in Manila and Online\r\nI am Andy, language trainer from HEAC Skills Training. I am teaching English and IELTS from zero to advanced learners.\r\n\r\nSchedule:\r\n\r\nMonday - 4pm-5pm\r\n\r\nWednesday - 4pm-5pm\r\n\r\nFriday - 4pm-5pm\r\n\r\nSaturday - 9am-11am\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "5 session\r\n\r\n10 session\r\n",
                            MapDetails = "Manila Metro Manila",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 8,
                            ActivityTypeId = 8,
                            ExperienceTypeId = 1,
                            Title = "Basketball Coaching",
                            Location = "Bicutan, Paranaque",
                            Price = "PHP 600 / person / session",
                            Subtitle = "Basketball lessons with a coach",
                            Description = "Basketball coach / private trainer with 9+ years experience working with players of all ages and skill levels\r\n\r\nSchedule:\r\n\r\nMonday - 4pm-5pm\r\n\r\nWednesday - 4pm-5pm\r\n\r\nFriday - 4pm-5pm\r\n\r\nSaturday - 9am-11am\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "5 session\r\n\r\n10 session\r\n",
                            MapDetails = "Manila Metro Manila",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 9,
                            ActivityTypeId = 9,
                            ExperienceTypeId = 1,
                            Title = "Biking group ride for kids",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 600 / person / session",
                            Subtitle = "Biking group ride for kids",
                            Description = "Introduces kids ages 2-10 to the fun and thrill of bike riding on a obstacle course. Bicycle safety is a key component. Balance bicycle, proper bike sizing, and a helmet are provided to each child before he or she enters the course. Parents are encouraged to cheer on their young riders, run alongside, or help them scoot along– whatever is best for your kid!\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Saturday 4pm-5pm\r\n",
                            MapDetails = "West Gate Alabang",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 10,
                            ActivityTypeId = 10,
                            ExperienceTypeId = 1,
                            Title = "Golf Lessons for kids",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 5000 /  person / 10 sessions",
                            Subtitle = "Golf Lessons for kids",
                            Description = "Golf — take a lesson with a pro!\r\n\r\nI can provide all the necessary equipment for men, seniors, women and kids of all ages! Your experience will be a one hour golf lesson at the San Bruno Golf Center where Mr Peters golf Camp is located. We have a double decker range, 3 giant practice greens for putting and the all important short game shots. We can hook you up to my computerized Bluetooth swing analyzer and take video of your swings to give you a better understanding of your swing and ways you can improve and become more consistent. This way you will surely have the very best San Francisco golf experience!\r\n\r\nOther things to note\r\nWeather could be an issue. worst case scenario we can hang out at the covered driving range or at the inside putting studio\r\n\r\nSchedule:\r\n\r\nMonday - 4pm-5pm\r\n\r\nWednesday - 4pm-5pm\r\n\r\nFriday - 4pm-5pm\r\n\r\nSaturday - 9am-11am\r\n",
                            ScheduleIndicator = "info",
                            Schedules = "Session schedule\r\n\r\nMonday - 4pm-5pm\r\n\r\nWednesday - 4pm-5pm\r\n\r\nFriday - 4pm-5pm\r\n\r\nSaturday - 9am-11am\r\n",
                            MapDetails = "Southpoint Driving Range",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 11,
                            ActivityTypeId = 11,
                            ExperienceTypeId = 1,
                            Title = "Swimming Lessons",
                            Location = "Ortigas",
                            Price = "PHP 4000 /  person / 6 sessions",
                            Subtitle = "Swimming Lessons",
                            Description = "Learning how to swim should be an enjoyable experience.\r\n\r\nWe come from the belief that learning how to swim shouldn’t come from a place of fear and that such skill could be learned even by the littlest of limbs or by an adult.\r\n\r\nSwimming is a skill that ANYONE of ANY AGE can learn and master.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Funny Bugs: 3 - 4yo\r\nSat 10am-11am\r\n\r\nGiggle Worms: 4-5yo\r\nSat 1pm-2pm\r\n\r\nGood friends: 5-6yo\r\nSat 2pm-3pm\r\n\r\nMini Jets: 4-6yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Palms Alabang",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 12,
                            ActivityTypeId = 12,
                            ExperienceTypeId = 1,
                            Title = "Tennis Lessons",
                            Location = "Alabang, Muntinlupa",
                            Price = "PHP 7500 / person / 10 session",
                            Subtitle = "Tennis Lessons",
                            Description = "The main objective of lessons here at Play!Tennis would be to introduce tennis to your child, sparking an interest in not only tennis, but exercise as well. We aim to provide your child with the necessary understanding and experience in the sport, allowing them to enjoy rallies and match play. Additionally, a good sense of sportsmanship, etiquette, courtesy and empathy would be developed throughout their lessons. Tennis is a great way to improve your child’s development in their early years. Lessons held by our experienced coaches are fun, engaging and progressive, allowing your little one to enjoy their time spent on the court!",
                            ScheduleIndicator = "selection",
                            Schedules = "Funny Bugs: 3 - 4yo\r\nSat 10am-11am\r\n\r\nGiggle Worms: 4-5yo\r\nSat 1pm-2pm\r\n\r\nGood friends: 5-6yo\r\nSat 2pm-3pm\r\n\r\nMini Jets: 4-6yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Palms Alabang",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 13,
                            ActivityTypeId = 13,
                            ExperienceTypeId = 1,
                            Title = "Taekwondo Lessons",
                            Location = "Quezon City",
                            Price = "PHP 700 / person",
                            Subtitle = "Taekwondo Lessons",
                            Description = "𝗠𝗼𝗹𝗱𝗶𝗻𝗴 𝘁𝗵𝗲 𝗻𝗲𝘅𝘁 𝗚𝗲𝗻𝗲𝗿𝗮𝘁𝗶𝗼𝗻𝘀 𝗼𝗳 𝗧𝗮𝗲𝗸𝘄𝗼𝗻𝗱𝗼 𝗮𝗻𝗱 𝗟𝗜𝗙𝗘 𝗖𝗵𝗮𝗺𝗽𝗶𝗼𝗻𝘀‼️🐂\U0001f94b\r\nStart your child's martial art journey with Taekwondo.\r\n🚨From Age 4 and Above‼️\r\nService Offered:\r\n📌Face to Face Group Training\r\n📌 Face to Face ONE on ONE Private Home or Gym Training.\r\n📌Online 1on1 Training via Zoom\r\n📌Online Group Training via Zoom\r\n✅All Coaches are Fully Vaccinated.\r\n✅Clean and sanitized area.\r\n✅ We Sanitize and Provide Complete Training Equipment.\r\n✅ We observe IATF Health Protocols for the Safety of our Students.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Group Training - Face to Face\r\nSat 10am-11am\r\n\r\nOne on one - Face to Face\r\nSat 1pm-2pm\r\n\r\nOnline one on one training\r\nSat 2pm-3pm\r\n\r\nOnling Group Training\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Quezon City",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 14,
                            ActivityTypeId = 14,
                            ExperienceTypeId = 1,
                            Title = "Theatre Acting Lessons",
                            Location = "Manila",
                            Price = "PHP 5000 / person / 15 sessions",
                            Subtitle = "Theatre Acting Lessons",
                            Description = "Give your little one a head start by enrolling themin our Children's Theater class. Beyond the joy ofdiscovering  music  with  others,  children  alsobenefit  from  key  developmental  skills,  such  aslanguage development and listening skills. Otherthan  providing  a  fun  and  interactive  way  forchildren to get moving, our classes also help buildtheir  cognitive  thinking,  physical  and  emotionaldevelopment. There  will  be  an  online  recital  at  the  end  of  theworkshop\r\n\r\nChildrens Theatre\r\n15 sessions \r\n1hr 3x per week\r\n4-6yo\r\n5kPHP\r\n",
                            ScheduleIndicator = "info",
                            Schedules = "Session schedule\r\n\r\nMonday - 4pm-5pm\r\n\r\nWednesday - 4pm-5pm\r\n\r\nFriday - 4pm-5pm\r\n\r\nSaturday - 9am-11am\r\n",
                            MapDetails = "Trumpet playshop",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 15,
                            ActivityTypeId = 15,
                            ExperienceTypeId = 2,
                            Title = "Art box art classes",
                            Location = "Online",
                            Price = "PHP 4500 / person / 8 sessions",
                            Subtitle = "Art box art classes",
                            Description = "A person’s ability to draw and represent what in his mind is a remarkable gift. As a child dealing with the paper as a physical substance useful for making marks, indicate that he is not only making markings but they point beyond themselves to a mental reality, a domain of the imagination” (Golomb, 2011). Art creation denotes a mental process that is indicative of a power to convey meaning (p. 20).\r\nArt then becomes the intersection of the abstract and concrete and resonates mental  faculty that is being utilized as a child engages in art and creating art. As you allow your children to be in art classes, they are not just gaining creative skills, their mind and thinking skills are also being utilized and practiced.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Color art: 3 - 4yo\r\nSat 10am-11am\r\n\r\nPastel Colors: 4-5yo\r\nSat 1pm-2pm\r\n\r\nWater and Pastel: 5-6yo\r\nSat 2pm-3pm\r\n\r\nPainting on canvass: 4-6yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 16,
                            ActivityTypeId = 16,
                            ExperienceTypeId = 2,
                            Title = "Online Ballet Lessons",
                            Location = "Online",
                            Price = "PHP 500 / person / session",
                            Subtitle = "Online Ballet Lessons",
                            Description = "This ballet program is especially designed to develop and expand each child’s individual creativity and artistic abilities. Ballet Day Care Class teaches 1 to 5 year olds through creative imagery and role-play rather than the technical approach traditionally associated with ballet. It develops an appreciation of rhythm and timing, music, movement and grace during the most receptive time in their lives. This program also develops gross motor skills, stimulates social interaction and ability to follow instruction and, gain confidence to take that first step on stage!\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "5 session - Sunday 2:30pm - 3:30pm\r\n\r\n10 session - Sunday 2:30pm - 3:30pm\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 17,
                            ActivityTypeId = 17,
                            ExperienceTypeId = 2,
                            Title = "Coding class for kids",
                            Location = "Online",
                            Price = "PHP 5000 / person / 10 session",
                            Subtitle = "Coding class for kids",
                            Description = "Coding 101\r\n\r\nGet creative and learn to program! Coding 101 introduces programming fundamentals with Scratch, a drag-and-drop learning environment that makes use of user-friendly block codes.\r\n\r\nAge:        6-12\r\nDuration:        10 Hours\r\nSkill Level:        Beginner\r\nPath:        Coding Fundamentals\r\nTools:        Scratch\r\n",
                            ScheduleIndicator = "info",
                            Schedules = "Session schedule\r\n\r\nMonday - 4pm-5pm\r\n\r\nWednesday - 4pm-5pm\r\n\r\nFriday - 4pm-5pm\r\n\r\nSaturday - 9am-11am\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 18,
                            ActivityTypeId = 18,
                            ExperienceTypeId = 2,
                            Title = "Chess Lessons kids",
                            Location = "Online",
                            Price = "PHP 5000 / person / 10 session",
                            Subtitle = "Chess Lessons",
                            Description = "Aldus Brant B. Austria - Professional Chess Player\r\n\r\nI'm a professional chess player with 16 years of experience of playing chess. I coach students in our local area and represent our institution in various competition. I won numerous awards and I'm fond of sharing my techniques and experiences to students. From chess openings, gambits, and chess positions. I'm also knowledgeable in using different chess applications that will enhance the capability of the students I teach, I can provide sample demo for the first few hours for free. I hope you'll consider hiring me. Let's learn together! I am an active, joyful and enthusiastic person.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Funny Bugs: 3 - 4yo\r\nSat 10am-11am\r\n\r\nGiggle Worms: 4-5yo\r\nSat 1pm-2pm\r\n\r\nGood friends: 5-6yo\r\nSat 2pm-3pm\r\n\r\nMini Jets: 4-6yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 19,
                            ActivityTypeId = 19,
                            ExperienceTypeId = 2,
                            Title = "Yoga classes for kids",
                            Location = "Online",
                            Price = "PHP 2500 / person / 4 sessions",
                            Subtitle = "Yoga classes for kids",
                            Description = "We make yoga and mindfulness\r\nFUN for kids!\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Funny Bugs: 3 - 4yo\r\nSat 10am-11am\r\n\r\nGiggle Worms: 4-5yo\r\nSat 1pm-2pm\r\n\r\nGood friends: 5-6yo\r\nSat 2pm-3pm\r\n\r\nMini Jets: 4-6yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 20,
                            ActivityTypeId = 20,
                            ExperienceTypeId = 2,
                            Title = "Guitar Lessons for kids",
                            Location = "Online",
                            Price = "PHP 5000 / person / 10 session",
                            Subtitle = "Guitar Lessons for kids",
                            Description = "World Class musician give online guitar lessons by Skype or Zoom. 100% refound if you won't like it\r\n\r\nAll kind of music and levels are welcome, from total beginner to advanced players. Everything will be personalized about your goals and needs.\r\nI can teach you songs, technique, theory, improvisation, fingerstyle and percussions, song writing, and many other stuff.\r\n\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Kids: 5-6yo\r\nSat 2pm-3pm\r\n\r\nGrade school: 7-10yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 21,
                            ActivityTypeId = 21,
                            ExperienceTypeId = 2,
                            Title = "Violin Lessons for kids\r\n",
                            Location = "Online",
                            Price = "PHP 6000 / person / 10 session",
                            Subtitle = "Violin Lessons for kids",
                            Description = "Professional violin performer with years teaching experience, and a piano instructor, music theory tutor\r\n\r\nAccording to different level of the students, I will use various methods to teach. I am very careful with students’ posture in the beginning because I don’t want them to get I hired in the future by playing the violin or piano.\r\nAlso, I would love my students to note their practice time, and send me some practice video, so I am able to keep track of them! I will also send some videos to the parents to let them know that your kids are doing well!\r\n\r\nIn the other hand, I like to make students to think about what the problem is first, and try their best to solve the problem by themselves instead of telling them everything.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Kids: 5-6yo\r\nSat 2pm-3pm\r\n\r\nGrade school: 7-10yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Restaurant - Las Flores in Festival",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                        await CoreDI.DataStore.Activities.SaveDataAsync(new ActivityModel
                        {
                            Id = 22,
                            ActivityTypeId = 22,
                            ExperienceTypeId = 2,
                            Title = "Piano Lessons for kids",
                            Location = "Online",
                            Price = "PHP 6000 / person / 10 session",
                            Subtitle = "Piano Lessons for kids",
                            Description = "Online piano lessons! A creative and personal approach to music. From a pianist and songwriter.\r\n\r\nI love music and I love teaching music. I believe that learning any kind of subject should be something inspiring, joyful and challenging. That is my goal when teaching.\r\n\r\nI prepare the lessons having 2 main concepts:\r\n1) A holistic view of music when learning: Learning to play an instrument is very rewarding, but I discovered that if you learn how to deepen your ability to listen to music, why does it work , how to make music by yourself and concepts like these, the process feels extremely more rewarding and interesting.\r\n2) Every student is unique: Each one has their own pace, their own interests, their own expectations and taste. A personalized lesson is extremely important, because it helps you to stay motivated and is more fulfilling if you accomplish what you desire.\r\n",
                            ScheduleIndicator = "selection",
                            Schedules = "Kids: 5-6yo\r\nSat 2pm-3pm\r\n\r\nGrade school: 7-10yo\r\nSat 3pm-4pm\r\n",
                            MapDetails = "Online",
                            Guarantee = "Write as caption - \r\n\r\n\r\n\"\"Not happy with the experience? Contact us and we'll solve the problem for you.\"\"\"\r\n",
                            Remarks = ""
                        });
                    }
                }
            }

            //Create Activity Image
            var activityImages = await CoreDI.DataStore.ActivityImages.GetAllAsync();
            if (activityImages.Count != 0)
            {
                var result = activityImages.Where(p=> p.Id==1).ToList();
                foreach (var item in result)
                {
                    if (item.ImageLocation != "images/Activities/Football1.jpg")
                    {
                        images = await CoreDI.DataStore.ActivityImages.GetAllAsync();
                        foreach (var items in images)
                        {
                            await CoreDI.DataStore.ActivityImages.DeleteDataAsync(items);
                        }
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 1,
                            ImageId = 1,
                            ImageLocation = "images/Activities/Football1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 2,
                            ImageId = 1,
                            ImageLocation = "images/Activities/Football2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 3,
                            ImageId = 1,
                            ImageLocation = "images/Activities/Football3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 4,
                            ImageId = 2,
                            ImageLocation = "images/Activities/Baking1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 5,
                            ImageId = 2,
                            ImageLocation = "images/Activities/Baking2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 6,
                            ImageId = 2,
                            ImageLocation = "images/Activities/Baking3.jpg"
                        });

                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 7,
                            ImageId = 3,
                            ImageLocation = "images/Activities/Ballet1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 8,
                            ImageId = 3,
                            ImageLocation = "images/Activities/Ballet2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 9,
                            ImageId = 3,
                            ImageLocation = "images/Activities/Ballet3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 10,
                            ImageId = 4,
                            ImageLocation = "images/Activities/Gymnastics1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 11,
                            ImageId = 4,
                            ImageLocation = "images/Activities/Gymnastics2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 12,
                            ImageId = 4,
                            ImageLocation = "images/Activities/Gymnastics3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 13,
                            ImageId = 5,
                            ImageLocation = "images/Activities/Play1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 14,
                            ImageId = 5,
                            ImageLocation = "images/Activities/Play2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 15,
                            ImageId = 5,
                            ImageLocation = "images/Activities/Play3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 16,
                            ImageId = 6,
                            ImageLocation = "images/Activities/Horseback1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 17,
                            ImageId = 6,
                            ImageLocation = "images/Activities/Horseback2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 18,
                            ImageId = 6,
                            ImageLocation = "images/Activities/Horseback3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 19,
                            ImageId = 7,
                            ImageLocation = "images/Activities/Language1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 20,
                            ImageId = 7,
                            ImageLocation = "images/Activities/Language2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 21,
                            ImageId = 7,
                            ImageLocation = "images/Activities/Language3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 22,
                            ImageId = 8,
                            ImageLocation = "images/Activities/Basketball1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 23,
                            ImageId = 8,
                            ImageLocation = "images/Activities/Basketball2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 24,
                            ImageId = 8,
                            ImageLocation = "images/Activities/Basketball3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 25,
                            ImageId = 9,
                            ImageLocation = "images/Activities/Biking1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 26,
                            ImageId = 9,
                            ImageLocation = "images/Activities/Biking2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 27,
                            ImageId = 9,
                            ImageLocation = "images/Activities/Biking3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 28,
                            ImageId = 10,
                            ImageLocation = "images/Activities/Golf1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 29,
                            ImageId = 10,
                            ImageLocation = "images/Activities/Golf2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 30,
                            ImageId = 10,
                            ImageLocation = "images/Activities/Golf3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 31,
                            ImageId = 11,
                            ImageLocation = "images/Activities/Swimming1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 32,
                            ImageId = 11,
                            ImageLocation = "images/Activities/Swimming2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 33,
                            ImageId = 11,
                            ImageLocation = "images/Activities/Swimming3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 34,
                            ImageId = 12,
                            ImageLocation = "images/Activities/Tennis1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 35,
                            ImageId = 12,
                            ImageLocation = "images/Activities/Tennis2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 36,
                            ImageId = 12,
                            ImageLocation = "images/Activities/Tennis3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 37,
                            ImageId = 13,
                            ImageLocation = "images/Activities/Taekwondo1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 38,
                            ImageId = 13,
                            ImageLocation = "images/Activities/Taekwondo2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 39,
                            ImageId = 13,
                            ImageLocation = "images/Activities/Taekwondo3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 40,
                            ImageId = 14,
                            ImageLocation = "images/Activities/Theater1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 41,
                            ImageId = 14,
                            ImageLocation = "images/Activities/Theater2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 42,
                            ImageId = 14,
                            ImageLocation = "images/Activities/Theater3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 43,
                            ImageId = 15,
                            ImageLocation = "images/Activities/Art1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 44,
                            ImageId = 15,
                            ImageLocation = "images/Activities/Art2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 45,
                            ImageId = 15,
                            ImageLocation = "images/Activities/Art3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 46,
                            ImageId = 16,
                            ImageLocation = "images/Activities/Online-Ballet1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 47,
                            ImageId = 16,
                            ImageLocation = "images/Activities/Online-Ballet2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 48,
                            ImageId = 16,
                            ImageLocation = "images/Activities/Online-Ballet3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 49,
                            ImageId = 17,
                            ImageLocation = "images/Activities/Coding1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 50,
                            ImageId = 17,
                            ImageLocation = "images/Activities/Coding2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 51,
                            ImageId = 17,
                            ImageLocation = "images/Activities/Coding3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 52,
                            ImageId = 18,
                            ImageLocation = "images/Activities/Chess1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 53,
                            ImageId = 18,
                            ImageLocation = "images/Activities/Chess2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 54,
                            ImageId = 18,
                            ImageLocation = "images/Activities/Chess3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 55,
                            ImageId = 19,
                            ImageLocation = "images/Activities/Yoga1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 56,
                            ImageId = 19,
                            ImageLocation = "images/Activities/Yoga2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 57,
                            ImageId = 19,
                            ImageLocation = "images/Activities/Yoga3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 58,
                            ImageId = 20,
                            ImageLocation = "images/Activities/Guitar1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 59,
                            ImageId = 20,
                            ImageLocation = "images/Activities/Guitar2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 60,
                            ImageId = 20,
                            ImageLocation = "images/Activities/Guitar3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 61,
                            ImageId = 21,
                            ImageLocation = "images/Activities/Violin1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 62,
                            ImageId = 21,
                            ImageLocation = "images/Activities/Violin2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 63,
                            ImageId = 21,
                            ImageLocation = "images/Activities/Violin3.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 64,
                            ImageId = 22,
                            ImageLocation = "images/Activities/Piano1.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 65,
                            ImageId = 22,
                            ImageLocation = "images/Activities/Piano2.jpg"
                        });
                        await CoreDI.DataStore.ActivityImages.SaveDataAsync(new ActivityImagesModels
                        {
                            Id = 66,
                            ImageId = 22,
                            ImageLocation = "images/Activities/Piano3.jpg"
                        });
                    }
                }
            }
        }  

    }
}

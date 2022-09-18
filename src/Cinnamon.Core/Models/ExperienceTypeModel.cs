namespace Cinnamon.Core
{
    /// <summary>
    /// Experience Type Model
    /// e.g In-Person, Online
    /// </summary>
    public class ExperienceTypeModel : BaseModel
    {
        /// <summary>
        /// Id - Unique Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = "";

        public virtual ICollection<ActivityModel> Activities { get; set; }
    }
}

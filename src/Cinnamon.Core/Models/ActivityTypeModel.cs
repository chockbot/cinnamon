namespace Cinnamon.Core
{
    /// <summary>
    /// Activity Type Model
    /// e.g Baking, Ballet, Fun Play
    /// </summary>
    public class ActivityTypeModel : BaseModel
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

namespace Cinnamon.Core
{
    public class BaseModel
    {
        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Change Date
        /// </summary>
        public DateTime ChangedOn { get; set; }
        /// <summary>
        /// Created By User Id
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Changed By User Id
        /// </summary>
        public int ChangedBy { get; set; }

        /// <summary>
        /// Deletion Flag
        /// </summary>
        public bool DeletionFlag { get; set; }

        /// <summary>
        /// Clear Virtual Properties
        /// </summary>
        public virtual void LoadVirtualProperties() { }
    }
}

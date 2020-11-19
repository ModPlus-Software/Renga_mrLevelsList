namespace mrLevelsList.Models
{
    using ModPlusAPI;
    using ModPlusAPI.Mvvm;
    using Renga;

    /// <summary>
    /// Level wrapper
    /// </summary>
    public class LevelWrapper : VmBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelWrapper"/> class.
        /// </summary>
        /// <param name="levelObject">Instance of <see cref="IModelObject"/></param>
        public LevelWrapper(IModelObject levelObject)
        {
            if (levelObject == null)
            {
                Name = Language.GetItem("all");
            }
            else
            {
                Name = levelObject.Name;
                Id = levelObject.Id;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Object id
        /// </summary>
        public int Id { get; }
    }
}

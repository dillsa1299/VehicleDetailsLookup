namespace VehicleDetailsLookup.Shared.Models.Enums
{
    /// <summary>
    /// Represents the types of defects that can be identified during an MOT test.
    /// </summary>
    public enum MotDefectType
    {
        /// <summary>
        /// A defect that is not serious but should be monitored or repaired in the future.
        /// </summary>
        Advisory,
        /// <summary>
        /// A defect that poses an immediate risk to road safety or the environment.
        /// </summary>
        Dangerous,
        /// <summary>
        /// A defect that results in an MOT test failure.
        /// </summary>
        Fail,
        /// <summary>
        /// A major defect that must be repaired for the vehicle to pass the MOT test.
        /// </summary>
        Major,
        /// <summary>
        /// A minor defect that does not significantly affect safety or the environment.
        /// </summary>
        Minor,
        /// <summary>
        /// A defect that does not fit into a specific category.
        /// </summary>
        NonSpecific,
        /// <summary>
        /// A defect entry generated automatically by the system.
        /// </summary>
        SystemGenerated,
        /// <summary>
        /// A defect entry manually entered by a user.
        /// </summary>
        UserEntered
    }
}

using VehicleDetailsLookup.Models.Database.Details;

namespace VehicleDetailsLookup.Models.Database.Lookup
{
    /// <summary>
    /// Represents the structure for storing lookup information related to vehicle details in the database.
    /// </summary>
    public interface ILookupDbModel
    {
        /// <summary>
        /// The date and time when the lookup record was created or last updated.
        /// </summary>
        DateTime DateTime { get; set; }
        /// <summary>
        /// The registration number of the vehicle being looked up.
        /// </summary>
        string RegistrationNumber { get; set; }
        /// <summary>
        /// Navigation property for related vehicle details.
        /// </summary>
        DetailsDbModel? Details { get; set; }
    }
}
namespace VehicleDetailsLookup.Models.ApiResponses.Mot
{
    /// <summary>
    /// Response data for a vehicle's MOT from the government MOT API.
    /// </summary>
    public interface IMotResponse
    {
        /// <summary>
        /// Date the vehicle was first used.
        /// </summary>
        string FirstUsedDate { get; set; }

        /// <summary>
        /// Type of fuel the vehicle uses.
        /// </summary>
        string FuelType { get; set; }

        /// <summary>
        /// Indicates whether the vehicle has any outstanding recalls.
        /// </summary>
        string HasOutstandingRecall { get; set; }

        /// <summary>
        /// Vehicle make.
        /// </summary>
        string Make { get; set; }

        /// <summary>
        /// Vehicle manufacture date.
        /// </summary>
        string ManufactureDate { get; set; }

        /// <summary>
        /// Vehicle model.
        /// </summary>
        string Model { get; set; }

        /// <summary>
        /// Collection of MOT tests for the vehicle.
        /// </summary>
        IEnumerable<IMotResponseTest> MotTests { get; set; }

        /// <summary>
        /// Primary colour of the vehicle.
        /// </summary>
        string PrimaryColour { get; set; }

        /// <summary>
        /// Vehicle registration number.
        /// </summary>
        string Registration { get; set; }

        /// <summary>
        /// Vehicle registration date.
        /// </summary>
        string RegistrationDate { get; set; }
    }
}
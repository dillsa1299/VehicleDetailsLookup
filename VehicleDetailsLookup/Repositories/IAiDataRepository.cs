﻿using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Shared.Models.Enums;
using System.Threading.Tasks;

namespace VehicleDetailsLookup.Repositories
{
    /// <summary>
    /// Defines methods for storing and retrieving AI-generated vehicle data in the database.
    /// </summary>
    public interface IAiDataRepository
    {
        /// <summary>
        /// Adds a new AI data record or updates an existing one for a vehicle in the database.
        /// </summary>
        /// <param name="aiData">The AI data model containing vehicle information to add or update.</param>
        Task UpdateAiDataAsync(IAiDataDbModel aiData);

        /// <summary>
        /// Retrieves AI-generated data for a specific vehicle registration number and AI data type.
        /// </summary>
        /// <param name="registrationNumber">The registration number of the vehicle.</param>
        /// <param name="type">The type of AI-generated data to retrieve.</param>
        /// <returns>
        /// The AI data model for the specified vehicle and type, or <c>null</c> if no data is found.
        /// </returns>
        ValueTask<IAiDataDbModel?> GetAiDataAsync(string registrationNumber, AiType type);
    }
}

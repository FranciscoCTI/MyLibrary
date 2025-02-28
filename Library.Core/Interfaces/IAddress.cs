namespace Library.Core.Interfaces;

/// <summary>
/// Represents a city address definition
/// </summary>
public interface IAddress
{
    /// <summary>
    /// The street where the property is located
    /// </summary>
    string Street { get; set; }

    /// <summary>
    /// The number of the property on the street
    /// </summary>
    int HouseNumber { get; set; }

    /// <summary>
    /// The city where the property is located
    /// </summary>
    string City { get; set; }

    /// <summary>
    /// The nation-state where the property is located
    /// </summary>
    string Country { get; set; }
}
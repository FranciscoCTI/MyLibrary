using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Bson;
using Library.Core.Models;

namespace Library.Core.Interfaces;

/// <summary>
/// Store the information of all the <see cref="IAuthor"/>s involved in the
/// creation of the content
/// </summary>
public interface IAuthorInformation:INotifyPropertyChanged
{
    /// <summary>
    /// The identification for use in MongoDB
    /// </summary>
    public string MongoId { get; set; }

    /// <summary>
    /// A list with all the <see cref="IAuthor"/>s that were involved in the creation of the content
    /// </summary>
    List<IAuthor> Authors { get; set; }

    /// <summary>
    /// Add a new <see cref="IAuthor"/> to the list of authors
    /// </summary>
    /// <param name="author">The <see cref="IAuthor"/> to be added to the list</param>
    void AddNewAuthor(IAuthor author);

    /// <summary>
    /// Checks if this <see cref="IAuthorInformation"/> has at least one
    /// <see cref="IAuthor"/>
    /// </summary>
    /// <returns></returns>
    bool AnyAuthor();
}
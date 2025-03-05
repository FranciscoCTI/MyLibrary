using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Bson;
using Library.Core.Models;

namespace Library.Core.Interfaces;

public interface IAuthorInformation:INotifyPropertyChanged
{
    public string Id { get; set; }
    [BsonElement("Authors")] 
    List<IAuthor> Authors { get; set; }

    void AddNewAuthor(IAuthor author);
    bool Any();
    void InputAuthors(IEnumerable<IAuthor>? contentAuthors);
}
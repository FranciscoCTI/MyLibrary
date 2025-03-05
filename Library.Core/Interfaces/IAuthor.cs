
namespace Library.Core.Interfaces
{
    /// <summary>
    /// A human person that participated in the creation of a <see cref="IBook"/> 
    /// </summary>
    public interface IAuthor:IUser
    {
        /// <summary>
        /// A special id to categorize the <see cref="IAuthor"/>
        /// </summary>
        string AuthorId { get; set; }

        /// <summary>
        /// Check if the Author have all the necessary data
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }
}

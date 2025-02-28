using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Library.Core.Interfaces
{
    /// <summary>
    /// Represents an instance of a physical paper book
    /// </summary>
    public interface IBook:INotifyPropertyChanged
    {
        /// <summary>
        /// The international code for this <see cref="IBook"/> (International Standard Book
        /// Number).
        /// A 13-digit unique number that identify every physical or digital <see cref="IBook"/>.
        /// </summary>
        long ISBN { get; set; }

        /// <summary>
        /// The Title of the <see cref="IBook"/>
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// A short description of the <see cref="IBook"/>. It could be the paragraph in the back cover.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The creator of the <see cref="IBook"/>.
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// Automatically creates a new ISBN, this is not a real ISBN (since it´s not calculated with the real algorithm), only a placeholder value.
        /// </summary>
        void CreateIsbnByDefault();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Library.Core.Interfaces;

namespace Library.Core.Models
{
    /// <summary>
    /// A human person that uses the <see cref="ILibrary"/> 
    /// </summary>
    public class User: IUser 
    {
        /// <inheritdoc />
        public string FirstName { get; set; }

        /// <inheritdoc />
        public string LastName { get; set; }

        /// <inheritdoc/>
        public string CompleteName
        {
            get => FirstName + " " + LastName;
            set
            {
                SetFirstName();
                SetLastName();
            }
        }

        private void SetLastName()
        {
            var divided = CompleteName.Split(" ");
            LastName = divided.Last();
        }

        private void SetFirstName()
        {
            var divided = CompleteName.Split(" ");
            FirstName = divided.First();
        }

        /// <inheritdoc />
        public int Age { get; set; }

        /// <inheritdoc />
        public IAddress Address { get; set; }

        /// <summary>
        /// The constructor for <see cref="IUser"/>
        /// </summary>
        /// <param name="name">The first name of this user</param>
        public User(string name)
        {
            this.FirstName = name;
            this.LastName = "-";
            this.Age = 0;
            this.Address = new Address();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

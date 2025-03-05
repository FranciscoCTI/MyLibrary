using System;
using System.Collections.Generic;
using System.Linq;
using Library.Core.Interfaces;
using Library.UI.Commands;
using System.ComponentModel;
using Library.UI.Views;
using System.Runtime.CompilerServices;
using Library.Core.Models;
using System.Collections.ObjectModel;

namespace Library.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the BookWindow UI. Which shows the properties of the current
    /// <see cref="IBook"/> to be added or modified.
    /// </summary>
    public class BookWindowViewModel:INotifyPropertyChanged
    {

        private IBook _book;

        /// <summary>
        /// The current <see cref="IBook"/>
        /// </summary>
        public IBook Book
        {
            get
            {
               return _book;
            }
            set
            {
                _book = (Book)value;
                OnPropertyChanged(nameof(Book));
                OnPropertyChanged(nameof(AuthorsCollection));
            }
        }

        /// <summary>
        /// Command to proceed with the add <see cref="IBook"/> process.
        /// </summary>
        public RelayCommand ProceedCommand => new(execute => ProceedAddingBook());

        /// <summary>
        /// Command to cancel the operation
        /// </summary>
        public RelayCommand CancelCommand => new(execute => CancelAddingBook());

        /// <summary>
        /// Command to add a new Author
        /// </summary>
        public RelayCommand AddAuthorCommand => new(execute => AddAuthor());

        /// <summary>
        /// The current UI which this ViewModel controls
        /// </summary>
        public BookWindow BookWindow;

        /// <summary>
        /// Return the list of authors of the book
        /// </summary>
        public ObservableCollection<IAuthor> AuthorsCollection
        {
            get
            {
                return new ObservableCollection<IAuthor>(Book.Authors.Authors);
            }
            set
            {
                OnPropertyChanged(nameof(AuthorsCollection));
            }
        }

        /// <summary>
        /// Constructor for the <see cref="BookWindowViewModel"/> class
        /// </summary>
        public BookWindowViewModel()
        {
            Book = new Book("Empty");
        }

        /// <summary>
        /// Adds to the <see cref="ILibrary"/> a <see cref="IBook"/>  with the values on
        /// this form.
        /// </summary>
        private void ProceedAddingBook()
        {
            if (Book != null)
            {
                Book.ISBN = long.Parse(BookWindow.TbxIsbn.Text);
                Book.Name = BookWindow.TbxTitle.Text;
                Book.Authors = GetAuthorInformation(BookWindow);
                Book.Description = BookWindow.TbxDescription.Text;
            }
            BookWindow.Close();
        }

        private IAuthorInformation GetAuthorInformation(BookWindow bookWindow)
        {
            IAuthorInformation authorInformation = new AuthorInformation();
            var contentAuthors = bookWindow.ItemsCtrlAuthors.ItemsSource as IEnumerable<IAuthor>;

            authorInformation.InputAuthors(contentAuthors);

            return authorInformation;
        }

        /// <summary>
        /// Close the window to cancel the process of adding or editing a <see cref="IBook"/>
        /// </summary>
        private void CancelAddingBook()
        {
            Book = null;
            BookWindow.Close();
        }
        public void AddAuthor()
        {
            Book.Authors.Authors.Add(new Author("Perico","Los palotes"));
            BookWindow.ItemsCtrlAuthors.ItemsSource = AuthorsCollection;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? 
                                                    propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void UpdateBookValues()
        {
            OnPropertyChanged(nameof(Book));
        }
    }
}

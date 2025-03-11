using System.Windows;
using Library.Core.Interfaces;
using Library.UI.ViewModels;
using System.Windows.Controls;
using Author = Library.Core.Models.Author;
using Book = Library.Core.Models.Book;
using System.Windows.Data;
using Library.Core.Enums;
using Library.Core.Models;

namespace Library.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml. The window that allow the user to input
    /// the data to create or modify a new <see cref="IBook"/>.
    /// </summary>
    public partial class LibraryWindow : Window
    {
        private LibraryMainViewModel _viewModel;
        public BookWindow BookWindow;

        /// <summary>
        /// Constructor for <see cref="LibraryWindow"/>
        /// </summary>
        public LibraryWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as LibraryMainViewModel;
            _viewModel.MainWindow = this;

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Update the <see cref="IBook"/>s in the <see cref="DataGrid"/>> when the UI is loaded
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateDgrBooks();
        }

        public void UpdateDgrBooks()
        {
            CollectionViewSource.GetDefaultView(dgrBooks.ItemsSource).Refresh();
        }

        /// <summary>
        /// Creates a new <see cref="Views.BookWindow"/> and define it in the
        /// <see cref="BookWindow"/> field
        /// </summary>
        public void SetBookWindow()
        {
            BookWindow = new BookWindow();
        }

        /// <summary>
        /// Gets the selected item in the <see cref="dgrBooks"/> <see cref="DataGrid"/>
        /// </summary>
        /// <returns></returns>
        public IBook? GetSelectedItem()
        {
            return (IBook)dgrBooks.SelectedItem;
        }

        /// <summary>
        /// Gets the current <see cref="BookWindow"/> in this <see cref="LibraryWindow"/>
        /// </summary>
        /// <returns></returns>
        internal BookWindow GetBookWindow()
        {
            return BookWindow;
        }

        /// <summary>
        /// Filter for the <see cref="CollectionViewSource"/> that shows the library items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cvs_Filter_Name(object sender, FilterEventArgs e)
        {
            IBook book = e.Item as IBook;

            bool nameResult = NameFiltered(book.Name);
            bool isbnResult = IsbnFiltered(book.ISBN);
            bool authorsResults = AuthorsResultFiltered(book.AuthorInformation);
            bool themesResults = ThemesResultFiltered(book.Themes);
            bool descriptionResult = DescriptionFiltered(book.Description);

            if (
                nameResult && isbnResult && authorsResults && themesResults && descriptionResult
                )
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        /// <summary>
        /// Passes only the <see cref="IBook"/>s where the name contains the
        /// <see cref="_viewModel.BookNameFilterString"/>
        /// </summary>
        private bool NameFiltered(string name)
        {
            if (name.Contains(_viewModel.BookNameFilterString, 
                    StringComparison.InvariantCultureIgnoreCase) || 
                _viewModel.BookNameFilterString == "")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Passes only the <see cref="IBook"/>s where the ISBN contains the
        /// <see cref="_viewModel.BookIsbnFilterString"/>
        /// </summary>
        private bool IsbnFiltered(long isbn)
        {
            string isbnToString = isbn.ToString();

            if (isbnToString.Contains(_viewModel.BookIsbnFilterString) 
                || _viewModel.BookIsbnFilterString =="")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Passes only the <see cref="IBook"/>s where any of the authors contains the
        /// <see cref="_viewModel.BookAuthorsFilteringString"/>
        /// </summary>
        private bool AuthorsResultFiltered(IAuthorInformation authorInformation)
        {
            var authors = authorInformation.Authors;

            if (authors.Any(x=>x.CompleteName.Contains(_viewModel.BookAuthorsFilterString, 
                    StringComparison.InvariantCultureIgnoreCase)) 
                ||
                _viewModel.BookAuthorsFilterString == "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Passes only the <see cref="IBook"/>s where one of the Themes contains the
        /// <see cref="_viewModel.BookThemesFilterString"/>
        /// </summary>
        private bool ThemesResultFiltered(List<Theme> themes)
        {
            List<string> themesString = themes.Select(x => x.ToString()).ToList();

            if (themesString.Any(x => x.Contains(_viewModel.BookThemesFilterString, 
                    StringComparison.CurrentCultureIgnoreCase)) 
                ||
                _viewModel.BookThemesFilterString == "")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Passes only the <see cref="IBook"/>s where the description contains the
        /// <see cref="_viewModel.BookDescriptionFilterString"/>
        /// </summary>
        private bool DescriptionFiltered(string description)
        {
            if (description.Contains(_viewModel.BookDescriptionFilterString,
                    StringComparison.InvariantCultureIgnoreCase) ||
                _viewModel.BookDescriptionFilterString == "")
            {
                return true;
            }
            return false;
        }
    }
}
using System.Windows;
using Library.Core.Interfaces;
using Library.UI.ViewModels;
using System.Windows.Controls;
using Author = Library.Core.Models.Author;
using Book = Library.Core.Models.Book;
using System.Windows.Data;

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

            if (book.Name.Contains(_viewModel.BookFilter, StringComparison.InvariantCultureIgnoreCase) 
                || _viewModel.BookFilter == "")
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }
    }
}
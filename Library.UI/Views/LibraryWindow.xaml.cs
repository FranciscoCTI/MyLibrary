using System.Windows;
using Library.Core.Interfaces;
using Library.UI.ViewModels;
using System.Windows.Controls;
using Author = Library.Core.Models.Author;
using Book = Library.Core.Models.Book;

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

        public LibraryWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as LibraryMainViewModel;
            _viewModel.MainWindow = this;
        }

        /// <summary>
        /// Creates a new <see cref="Library.UI.Views.BookWindow"/> and define it in the <see cref="BookWindow"/> field
        /// </summary>
        public void SetBookWindow()
        {
            //BookWindowViewModel bookWindowVM = BookWindow.DataContext as BookWindowViewModel;

            BookWindowViewModel bwvn = new BookWindowViewModel();
            bwvn.Book = new Book("Sample book");

            IAuthor newAuthor = new Author("Pedro", "Paramo");
            bwvn.Book.Authors.AddNewAuthor(newAuthor);
            bwvn.AuthorsCollection.Add(newAuthor);

            BookWindow = new BookWindow(bwvn);
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
    }
}
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
using System.Windows;
using System.Windows.Controls;
using Library.Core.Enums;
using Library.Core.Factories;
using System.Windows.Media;
using Library.UI.UI.Solvers;
using Library.Services;

namespace Library.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the BookWindow UI. Which shows the properties of the current
    /// <see cref="IBook"/> to be added or modified.
    /// </summary>
    public class BookWindowViewModel:INotifyPropertyChanged
    {

        private IBook _book;
        private ExceptionManager ExceptionManager { get; }

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
        /// Command to add a new <see cref="IAuthor"/>
        /// </summary>
        public RelayCommand AddAuthorCommand => new(execute => AddAuthor());

        /// <summary>
        /// Command to remove the last <see cref="IAuthor"/>
        /// </summary>
        public RelayCommand RemoveAuthorCommand => new(execute => RemoveAuthor());

        /// <summary>
        /// The current UI which this ViewModel controls
        /// </summary>
        public BookWindow BookWindow;

        /// <summary>
        /// Return the list of <see cref="IAuthor"/>s of the <see cref="IBook"/>
        /// </summary>
        public ObservableCollection<IAuthor> AuthorsCollection
        {
            get
            {
                return new ObservableCollection<IAuthor>(Book.AuthorInformation.Authors);
            }
            set
            {
                OnPropertyChanged(nameof(AuthorsCollection));
            }
        }

        /// <summary>
        /// Return a list of all <see cref="Theme"/>s available to add to a
        /// <see cref="IBook"/>
        /// </summary>
        public ObservableCollection<Theme> AllAvailableThemes
        {
            get
            {
                return new ObservableCollection<Theme>(Enum.GetValues(typeof(Theme))
                    .Cast<Theme>().ToList());
            }
        }

        /// <summary>
        /// Constructor for the <see cref="BookWindowViewModel"/> class
        /// </summary>
        public BookWindowViewModel()
        {
            LibraryFactory lf = new LibraryFactory();
            Book = lf.CreateBook("Empty");

            ExceptionManager = new ExceptionManager();
        }

        /// <summary>
        /// Adds to the <see cref="ILibrary"/> a <see cref="IBook"/>  with the values on
        /// this form.
        /// </summary>
        private void ProceedAddingBook()
        {
            UpdateThemes();

            BookWindow.DialogResult = true;
            BookWindow.Close();
        }

        /// <summary>
        /// Gets the selected <see cref="Theme"/>s in the UI and add them to the
        /// <see cref="IBook"/>> element
        /// </summary>
        private void UpdateThemes()
        {
            Book.Themes.Clear();

            List<StackPanel> allStackPanels = VisualTreeProcesses
                        .FindVisualChildren<StackPanel>(BookWindow.ItemsCtrlThemes);

            foreach (var panel in allStackPanels)
            {
                CheckBox chbx = VisualTreeProcesses.GetFirstCheckBox(panel);
                Label label = VisualTreeProcesses.GetFirstLabel(panel);

                if (chbx==null || label == null)
                {
                    ExceptionManager.HandleException(
                        new NullReferenceException("UI element not found"), 
                        "BookWindowUI");
                }

                if (chbx!= null && (bool)chbx.IsChecked)
                {
                    if (Enum.TryParse(label.Content.ToString(), out Theme result))
                    {
                        Book.Themes.Add(result);
                    }
                }
            }
        }

        /// <summary>
        /// Show the current <see cref="IBook"/> themes on the UI
        /// </summary>
        private void InputThemes()
        {
            var themeList = Book.Themes.Select(x=>x.ToString());

            List<StackPanel> allStackPanels = VisualTreeProcesses
                .FindVisualChildren<StackPanel>(BookWindow.ItemsCtrlThemes);

            foreach (var panel in allStackPanels)
            {
                CheckBox chbx = VisualTreeProcesses.GetFirstCheckBox(panel);
                Label label = (Label)VisualTreeProcesses.GetFirstLabel(panel);

                if (chbx == null || label == null)
                {
                    ExceptionManager.HandleException(
                        new NullReferenceException("UI element not found"), 
                        "BookWindowUI");
                }

                if (themeList.Contains(label.Content.ToString()))
                {
                    chbx.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Close the window to cancel the process of adding or editing a
        /// <see cref="IBook"/>
        /// </summary>
        private void CancelAddingBook()
        {
            BookWindow.DialogResult = false;
            BookWindow.Close();
        }

        /// <summary>
        /// Adds a new mock <see cref="IAuthor"/> to
        /// <see cref="AuthorInformation.Authors"/>
        /// and to the <see cref="IAuthor"/>s <see cref="ItemsControl"/>
        /// </summary>
        public void AddAuthor()
        {
            Book.AuthorInformation.Authors.Add(new Author("Perico","Los palotes"));
            BookWindow.ItemsCtrlAuthors.ItemsSource = AuthorsCollection;
        }

        /// <summary>
        /// Removes the last <see cref="IAuthor"/> from
        /// <see cref="AuthorInformation.Authors"/> and from the
        /// <see cref="IAuthor"/>s <see cref="ItemsControl"/>
        /// </summary>
        public void RemoveAuthor()
        {
            Book.RemoveLastAuthor();
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
            InputThemes();
        }
    }
}

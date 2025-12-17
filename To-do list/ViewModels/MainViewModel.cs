using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Data;
using System.Windows.Input;
using To_do_list.models;

namespace To_do_list.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private const string FileName = "notes.json";

        public ObservableCollection<Note> Notes { get; } = new();
        public ICollectionView FilteredNotes { get; }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (!SetProperty(ref _searchText, value)) return;
                FilteredNotes.Refresh();
            }
        }

        private Note? _selectedNote;
        public Note? SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (!SetProperty(ref _selectedNote, value)) return;
                ((RelayCommand)DeleteSelectedCommand).RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedNoteTodos));
                OnPropertyChanged(nameof(CompletedTodosCount));
                OnPropertyChanged(nameof(TotalTodosCount));
            }
        }

        // ✅ Todos de la nota seleccionada
        public ObservableCollection<TodoItem>? SelectedNoteTodos => SelectedNote?.Todos;

        // ✅ Contadores de tareas
        public int CompletedTodosCount => SelectedNote?.Todos?.Count(t => t.IsDone) ?? 0;
        public int TotalTodosCount => SelectedNote?.Todos?.Count ?? 0;

        private string _newTodoText = "";
        public string NewTodoText
        {
            get => _newTodoText;
            set => SetProperty(ref _newTodoText, value);
        }

        // ✅ Comandos
        public ICommand AddNoteCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
        public ICommand AddTodoCommand { get; }
        public ICommand DeleteTodoCommand { get; }

        public MainViewModel()
        {
            LoadNotes();

            FilteredNotes = CollectionViewSource.GetDefaultView(Notes);
            FilteredNotes.Filter = FilterNote;

            AddNoteCommand = new RelayCommand(AddNote);
            DeleteSelectedCommand = new RelayCommand(DeleteSelected, () => SelectedNote != null);
            AddTodoCommand = new RelayCommand(AddTodo, () => !string.IsNullOrWhiteSpace(NewTodoText));
            DeleteTodoCommand = new RelayCommand<TodoItem>(DeleteTodo);

            SelectedNote = Notes.FirstOrDefault();
        }

        private bool FilterNote(object obj)
        {
            if (obj is not Note n) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            return (n.Title?.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ?? false)
                || (n.Content?.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ?? false);
        }

        public void AddNote()
        {
            var note = new Note
            {
                Title = "Nueva nota",
                Content = ""
            };
            Notes.Insert(0, note);
            SelectedNote = note;
            SaveNotes();
            FilteredNotes.Refresh();
        }

        public void DeleteSelected()
        {
            if (SelectedNote == null) return;
            var toRemove = SelectedNote;
            Notes.Remove(toRemove);
            SelectedNote = Notes.FirstOrDefault();
            SaveNotes();
            FilteredNotes.Refresh();
        }

        // ✅ Agregar TODO
        public void AddTodo()
        {
            if (SelectedNote == null || string.IsNullOrWhiteSpace(NewTodoText)) return;

            var todo = new TodoItem
            {
                Text = NewTodoText.Trim(),
                IsDone = false
            };

            SelectedNote.Todos.Add(todo);
            NewTodoText = "";
            OnPropertyChanged(nameof(CompletedTodosCount));
            OnPropertyChanged(nameof(TotalTodosCount));
            ((RelayCommand)AddTodoCommand).RaiseCanExecuteChanged();
            TouchSelected();
        }

        // ✅ Eliminar TODO
        public void DeleteTodo(TodoItem? todo)
        {
            if (SelectedNote == null || todo == null) return;

            SelectedNote.Todos.Remove(todo);
            OnPropertyChanged(nameof(CompletedTodosCount));
            OnPropertyChanged(nameof(TotalTodosCount));
            TouchSelected();
        }

        // ✅ Actualizar nota (timestamp + guardar)
        public void TouchSelected()
        {
            if (SelectedNote == null) return;

            if (string.IsNullOrWhiteSpace(SelectedNote.Title))
                SelectedNote.Title = "Nota sin título";

            SelectedNote.UpdatedAt = System.DateTime.Now;
            OnPropertyChanged(nameof(CompletedTodosCount));
            OnPropertyChanged(nameof(TotalTodosCount));
            SaveNotes();
            FilteredNotes.Refresh();
        }

        // ✅ Guardar/Cargar JSON
        private void SaveNotes()
        {
            var json = JsonSerializer.Serialize(Notes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }

        private void LoadNotes()
        {
            if (!File.Exists(FileName)) return;

            var json = File.ReadAllText(FileName);
            var notes = JsonSerializer.Deserialize<ObservableCollection<Note>>(json);

            if (notes == null) return;

            Notes.Clear();
            foreach (var n in notes)
                Notes.Add(n);
        }

        public void SaveOnExit() => SaveNotes();
    }
}

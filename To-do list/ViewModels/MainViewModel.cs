using System.Collections.ObjectModel;
using System.Linq;                 // <-- IMPORTANTE
using System.Windows.Input;
using To_do_list.models;

namespace To_do_list.ViewModels;

public class MainViewModel : ObservableObject
{
    public ObservableCollection<Note> Notes { get; } = new();

    private Note? _selectedNote;
    public Note? SelectedNote
    {
        get => _selectedNote;
        set
        {
            if (SetProperty(ref _selectedNote, value))
            {
                // si tienes botones que dependen de SelectedNote != null
                (DeleteNoteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand NewNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    public MainViewModel()
    {
        NewNoteCommand = new RelayCommand(_ => NewNote());
        DeleteNoteCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedNote != null);

        // Datos demo
        Notes.Add(new Note { Title = "Bienvenido", Content = "Primera nota 👋" });
        Notes.Add(new Note { Title = "Checklist", Content = "Luego agregamos tareas aquí." });
        SelectedNote = Notes.FirstOrDefault();
    }

    private void NewNote()
    {
        var n = new Note { Title = "Nueva nota", Content = "" };
        Notes.Insert(0, n);
        SelectedNote = n;
    }

    private void DeleteSelected()
    {
        if (SelectedNote == null) return;

        var toRemove = SelectedNote;
        Notes.Remove(toRemove);
        SelectedNote = Notes.FirstOrDefault();
    }
}

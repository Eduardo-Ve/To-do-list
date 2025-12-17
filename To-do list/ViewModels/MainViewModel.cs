using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using To_do_list.models;

namespace To_do_list.ViewModels;

public class MainViewModel : ObservableObject
{
    public ObservableCollection<Note> Notes { get; } = new();

    private Note? _selectedNote;
    public Note? SelectedNote
    {
        get => _selectedNote;
        set => SetProperty(ref _selectedNote, value);
    }

    public ICommand NewNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    public MainViewModel()
    {
        // Datos demo
        Notes.Add(new Note { Title = "Bienvenido", Content = "Primera nota 👋" });
        Notes.Add(new Note { Title = "Checklist", Content = "Luego agregamos tareas aquí." });
        SelectedNote = Notes.FirstOrDefault();

        NewNoteCommand = new RelayCommand(_ => NewNote());
        DeleteNoteCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedNote != null);
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

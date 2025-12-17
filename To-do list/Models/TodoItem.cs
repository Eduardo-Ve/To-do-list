using To_do_list.ViewModels;

namespace To_do_list.models
{
    public class TodoItem : ObservableObject
    {
        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }

        private string _text = "";
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
    }
}

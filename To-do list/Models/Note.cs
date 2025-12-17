using System;
using System.Collections.ObjectModel;
using To_do_list.ViewModels;

namespace To_do_list.models
{
    public class Note : ObservableObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private string _title = "Nueva nota";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _content = "";
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        private DateTime _updatedAt = DateTime.Now;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => SetProperty(ref _updatedAt, value);
        }

        public ObservableCollection<TodoItem> Todos { get; set; } = new();
    }
}

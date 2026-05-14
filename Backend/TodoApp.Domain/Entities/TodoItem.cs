namespace TodoApp.Domain.Entities
{
        public class TodoItem
        {
            public Guid Id { get; set; }

            public string Task { get; set; } = string.Empty;

            public bool IsCompleted { get; set; }

            public DateTime CreatedAt { get; set; }



            public TodoItem(string task)
            {
            if (string.IsNullOrEmpty(task?.Trim()))
                throw new ArgumentException("Task cannot be null or empty", nameof(Task));

                Id = Guid.NewGuid();
                Task = task;
                IsCompleted = false;
                CreatedAt = DateTime.UtcNow;
            }
    }


}

namespace ToDoList.Dtos
{
    public record TodoListDto
    {
#pragma warning disable CS8618
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DataAccessLibrary.Models.Task> TaskList { get; set; }
    }
}

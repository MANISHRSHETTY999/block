using AElf.Contracts.TestKit;
using AElf.Kernel;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using Shouldly;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace AElf.Contracts.ToDo.Tests
{
    public class ToDoTests : ToDoContractTestBase
    {
        private async Task InitializeContractAsync()
        {
            var result = await ToDoContractStub.Initialize.SendAsync(new Empty());
            result.TransactionResult.Status.ShouldBe(TransactionResultStatus.Mined);
        }

        [Fact]
        public async Task Initialize_Contract_Test()
        {
            await InitializeContractAsync();

            var status = await ToDoContractStub.GetInitialStatus.CallAsync(new Empty());
            status.Value.ShouldBeTrue();
        }

        [Fact]
        public async Task CreateTask_Test()
        {
            await InitializeContractAsync();

            var taskInput = new TaskInput
            {
                Name = "Test Task",
                Description = "This is a test task.",
                Category = "General"
            };

            var result = await ToDoContractStub.CreateTask.SendAsync(taskInput);
            result.TransactionResult.Status.ShouldBe(TransactionResultStatus.Mined);

            var taskId = result.Output.Value;
            var task = await ToDoContractStub.GetTask.CallAsync(new StringValue { Value = taskId });

            task.TaskId.ShouldBe(taskId);
            task.Name.ShouldBe(taskInput.Name);
            task.Description.ShouldBe(taskInput.Description);
            task.Category.ShouldBe(taskInput.Category);
            task.Status.ShouldBe("pending");
        }

        [Fact]
        public async Task UpdateTask_Test()
        {
            await InitializeContractAsync();

            var taskInput = new TaskInput
            {
                Name = "Task to Update",
                Description = "Update this task.",
                Category = "General"
            };

            var createResult = await ToDoContractStub.CreateTask.SendAsync(taskInput);
            var taskId = createResult.Output.Value;

            var updateInput = new TaskUpdateInput
            {
                TaskId = taskId,
                Name = "Updated Task",
                Description = "Task has been updated.",
                Category = "Work",
                Status = "completed"
            };

            var updateResult = await ToDoContractStub.UpdateTask.SendAsync(updateInput);
            updateResult.TransactionResult.Status.ShouldBe(TransactionResultStatus.Mined);

            var updatedTask = await ToDoContractStub.GetTask.CallAsync(new StringValue { Value = taskId });

            updatedTask.Name.ShouldBe(updateInput.Name);
            updatedTask.Description.ShouldBe(updateInput.Description);
            updatedTask.Category.ShouldBe(updateInput.Category);
            updatedTask.Status.ShouldBe(updateInput.Status);
        }

        [Fact]
        public async Task DeleteTask_Test()
        {
            await InitializeContractAsync();

            var taskInput = new TaskInput
            {
                Name = "Task to Delete",
                Description = "This task will be deleted.",
                Category = "General"
            };

            var createResult = await ToDoContractStub.CreateTask.SendAsync(taskInput);
            var taskId = createResult.Output.Value;

            var deleteResult = await ToDoContractStub.DeleteTask.SendAsync(new StringValue { Value = taskId });
            deleteResult.TransactionResult.Status.ShouldBe(TransactionResultStatus.Mined);

            var deletedTask = await ToDoContractStub.GetTask.CallAsync(new StringValue { Value = taskId });
            deletedTask.Name.ShouldBe("Task not found.");
        }

        [Fact]
        public async Task ListTasks_Test()
        {
            await InitializeContractAsync();

            var taskInput1 = new TaskInput
            {
                Name = "Task 1",
                Description = "First task.",
                Category = "General"
            };

            var taskInput2 = new TaskInput
            {
                Name = "Task 2",
                Description = "Second task.",
                Category = "Work"
            };

            await ToDoContractStub.CreateTask.SendAsync(taskInput1);
            await ToDoContractStub.CreateTask.SendAsync(taskInput2);

            var taskList = await ToDoContractStub.ListTasks.CallAsync(new StringValue { Value = DefaultSender.ToString() });

            taskList.Tasks.Count.ShouldBe(2);
        }
    }
}
